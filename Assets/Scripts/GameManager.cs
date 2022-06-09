using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Main;

    [HideInInspector] public UnityEvent OnGameStarted;

    [SerializeField] private CameraController cameraController;
    [SerializeField] private LevelsConfig levelsConfig;
    [SerializeField, Min(0)] private int levelIndex;
    [SerializeField] int arrowsOnSceneLimit = 5;
    [Space]
    [SerializeField] UIController uiController;

    private Level _currentLevel;
    private int _levelIndex = 0;
    private PlayerController _playerController;
    private RaycastManager _hitManager;
    
    private void Awake()
    {
        if(Main != null)
        {
            Destroy(gameObject);
            return;
        }

        Main = this;

        _hitManager = GetComponent<RaycastManager>();
    }

    private void Start()
    {
        LevelInit(levelsConfig.GetLevel(_levelIndex), _levelIndex);
    }

    private void LevelInit(Level level, int index)
    {
        _currentLevel = Instantiate(level);
        _currentLevel.OnLevelComplete.AddListener(OnLevelComplete);


        _playerController = _currentLevel.Initialize(index);
        _playerController.ArrowsLimit = arrowsOnSceneLimit;
        _playerController.OnShoot.AddListener(UpdateCounter);
        cameraController.Initialize(_playerController.transform);

        _currentLevel.OnTargetsOnLevelUpdate.AddListener(uiController.UpdateLevelProgressBar);
        uiController.UpdateLevelIndex((_currentLevel.Index + 1).ToString());
        uiController.UpdateLevelProgressBar(1, 1);

        _hitManager.Initialize(cameraController.Camera);
        _hitManager.OnHit.AddListener(_playerController.Shoot);

        StartGame();
    }

    public void StartGame()
    {
        uiController.ShowPanel(UiPanel.GamePanel);
        _playerController.Health.OnDeath.AddListener(OnGameEnd);
        OnGameStarted?.Invoke();
        UpdateCounter(_playerController.Ammo);
    }

    private void OnLevelComplete()
    {
        uiController.ShowPanel(UiPanel.WinPanel);
    }
    private void OnGameEnd()
    {
        uiController.ShowPanel(UiPanel.FailPanel);
    }

    public void RestartLevel()
    {
        _currentLevel.Destroy();
        if (_levelIndex >= levelsConfig.Count) _levelIndex = 0;
        LevelInit(levelsConfig.GetLevel(_levelIndex), _levelIndex);
    }

    public void NextLevel()
    {
        _currentLevel.Destroy();
        _levelIndex++;
        if (_levelIndex >= levelsConfig.Count) _levelIndex = 0;
        LevelInit(levelsConfig.GetLevel(_levelIndex), _levelIndex);
    }


    private void UpdateCounter(int count)
    {
        uiController.UpdateCounter(count.ToString());
    }

    private void OnDestroy()
    {
        OnGameStarted.RemoveAllListeners();
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Main;

    [SerializeField] private CameraController cameraController;
    [SerializeField] private LevelsConfig levelsConfig;
    [SerializeField, Min(0)] private int levelIndex;
    [SerializeField] int arrowsOnSceneLimit = 5;
    [Space]
    [SerializeField] Text counter;

    private Level _currentLevel;
    private PlayerController _playerController;
    private RaycastManager _hitManager;
    
    public UnityEvent OnGameStarted;

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
        _currentLevel = Instantiate(levelsConfig.GetLevel(levelIndex));
        _currentLevel.OnLevelComplete.AddListener(OnGameEnd);


        _playerController = _currentLevel.Initialize();
        _playerController.ArrowsLimit = arrowsOnSceneLimit;
        _playerController.OnShoot.AddListener(UpdateCounter);
        cameraController.Initialize(_playerController.transform);


        _hitManager.Initialize(cameraController.Camera);
        _hitManager.OnHit.AddListener(_playerController.Shoot);

        StartGame();
    }

    [ContextMenu("Utils/StartGame")]
    public void StartGame()
    {
        OnGameStarted?.Invoke();
        UpdateCounter(_playerController.Ammo);
    }

    [ContextMenu("Utils/NextPosition")]
    public void NextPosition()
    {
        _currentLevel.PlayerNextPoint();
    }

    private void OnGameEnd()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateCounter(int count)
    {
        counter.text = "Arrows: " + count;
    }

    private void OnDestroy()
    {
        OnGameStarted.RemoveAllListeners();
    }
}

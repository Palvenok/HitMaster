using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Main;

    [SerializeField] private CameraController cameraController;
    [SerializeField] private LevelsConfig levelsConfig;
    [SerializeField, Min(0)] private int levelIndex;

    private Level _currentLevel;
    private PlayerController _playerController;
    private HitManager _hitManager;
    
    public UnityEvent OnGameStarted;

    private void Awake()
    {
        if(Main != null)
        {
            Destroy(gameObject);
            return;
        }

        Main = this;

        _hitManager = GetComponent<HitManager>();
    }

    private void Start()
    {
        _currentLevel = Instantiate(levelsConfig.GetLevel(levelIndex));


        _playerController = _currentLevel.Initialize();
        cameraController.Initialize(_playerController.transform);


        _hitManager.Initialize(cameraController.Camera);
        _hitManager.OnHit.AddListener(_playerController.Shoot);

        StartGame();
    }

    [ContextMenu("Utils/StartGame")]
    public void StartGame()
    {
        OnGameStarted?.Invoke();
    }

    [ContextMenu("Utils/NextPosition")]
    public void NextPosition()
    {
        _currentLevel.PlayerNextPoint();
    }

    private void OnDestroy()
    {
        OnGameStarted.RemoveAllListeners();
    }
}

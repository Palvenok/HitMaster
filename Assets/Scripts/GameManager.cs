using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Main;


    [SerializeField] private CameraController camera;
    [SerializeField] private LevelsConfig levelsConfig;
    [SerializeField, Min(0)] private int levelIndex;
    
    private Level _currentLevel;
    private PlayerController _playerController;
    
    public UnityEvent OnGameStarted;

    private void Awake()
    {
        if(Main != null)
        {
            Destroy(gameObject);
            return;
        }

        Main = this;
    }

    private void Start()
    {
        _currentLevel = Instantiate(levelsConfig.GetLevel(levelIndex));
        _playerController = _currentLevel.Instance();

        camera.Instance(_playerController.transform);
    }

    [ContextMenu("Utils/StartGame")]
    public void StartGame()
    {
        OnGameStarted?.Invoke();
    }

    public void NextPosition()
    {
        _currentLevel.PlayerNextPoint();
    }

    private void OnDestroy()
    {
        OnGameStarted.RemoveAllListeners();
    }
}

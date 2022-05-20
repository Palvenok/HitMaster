using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private CinemachineVirtualCamera _virtualCamera;
    private Transform _player;

    public Camera Camera => _camera;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void Initialize(Transform player)
    {
        _player = player;

        _virtualCamera.LookAt = player;

        GameManager.Main.OnGameStarted.AddListener(OnGameStarted);
    }

    private void OnGameStarted()
    {
        _virtualCamera.Follow = _player;
    }
}

using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera _camera;
    private Transform _player;

    private void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
    }

    public void Instance(Transform player)
    {
        _player = player;

        _camera.LookAt = player;

        GameManager.Main.OnGameStarted.AddListener(OnGameStarted);
    }

    private void OnGameStarted()
    {
        _camera.Follow = _player;
    }
}

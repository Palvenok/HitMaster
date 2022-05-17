using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera camera;
    private Transform player;

    private void Awake()
    {
        camera = GetComponent<CinemachineVirtualCamera>();
    }

    public void Instance(Transform player)
    {
        this.player = player;

        camera.LookAt = player;

        GameManager.Main.OnGameStarted.AddListener(OnGameStarted);
    }

    private void OnGameStarted()
    {
        camera.Follow = player;
    }
}

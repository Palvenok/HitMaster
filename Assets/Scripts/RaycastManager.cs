using UnityEngine;
using UnityEngine.Events;

public class RaycastManager : MonoBehaviour
{
    [SerializeField] private LayerMask mask;

    private Camera _camera;
    private bool _isEnabled;

    public UnityEvent<Vector3> OnHit;

    public void Initialize(Camera camera)
    {
        _camera = camera;
        _isEnabled = true;
    }

    public bool ChangeStatus()
    {
        _isEnabled = !_isEnabled;
        return _isEnabled;
    }

    private void Update()
    {
        if (!_isEnabled) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 100f, mask);
            OnHit?.Invoke(hit.point);
        }
    }

    private void OnDestroy()
    {
        OnHit.RemoveAllListeners();
    }
}

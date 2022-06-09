using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health health;

    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponentInChildren<Slider>();
    }

    private void FixedUpdate()
    {
        _slider.value = Utils.Map(health.Value, 0, health.MaxValue, 0, 1);
        transform.LookAt(Camera.main.transform.position);
    }
}

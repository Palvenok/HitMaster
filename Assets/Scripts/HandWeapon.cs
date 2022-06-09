using UnityEngine;

public class HandWeapon : MonoBehaviour
{
    [SerializeField] private float damage = 100;

    public bool IsEnabled { get; set; } = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsEnabled) return;

        if (other.CompareTag("Player"))
        {
            other.GetComponent<Health>()?.TakeDamage(damage);
        }
    }
}

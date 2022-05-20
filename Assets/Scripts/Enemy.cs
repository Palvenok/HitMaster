using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();

        _health.OnDeath.AddListener(OnEnemyDeath);
    }

    private void OnEnemyDeath()
    {
        Destroy(gameObject);
        ///TODO: death animations/ragdol
    }
}

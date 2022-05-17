using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    private Health _health;

    public UnityEvent OnDeath;

    private void Awake()
    {
        _health = GetComponent<Health>();

        _health.OnDeath.AddListener(OnEnemyDeath);
    }

    private void OnEnemyDeath()
    {
        OnDeath?.Invoke();
        ///TODO: death animations/ragdol
    }

    private void OnDestroy()
    {
        OnDeath.RemoveAllListeners();
    }
}

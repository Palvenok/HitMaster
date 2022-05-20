using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    private TargetGroup _currentTargetGroup;
    private Health _health;

    private void Awake()
    {
        _currentTargetGroup = GetComponentInParent<TargetGroup>();
        _health = GetComponent<Health>();

        _health.OnDeath.AddListener(OnEnemyDeath);
    }

    private void OnEnemyDeath()
    {
        _currentTargetGroup.RemoveEnemy(this);
        Destroy(gameObject);
        ///TODO: death animations/ragdol
    }
}

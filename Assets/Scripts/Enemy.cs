using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Canvas healthBar;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rigidBody;
    [Space]
    [SerializeField] private bool isStatic;
    [SerializeField] private Material[] deathMaterials;
    [SerializeField] private SkinnedMeshRenderer[] meshRenderers;

    private NavMeshAgent _agent;
    private TargetGroup _currentTargetGroup;
    private Health _health;

    private void Awake()
    {
        _currentTargetGroup = GetComponentInParent<TargetGroup>();
        _health = GetComponent<Health>();
        _agent = GetComponent<NavMeshAgent>();

        _health.OnDeath.AddListener(OnEnemyDeath);
    }

    public void MoveToTarget(Vector3 target)
    {
        if (isStatic) return;
        animator.SetTrigger((int)AnimatorStates.Walk);
        _agent.SetDestination(target);
    }

    private void OnEnemyDeath()
    {
        _currentTargetGroup.RemoveEnemy(this);
        rigidBody.isKinematic = false;
        if (isStatic) return;
        if(healthBar != null) healthBar.enabled = false;
        if (_agent != null) _agent.isStopped = true;
        animator.enabled = false;

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material = deathMaterials[i];
        }
    }
}

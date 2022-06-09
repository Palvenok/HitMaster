using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Canvas healthBar;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private HandWeapon weapon;
    [Space]
    [SerializeField] private bool isStatic;
    [SerializeField] private Material[] deathMaterials;
    [SerializeField] private SkinnedMeshRenderer[] meshRenderers;

    private NavMeshAgent _agent;
    private TargetGroup _currentTargetGroup;
    private Health _health;
    private Vector3 _target;

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
        _target = target;
        animator.SetTrigger((int)AnimatorStates.Walk);
        _agent.SetDestination(_target);
        StartCoroutine(WaitMoveComplete());
    }

    private IEnumerator WaitMoveComplete()
    {
        yield return new WaitForFixedUpdate();
        while ((_target - _agent.transform.position).magnitude > .6f)
        {
            yield return null;
        }
        animator.SetTrigger((int)AnimatorStates.Hook);
    }

    private void OnEnemyDeath()
    {
        _currentTargetGroup.RemoveEnemy(this);
        rigidBody.isKinematic = false;
        if (isStatic) return;
        if(healthBar != null) healthBar.enabled = false;
        if (_agent != null) 
        {
            _agent.isStopped = true;
            _agent.enabled = false;
        }
        animator.enabled = false;
        weapon.IsEnabled = false;

        rigidBody.velocity = Vector3.up * 10;

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material = deathMaterials[i];
        }
    }
}

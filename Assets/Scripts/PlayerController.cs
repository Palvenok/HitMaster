using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowStartPosition;
    [SerializeField] private float arrowLaunchForce;

    private int _ammoCount;
    private NavMeshAgent _agent;
    private Health _health;
    private Animator _animator;
    private AnimationEvents _events;
    private Vector3 _point;
    private Vector3 _targetPoint;
    private Enemy _target;
    private TargetGroup _targetGroup;

    public int Ammo
    {
        get { return _ammoCount; }
        set { _ammoCount = value; }
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();
        _animator = GetComponentInChildren<Animator>();
        _events = GetComponentInChildren<AnimationEvents>();
        _events.OnLaunch.AddListener(LaunchArrow);
    }

    public void MoveToPoint(Vector3 point)
    {
        _point = point;
        _agent.SetDestination(point);
        StartCoroutine(WaitMoveComplete());
    }

    public void SetTargetGroup(TargetGroup targetGroup)
    {
        _targetGroup = targetGroup;
    }

    public void LookAt(Vector3 target)
    {
        _agent.transform.LookAt(target);
    }

    public void Shoot(Vector3 point)
    {
        _animator.SetTrigger((int)AnimatorStates.Shoot);
        _targetPoint = point;
    }

    private void LaunchArrow()
    {
        var arrow = Instantiate(arrowPrefab, arrowStartPosition).GetComponent<Arrow>();
        arrow.Launch(arrowLaunchForce, _targetPoint);
        Debug.Log("Launched");
    }

    private IEnumerator WaitMoveComplete()
    {
        _animator.SetTrigger((int)AnimatorStates.Run);
        yield return new WaitForFixedUpdate();

        while(_agent.transform.position.x != _point.x && _agent.transform.position.z != _point.z)
        {
            yield return null;
        }


        _animator.SetTrigger((int)AnimatorStates.Idle);
        _target = _targetGroup.GetTarget();

        LookAt(_target.transform.position);
    }
}

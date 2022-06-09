using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public UnityEvent<string> OnHit;
    [HideInInspector] public UnityEvent<int> OnShoot;

    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowStartPosition;
    [SerializeField] private float arrowLaunchForce;
    
    private int _ammoCount;
    private int _arrowsLimit;
    private bool _isCanShoot;
    private NavMeshAgent _agent;
    private Animator _animator;
    private AnimationEvents _events;
    private Vector3 _point;
    private Vector3 _targetPoint;
    private Enemy _target;
    private TargetGroup _targetGroup;
    private List<Arrow> _arrowList = new List<Arrow>();

    public int Ammo
    {
        get { return _ammoCount; }
        set { _ammoCount = value; }
    }

    public int ArrowsLimit
    {
        set { _arrowsLimit = value; }
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _events = GetComponentInChildren<AnimationEvents>();
        _events.OnLaunch.AddListener(LaunchArrow);

        _isCanShoot = true;
    }

    public void MoveToPoint(Vector3 point)
    {
        _point = point;
        _agent.SetDestination(point);
        _isCanShoot = false;
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
        if(_ammoCount <= 0) return;
        if (!_isCanShoot) return;

        _ammoCount--;
        OnShoot?.Invoke(_ammoCount);
        _animator.SetTrigger((int)AnimatorStates.Shoot);
        _targetPoint = point;
    }

    private void LaunchArrow()
    {
        Arrow arrow;

        if (_arrowList.Count < _arrowsLimit)
        {
            arrow = Instantiate(arrowPrefab, arrowStartPosition.position, Quaternion.identity).GetComponent<Arrow>();
        }
        else
        {
            arrow = _arrowList[0];
            arrow.transform.position = arrowStartPosition.position;
            _arrowList.RemoveAt(0);
        }
        arrow.Launch(arrowLaunchForce, _targetPoint);
        arrow.OnHitStart.AddListener(() => { OnHit?.Invoke("Start"); });
        arrow.OnHit.AddListener(OnArrowHit);
        arrow.OnDestroed.AddListener((Arrow a) => { _arrowList.Remove(a); });
        _arrowList.Add(arrow);
    }

    private void OnArrowHit(Health health, Arrow arrow)
    {
        if (health == null) return;
        if (health.Value <= 0)
        {
            _target = _targetGroup?.GetTarget();
            if (_target == null)  return; 
            LookAt(_target.transform.position);
        }
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

        if (_targetGroup == null) yield break;
        _target = _targetGroup.GetTarget();
        _targetGroup.MoveEnemy();
        LookAt(_target.transform.position);
        _isCanShoot = true;
    }

    private void OnDestroy()
    {
        OnHit.RemoveAllListeners();
        OnShoot.RemoveAllListeners();
    }
}

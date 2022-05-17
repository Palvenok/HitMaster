using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Vector3 _point;
    private Enemy _target;
    private TargetGroup _targetGroup;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
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

    private IEnumerator WaitMoveComplete()
    {
        yield return new WaitForFixedUpdate();

        while(_agent.transform.position.x != _point.x && _agent.transform.position.z != _point.z)
        {
            yield return null;
        }

        _target = _targetGroup.GetTarget();

        LookAt(_target.transform.position);
    }
}

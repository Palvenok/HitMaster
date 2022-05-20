using UnityEngine;
using UnityEngine.Events;

public class Arrow : MonoBehaviour
{
    private Rigidbody _rb;

    public UnityEvent<Health, Arrow> OnHit;
    public UnityEvent OnHitStart;
    public UnityEvent OnHitEnd;
    public UnityEvent<Arrow> OnDestroed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Launch(float force, Vector3 targetPoint)
    {
        transform.LookAt(targetPoint);
        _rb.AddForce(transform.forward * force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;
        if (other.CompareTag("Start")) OnHitStart?.Invoke();
        if (other.CompareTag("Finish")) OnHitEnd?.Invoke();

        _rb.velocity = Vector3.zero;
        transform.parent = other.transform;

        var trigger = other.GetComponent<IBody>();
        var healh = other.GetComponentInParent<Health>();

        float damage = 0;

        if (trigger != null)
            switch (trigger.Type)
            {
                case TriggerType.Head:
                    damage = 50;
                    break;
                case TriggerType.Body:
                    damage = 8;
                    break;
                case TriggerType.Arm:
                    damage = 4;
                    break;
                case TriggerType.Leg:
                    damage = 4;
                    break;
            }

        healh?.TakeDamage(damage);
        OnHit?.Invoke(healh, this);
    }

    private void OnDestroy()
    {
        OnDestroed?.Invoke(this);
        OnDestroed.RemoveAllListeners();
        OnHitStart.RemoveAllListeners();
        OnHitEnd.RemoveAllListeners();
        OnHit.RemoveAllListeners();
    }
}

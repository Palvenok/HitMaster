using UnityEngine;
using UnityEngine.Events;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float headShotDamage = 50;
    [SerializeField] private float normalDamage = 8;
    [Space]
    [SerializeField] private ParticleSystem hitParticleSystem;

    private Rigidbody _rb;
    private Collider _collider;

    [HideInInspector] public UnityEvent<Health, Arrow> OnHit;
    [HideInInspector] public UnityEvent OnHitStart;
    [HideInInspector] public UnityEvent<Arrow> OnDestroed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public void Launch(float force, Vector3 targetPoint)
    {
        _collider.enabled = true;
        _rb.isKinematic = false;
        transform.parent = null;
        transform.LookAt(targetPoint);
        _rb.AddForce(transform.forward * force);
    }

    private void OnTriggerEnter(Collider other)
    {
        var pos = transform.position - transform.forward * .2f;
        if (other.CompareTag("Player")) return;
        if (other.CompareTag("Start")) OnHitStart?.Invoke();

        Instantiate(hitParticleSystem, transform);
        _collider.enabled = false;
        _rb.isKinematic = true;
        _rb.velocity = Vector3.zero;
        transform.parent = other.transform;
        transform.position = pos;

        var trigger = other.GetComponent<IBody>();
        var healh = other.GetComponentInParent<Health>();

        float damage = 0;

        if (trigger != null)
            switch (trigger.Type)
            {
                case TriggerType.Head:
                    damage = headShotDamage;
                    break;
                case TriggerType.Body:
                    damage = normalDamage;
                    break;
                case TriggerType.Arm:
                    damage = normalDamage * .4f;
                    break;
                case TriggerType.Leg:
                    damage = normalDamage * .4f;
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
        OnHit.RemoveAllListeners();
    }
}

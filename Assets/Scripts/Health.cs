using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField, Min(0)] private float health;
    [SerializeField] private float maxHealth;

    public float Value => health;
    public float MaxValue => maxHealth;

    public UnityEvent OnDeath;

    public void Instance()
    {
        Instance(maxHealth);
    }

    public void Instance(float health)
    {
        if (health < 0) health = 0;
        if (health > maxHealth) health = maxHealth;
        this.health = health;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) Death();
    }

    private void Death()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        OnDeath.RemoveAllListeners();
    }
}

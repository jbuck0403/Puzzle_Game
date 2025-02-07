using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 10;
    private float currentHealth;

    public Action<float> onDamageTaken;
    public Action onDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void ReceiveDamage(float damageDealt)
    {
        currentHealth -= damageDealt;
        onDamageTaken?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            onDeath?.Invoke();
        }
    }

    public void ReceiveHealing(float healingTaken)
    {
        currentHealth += healingTaken;
        Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    void Die()
    {
        onDeath?.Invoke();
    }
}

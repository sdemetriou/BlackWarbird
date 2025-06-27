using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public bool isDead;

    public delegate void OnDeath();
    public event OnDeath onDeathCallback;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        Debug.Log("damaged. Current Health: "+ currentHealth);
        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        isDead = true;
        Debug.Log("has died");
        onDeathCallback?.Invoke();
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        Debug.Log("healed. Current Health: "+ currentHealth);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

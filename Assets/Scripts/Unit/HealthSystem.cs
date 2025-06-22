using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class HealthSystem : MonoBehaviour
{

    public event EventHandler<float> onHealthChanged;
    private int MAX_HEALTH = 100;
    private int currentHealth;
    [SerializeField] private RagdollSpawner ragdollSpawner;

    void Awake()
    {
        currentHealth = MAX_HEALTH;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Debug.Log("Unit Dead");
            ragdollSpawner.GenerateRagdoll();
            Destroy(gameObject);
        }
        onHealthChanged?.Invoke(this, (float)currentHealth / MAX_HEALTH);
        Debug.Log("Current Health: " + currentHealth);
    }
}

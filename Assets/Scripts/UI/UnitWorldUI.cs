using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Unit unit;
    [SerializeField] private HealthSystem healthSystem;

    void Start()
    {
        healthSystem.onHealthChanged += HealthSystem_onHealthChanged;
    }
    void Awake()
    {
        SetHealthBar(1);
    }
    public void SetHealthBar(float normalizedHealth)
    {
        healthBar.fillAmount = normalizedHealth;
    }

    private void HealthSystem_onHealthChanged(object sender, float normalizedHealth)
    {
        SetHealthBar(normalizedHealth);
    }

}

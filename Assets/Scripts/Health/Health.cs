using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    [SerializeField] private HealthBarUI healthBar;
    private int maxHealth;
    private int currentHealth;
    private HealthEvent healthEvent;


    private void Awake()
    {
        healthEvent = GetComponent<HealthEvent>();
    }
    private void Start()
    {
        CallHealthEvent(0); // UI ������Ʈ ����
    }


    public void CallHealthEvent(int damageAmount)
    {
        // ����ü��/�ִ�ü�� (ü�º���), ����ü��, ������ �Ű������� �̺�Ʈ ȣ��
        healthEvent.CallHealthChangedEvent(
            ((float)currentHealth / (float)maxHealth), currentHealth, damageAmount);
    }

    public void SetHealthBar()
    {
        healthBar.SetHealthBar((float)currentHealth / (float)maxHealth);
    }

    public void SetStartingHealth(int startingHealth) // ����ü�� �ʱ�ȭ
    {
        this.maxHealth = startingHealth;
        currentHealth = startingHealth;
    }

    public void SetMaxHealth(int changeValue) // �ִ�ü�� ����
    {
        this.maxHealth += changeValue;

        if (currentHealth > maxHealth) 
            currentHealth = maxHealth; // �ִ�ü�� ��� �������� ��� ���

        SetHealthBar(); // �ִ�ü���� ����Ǿ����Ƿ�
    }

    public void SetCurrentHealth(int damageAmount) // ����ü�� ����
    {
        this.currentHealth -= damageAmount;
    }

    public int GetStartingHealth()
    {
        return maxHealth;
    }   
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    public float GetCurrentHealthRatio()
    {
        return (float)currentHealth / (float)maxHealth;
    }

    public void AddHealth(int healthPercent)
    {
        // ������ ü���� ���� ������ ��ȯ
        int healthIncrease = Mathf.RoundToInt((maxHealth * healthPercent) / 100f);

        int totalHealth = currentHealth + healthIncrease;

        if (totalHealth > maxHealth)
            currentHealth = maxHealth;
        else
            currentHealth = totalHealth;

        CallHealthEvent(0); // ü���� ����Ǿ����� UI������Ʈ�� ���� �̺�Ʈ ȣ��
    }
}

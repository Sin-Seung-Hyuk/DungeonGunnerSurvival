using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    //[SerializeField] private HealthBar healthBar;

    private int startingHealth;
    public int currentHealth { get; private set; }
    private HealthEvent healthEvent;
    private Player player;

    [HideInInspector] public bool isDamageable = true; // ������ �޴���
    [HideInInspector] public Enemy enemy;

    private void Awake()
    {
        healthEvent = GetComponent<HealthEvent>();
    }
    private void Start()
    {
        CallHealthEvent(0); // UI ������Ʈ ����

        player = GetComponent<Player>();
        enemy = GetComponent<Enemy>();

        //if (enemy != null && enemy.enemyDetails.isHealthBarDisplayed == true && healthBar != null)
        //{
        //    healthBar.EnableHealthBar();
        //}
        //else if (healthBar != null) healthBar.DisableHealthBar();
    }
    public void TakeDamage(int damageAmount)
    {
        if (isDamageable)
        {
            currentHealth -= damageAmount;
            CallHealthEvent(damageAmount);

            //if (healthBar != null)
            //    healthBar.SetHealthBar((float)currentHealth / (float)startingHealth);
        }
    }
    private void CallHealthEvent(int damageAmount)
    {
        // ����ü��/�ִ�ü�� (ü�º���), ����ü��, ������ �Ű������� �̺�Ʈ ȣ��
        healthEvent.CallHealthChangedEvent(
            ((float)currentHealth / (float)startingHealth), currentHealth, damageAmount);
    }

    public void SetStartingHealth(int startingHealth)
    {
        this.startingHealth = startingHealth;
        currentHealth = startingHealth;
    }

    public int GetStartingHealth()
    {
        return startingHealth;
    }

    public void AddHealth(int healthPercent)
    {
        // ������ ü���� ���� ������ ��ȯ
        int healthIncrease = Mathf.RoundToInt((startingHealth * healthPercent) / 100f);

        int totalHealth = currentHealth + healthIncrease;

        if (totalHealth > startingHealth)
            currentHealth = startingHealth;
        else
            currentHealth = totalHealth;

        CallHealthEvent(0); // ü���� ����Ǿ����� UI������Ʈ�� ���� �̺�Ʈ ȣ��
    }
}

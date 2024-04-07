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

    [HideInInspector] public bool isDamageable = true; // 데미지 받는지
    [HideInInspector] public Enemy enemy;

    private void Awake()
    {
        healthEvent = GetComponent<HealthEvent>();
    }
    private void Start()
    {
        CallHealthEvent(0); // UI 업데이트 위해

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
        // 현재체력/최대체력 (체력비율), 현재체력, 데미지 매개변수로 이벤트 호출
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
        // 증가할 체력의 양을 정수로 변환
        int healthIncrease = Mathf.RoundToInt((startingHealth * healthPercent) / 100f);

        int totalHealth = currentHealth + healthIncrease;

        if (totalHealth > startingHealth)
            currentHealth = startingHealth;
        else
            currentHealth = totalHealth;

        CallHealthEvent(0); // 체력이 변경되었으니 UI업데이트를 위해 이벤트 호출
    }
}

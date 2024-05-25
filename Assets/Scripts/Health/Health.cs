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
        CallHealthEvent(0); // UI 업데이트 위해
    }


    public void CallHealthEvent(int damageAmount)
    {
        // 현재체력/최대체력 (체력비율), 현재체력, 데미지 매개변수로 이벤트 호출
        healthEvent.CallHealthChangedEvent(
            ((float)currentHealth / (float)maxHealth), currentHealth, damageAmount);
    }

    public void SetHealthBar()
    {
        healthBar.SetHealthBar((float)currentHealth / (float)maxHealth);
    }

    public void SetStartingHealth(int startingHealth) // 시작체력 초기화
    {
        this.maxHealth = startingHealth;
        currentHealth = startingHealth;
    }

    public void SetMaxHealth(int changeValue) // 최대체력 변경
    {
        this.maxHealth += changeValue;

        if (currentHealth > maxHealth) 
            currentHealth = maxHealth; // 최대체력 장비 해제했을 경우 대비

        SetHealthBar(); // 최대체력이 변경되었으므로
    }

    public void SetCurrentHealth(int damageAmount) // 현재체력 변경
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
        // 증가할 체력의 양을 정수로 변환
        int healthIncrease = Mathf.RoundToInt((maxHealth * healthPercent) / 100f);

        int totalHealth = currentHealth + healthIncrease;

        if (totalHealth > maxHealth)
            currentHealth = maxHealth;
        else
            currentHealth = totalHealth;

        CallHealthEvent(0); // 체력이 변경되었으니 UI업데이트를 위해 이벤트 호출
    }
}

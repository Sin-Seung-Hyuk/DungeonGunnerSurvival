using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class ReciveContactDamage : MonoBehaviour
{
    [Header("접촉시 데미지 수치")]
    [SerializeField] private int contactDamageAmount;

    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    public void TakeContactDamage(int damage = 0)
    {
        if (contactDamageAmount > 0) damage = contactDamageAmount;

        health.TakeDamage(damage);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class ReciveContactDamage : MonoBehaviour
{
    [Header("���˽� ������ ��ġ")]
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

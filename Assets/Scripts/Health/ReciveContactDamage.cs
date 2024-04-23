using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class ReciveContactDamage : MonoBehaviour
{
    [Header("���˽� ������ ��ġ")]
    private int contactDamageAmount;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void TakeContactDamage(int damage = 0)
    {
        if (contactDamageAmount > 0) damage = contactDamageAmount;

        player.TakeDamage(damage);
    }
}

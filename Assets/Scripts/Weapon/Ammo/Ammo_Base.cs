using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_Base : Ammo // ���� �⺻���� ź
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        gameObject.SetActive(false); // �ε����� �ٷ� �����
    }
}

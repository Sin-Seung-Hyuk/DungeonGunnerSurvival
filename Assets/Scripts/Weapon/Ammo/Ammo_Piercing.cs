using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_Piercing : Ammo // 적을 관통하는 탄
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}

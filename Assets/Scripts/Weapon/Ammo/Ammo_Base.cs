using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_Base : Ammo // 가장 기본적인 탄
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        gameObject.SetActive(false); // 부딪히면 바로 사라짐
    }
}

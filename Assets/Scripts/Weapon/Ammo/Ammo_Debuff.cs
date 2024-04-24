using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Debuff_Type
{
    Slow,
    Burn
}

public class Ammo_Debuff : Ammo // µð¹öÇÁ Åº
{
    [SerializeField] Debuff_Type debuff_type;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        IDebuff enemy = collision.GetComponent<IDebuff>();

        if (enemy != null)
        {
            switch (debuff_type)
            {
                case Debuff_Type.Slow:
                    enemy.Debuff_Slow();
                    break;
                case Debuff_Type.Burn:
                    enemy.Debuff_Burn(ammoDamage);
                    break;
            }
        }

        gameObject.SetActive(false); // ºÎµúÈ÷¸é ¹Ù·Î »ç¶óÁü
    }
}

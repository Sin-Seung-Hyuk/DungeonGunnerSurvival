using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthObject 
{
                            // ź ������   ,  ���� ������ (���� ���)
    public int TakeDamage(int ammoDamage, out int damageAmount);
}

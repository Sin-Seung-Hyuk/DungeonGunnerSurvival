using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthObject 
{
                            // 탄 데미지   ,  실제 데미지 (방어력 계산)
    public int TakeDamage(int ammoDamage, out int damageAmount);
}

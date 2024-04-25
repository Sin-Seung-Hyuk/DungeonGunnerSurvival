using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsManager : Singleton<StatisticsManager> // ��� ����
{
    [SerializeField] private List<DamageStatistics> DamageStatisticsList = new List<DamageStatistics>(8);
    public int TotalEnemiesKill { get; private set; } // �� óġ�� ��
    public int TotalSpentGold { get; private set; } // �� ����� ���


    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        ClearDamageStatisticsList(); // ������ ���۵Ǹ� ��� �ʱ�ȭ
    }

    private void ClearDamageStatisticsList()
    {
        for (int i = 0; i < DamageStatisticsList.Count; ++i)
            DamageStatisticsList[i].dealt = 0;   
    }

    public void SetDamageStatistics(WeaponType weaponType, int damageAmount)
    {
        for (int i= 0; i < DamageStatisticsList.Count; ++i)
        {
            if (weaponType == DamageStatisticsList[i].weaponType)
            {
                DamageStatisticsList[i].dealt += damageAmount;
            }
        }
    }

    public int GetDamageStatistics(WeaponType weaponType)
    {
        for (int i = 0; i < DamageStatisticsList.Count; ++i)
        {
            if (weaponType == DamageStatisticsList[i].weaponType)
            {
                return DamageStatisticsList[i].dealt;
            }
        }

        return 0;
    }

}

[System.Serializable] 
public class DamageStatistics
{
    public WeaponType weaponType;
    public int dealt;
}
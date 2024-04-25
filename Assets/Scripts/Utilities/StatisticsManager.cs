using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsManager : Singleton<StatisticsManager> // 통계 관리
{
    [SerializeField] private List<DamageStatistics> DamageStatisticsList = new List<DamageStatistics>(8);
    public int TotalEnemiesKill { get; private set; } // 총 처치한 적
    public int TotalSpentGold { get; private set; } // 총 사용한 골드


    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        ClearDamageStatisticsList(); // 게임이 시작되면 통계 초기화
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
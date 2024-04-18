using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public string weaponName { get; private set; }
    public Sprite weaponSprite { get; private set; }
    public WeaponDetailsSO weaponDetail { get; private set; }
    public int weaponAmmoCapacity { get; private set; } // 탄창 용량
    public List<GameObject> weaponAmmoList { get; private set; } // 무기 탄
    

    // 무기의 스탯 (레벨업하여 스탯 상승 가능)
    public int weaponLevel; // 무기 레벨
    public int weaponBaseDamage; // 기본데미지
    public int weaponCriticChance; // 치명타 확률 (%)
    public int weaponCriticDamage; // 치명타 피해 (%)
    public float weaponFireRate; // 연사속도
    public float weaponReloadTime; // 재장전 속도

    // 실시간으로 인게임에서 사격하면서 바뀌는 변수들
    public int weaponAmmoRemaining; // 남은 탄약
    public bool isWeaponReloading;  // 재장전 여부
    public float weaponReloadTimer = 0f; // 재장전 남은시간
    public float weaponFireRateTimer; // 사격 남은시간


    public void InitializeWeapon(WeaponDetailsSO weaponDetails) // 무기 초기화
    {
        weaponDetail = weaponDetails; // 소리,이펙트 등에 접근하기 위해
        weaponName = weaponDetails.weaponName;
        weaponSprite = weaponDetails.weaponSprite;

        weaponLevel = 1;
        weaponBaseDamage = weaponDetails.weaponBaseDamage;
        weaponCriticChance = weaponDetails.weaponCriticChance;
        weaponCriticDamage = weaponDetails.weaponCriticDamage;
        weaponAmmoCapacity = weaponDetails.weaponAmmoCapacity;
        weaponFireRate = weaponDetails.weaponFireRate;
        weaponReloadTime = weaponDetails.weaponReloadTime;
        weaponAmmoList = weaponDetails.weaponAmmo;

        weaponAmmoRemaining = weaponDetails.weaponAmmoCapacity;
        weaponFireRateTimer = weaponDetails.weaponFireRate;
    }

    private void Update()
    {
        // 연사속도 
        if (weaponFireRateTimer > 0f)
            weaponFireRateTimer -= Time.deltaTime;
        else weaponFireRateTimer = weaponFireRate;

    }

    public GameObject GetCurrentAmmo(int level)
    {
        int idx = level / 10; // 무기 10레벨 찍으면 탄 업그레이드
        return weaponAmmoList[idx];
    }
                                                                          // % 계산 여부
    public void ChangeWeaponStat(PlayerStatType statType, float value, bool isPercent)
    {
        switch (statType)
        {
            case PlayerStatType.BaseDamage:
                if (isPercent)
                    weaponBaseDamage = Utilities.IncreaseByPercent(weaponBaseDamage, value);
                else weaponBaseDamage += (int)value;
                break;

            case PlayerStatType.CriticChance:
                weaponCriticChance += (int)value;
                break;

            case PlayerStatType.CriticDamage:
                weaponCriticDamage += (int)value;
                break;

            case PlayerStatType.FireRate:
                weaponFireRate = Utilities.DecreaseByPercent(weaponFireRate, value);
                break;

            case PlayerStatType.ReloadSpeed:
                weaponReloadTime = Utilities.DecreaseByPercent(weaponReloadTime, value);
                break;

            default:
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName { get; private set; }
    public Sprite weaponSprite { get; private set; }
    public WeaponDetailsSO weaponDetail { get; private set; }

    public int weaponBaseDamage; // 기본데미지
    public int weaponAmmoCapacity; // 탄창
    public float weaponFireRate; // 연사속도
    public float weaponReloadTime; // 재장전 속도


    public void InitializeWeapon(WeaponDetailsSO weaponDetails)
    {
        weaponDetail = weaponDetails; // 소리,이펙트 등에 접근하기 위해
        weaponName = weaponDetails.weaponName;
        weaponSprite = weaponDetails.weaponSprite;

        weaponBaseDamage = weaponDetails.weaponBaseDamage;
        weaponAmmoCapacity = weaponDetails.weaponAmmoCapacity;
        weaponFireRate = weaponDetails.weaponFireRate;
        weaponReloadTime = weaponDetails.weaponReloadTime;
    }
}

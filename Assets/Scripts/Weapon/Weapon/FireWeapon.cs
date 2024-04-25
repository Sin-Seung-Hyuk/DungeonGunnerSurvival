using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{
    [SerializeField] Transform weaponShootPosition; // 사격 포인트
    private FireWeaponEvent fireWeaponEvent;
    private WeaponFiredEvent weaponFiredEvent;
    private ReloadWeaponEvent reloadWeaponEvent;

    private void Awake()
    {
        fireWeaponEvent = GetComponent<FireWeaponEvent>();
        weaponFiredEvent = GetComponent<WeaponFiredEvent>();
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();

    }
    private void OnEnable()
    {
        fireWeaponEvent.OnFireWeapon += FireWeaponEvent_OnFireWeapon;
    }
    private void OnDisable()
    {
        fireWeaponEvent.OnFireWeapon -= FireWeaponEvent_OnFireWeapon;
    }



    private void FireWeaponEvent_OnFireWeapon(FireWeaponEvent arg1, FireWeaponEventArgs args)
    {
        WeaponFire(args);
    }

    private void WeaponFire(FireWeaponEventArgs fireWeaponEventArgs)
    {
        if (IsWeaponReadyToFire(fireWeaponEventArgs.weapon, fireWeaponEventArgs.weaponIndex))
        {
            FireAmmo(fireWeaponEventArgs.weapon,fireWeaponEventArgs.aimAngle, fireWeaponEventArgs.weaponAimDirectionVector, fireWeaponEventArgs.weaponIndex);
        }
    }

    private bool IsWeaponReadyToFire(Weapon weapon, int weaponIndex)
    {
        // 탄약이 남아있는지
        if (weapon.weaponAmmoRemaining <= 0)
        {
            reloadWeaponEvent.CallReloadWeaponEvent(weapon, weaponIndex); // 장전이벤트 호출
            return false;
        }
        // 지금 장전중이 아닐때
        if (weapon.isWeaponReloading)
            return false;
        // 연사속도가 남아있을때 (다음 사격까지 대기)
        if (weapon.weaponFireRateTimer > 0f)
            return false;

        return true;
    }

    private void FireAmmo(Weapon weapon, float aimAngle, Vector3 weaponAimDirectionVector, int weaponIndex)
    {
        GameObject ammoPrefab = weapon.GetCurrentAmmo(); // 현재 사용하고 있는 탄

        if (ammoPrefab != null)
        {
            // ammo에 오브젝트 풀에 등록된 Ammo프리팹이 가지고있는 IFireable 컴포넌트가 반환됨
            IFireable ammo = (IFireable)ObjectPoolManager.Instance.Release(ammoPrefab, weaponShootPosition.position, Quaternion.identity);

            ammo.InitializeAmmo(aimAngle, weaponAimDirectionVector, weapon);

            weapon.weaponAmmoRemaining--; // 남은 탄 감소

            weaponFiredEvent.CallWeaponFiredEvent(weapon, weaponIndex); // 사격했음을 알리는 이벤트 호출

            WeaponSoundEffect(weapon); // 사격 사운드
        }
    }

    private void WeaponSoundEffect(Weapon weapon)
    {
        if (weapon.weaponDetail.weaponFiringSoundEffect != null)
        {
            SoundEffectManager.Instance.PlaySoundEffect(
                weapon.weaponDetail.weaponFiringSoundEffect);
        }
    }
}


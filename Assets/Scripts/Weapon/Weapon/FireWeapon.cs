using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{
    [SerializeField] Transform weaponShootPosition;
    private FireWeaponEvent fireWeaponEvent;

    private void Awake()
    {
        fireWeaponEvent = GetComponent<FireWeaponEvent>();
        
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
        if (IsWeaponReadyToFire(fireWeaponEventArgs.weapon))
        {
            FireAmmo(fireWeaponEventArgs.weapon,fireWeaponEventArgs.aimAngle, fireWeaponEventArgs.weaponAimDirectionVector);
        }
    }

    private bool IsWeaponReadyToFire(Weapon weapon)
    {
        // 탄약이 남아있는지
        if (weapon.weaponAmmoRemaining <= 0)
            return false;
        // 지금 장전중이 아닐때
        if (weapon.isWeaponReloading)
            return false;
        // 연사속도가 남아있을때 (다음 사격까지 대기)
        if (weapon.weaponFireRateTimer > 0f)
            return false;

        return true;
    }

    private void FireAmmo(Weapon weapon, float aimAngle, Vector3 weaponAimDirectionVector)
    {
        Ammo currentAmmo = weapon.GetCurrentAmmo(weapon.weaponLevel); // 현재 무기의 탄

        if (currentAmmo != null)
        {
            GameObject ammoPrefab = currentAmmo.gameObject;

            float ammoSpeed = 15f;
            // ammo에 오브젝트 풀에 등록된 Ammo프리팹이 가지고있는 IFireable 컴포넌트가 반환됨
            IFireable ammo = (IFireable)ObjectPoolManager.Instance.Release(ammoPrefab, weaponShootPosition.position, Quaternion.identity);

            ammo.InitializeAmmo(aimAngle, weaponAimDirectionVector, weapon.weaponRange, ammoSpeed, 10);

            weapon.weaponAmmoRemaining--; // 남은 탄 감소

            //weaponFiredEvent.CallWeaponFiredEvent(activeWeapon.GetCurrentWeapon()); // 사격했음을 알리는 이벤트 호출

            //WeaponShootEffect(aimAngle); // 사격 효과
            //WeaponSoundEffect(); // 사격 사운드
        }
    }
}


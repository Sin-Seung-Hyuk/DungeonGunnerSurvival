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
        // ź���� �����ִ���
        if (weapon.weaponAmmoRemaining <= 0)
            return false;
        // ���� �������� �ƴҶ�
        if (weapon.isWeaponReloading)
            return false;
        // ����ӵ��� ���������� (���� ��ݱ��� ���)
        if (weapon.weaponFireRateTimer > 0f)
            return false;

        return true;
    }

    private void FireAmmo(Weapon weapon, float aimAngle, Vector3 weaponAimDirectionVector)
    {
        Ammo currentAmmo = weapon.GetCurrentAmmo(weapon.weaponLevel); // ���� ������ ź

        if (currentAmmo != null)
        {
            GameObject ammoPrefab = currentAmmo.gameObject;

            float ammoSpeed = 15f;
            // ammo�� ������Ʈ Ǯ�� ��ϵ� Ammo�������� �������ִ� IFireable ������Ʈ�� ��ȯ��
            IFireable ammo = (IFireable)ObjectPoolManager.Instance.Release(ammoPrefab, weaponShootPosition.position, Quaternion.identity);

            ammo.InitializeAmmo(aimAngle, weaponAimDirectionVector, weapon.weaponRange, ammoSpeed, 10);

            weapon.weaponAmmoRemaining--; // ���� ź ����

            //weaponFiredEvent.CallWeaponFiredEvent(activeWeapon.GetCurrentWeapon()); // ��������� �˸��� �̺�Ʈ ȣ��

            //WeaponShootEffect(aimAngle); // ��� ȿ��
            //WeaponSoundEffect(); // ��� ����
        }
    }
}


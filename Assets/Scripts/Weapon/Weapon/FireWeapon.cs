using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{
    [SerializeField] Transform weaponShootPosition; // ��� ����Ʈ
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
        // ź���� �����ִ���
        if (weapon.weaponAmmoRemaining <= 0)
        {
            reloadWeaponEvent.CallReloadWeaponEvent(weapon, weaponIndex); // �����̺�Ʈ ȣ��
            return false;
        }
        // ���� �������� �ƴҶ�
        if (weapon.isWeaponReloading)
            return false;
        // ����ӵ��� ���������� (���� ��ݱ��� ���)
        if (weapon.weaponFireRateTimer > 0f)
            return false;

        return true;
    }

    private void FireAmmo(Weapon weapon, float aimAngle, Vector3 weaponAimDirectionVector, int weaponIndex)
    {
        GameObject ammoPrefab = weapon.GetCurrentAmmo(); // ���� ����ϰ� �ִ� ź

        if (ammoPrefab != null)
        {
            // ammo�� ������Ʈ Ǯ�� ��ϵ� Ammo�������� �������ִ� IFireable ������Ʈ�� ��ȯ��
            IFireable ammo = (IFireable)ObjectPoolManager.Instance.Release(ammoPrefab, weaponShootPosition.position, Quaternion.identity);

            ammo.InitializeAmmo(aimAngle, weaponAimDirectionVector, weapon);

            weapon.weaponAmmoRemaining--; // ���� ź ����

            weaponFiredEvent.CallWeaponFiredEvent(weapon, weaponIndex); // ��������� �˸��� �̺�Ʈ ȣ��

            WeaponSoundEffect(weapon); // ��� ����
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


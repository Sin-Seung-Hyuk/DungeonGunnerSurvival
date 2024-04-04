using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponAimEvent))]
public class WeaponAim : MonoBehaviour
{
    // �ѱ��� ���ӵ��� ���ư�
    [SerializeField] private Transform weaponShootPoint;
    private WeaponAimEvent weaponAimEvent;

    private void Awake()
    {
        weaponAimEvent = GetComponent<WeaponAimEvent>();
    }

    private void OnEnable()
    {
        weaponAimEvent.OnWeaponAim += WeaponAimEvent_OnWeaponAim;
    }
    private void OnDisable()
    {
        weaponAimEvent.OnWeaponAim -= WeaponAimEvent_OnWeaponAim;
    }

    private void WeaponAimEvent_OnWeaponAim(WeaponAimEvent @event, WeaponAimEventArgs args)
    {
        Aim(args.aimDirection, args.aimAngle);
    }

    private void Aim(AimDirection aimDirection, float aimAngle)
    {
        // ���� �ѱ� ���Ϸ��� ȸ����Ű�� (z�� ȸ��)
        weaponShootPoint.eulerAngles = new Vector3(0f, 0f, aimAngle);

        switch (aimDirection)
        {
            case AimDirection.Left:
            case AimDirection.UpLeft:
                weaponShootPoint.localScale = new Vector3(1f, -1f, 0); // �������� 
                break;

            case AimDirection.Up:
            case AimDirection.UpRight:
            case AimDirection.Right:
            case AimDirection.Down:
                weaponShootPoint.localScale = new Vector3(1f, 1f, 0f); // ����������
                break;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponAimEvent))]
public class WeaponAim : MonoBehaviour
{
    // 총구가 에임따라 돌아감
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
        // 무기 총구 오일러각 회전시키기 (z축 회전)
        weaponShootPoint.eulerAngles = new Vector3(0f, 0f, aimAngle);

        switch (aimDirection)
        {
            case AimDirection.Left:
            case AimDirection.UpLeft:
                weaponShootPoint.localScale = new Vector3(1f, -1f, 0); // 왼쪽으로 
                break;

            case AimDirection.Up:
            case AimDirection.UpRight:
            case AimDirection.Right:
            case AimDirection.Down:
                weaponShootPoint.localScale = new Vector3(1f, 1f, 0f); // 오른쪽으로
                break;
        }
    }
}

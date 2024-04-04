using System;
using System.Collections.Generic;
using UnityEngine;

public class FireWeaponEvent : MonoBehaviour
{
    public event Action<FireWeaponEvent, FireWeaponEventArgs> OnFireWeapon;

    public void CallFireWeaponEvent(Weapon weapon, AimDirection aimDirection, float aimAngle, Vector3 weaponAimDirectionVector)
    {
        OnFireWeapon?.Invoke(this, new FireWeaponEventArgs()
        {
            weapon = weapon,
            aimDirection = aimDirection,
            aimAngle = aimAngle,
            weaponAimDirectionVector = weaponAimDirectionVector
        });
    }
}

public class FireWeaponEventArgs : EventArgs
{
    public Weapon weapon;
    public AimDirection aimDirection;
    public float aimAngle;
    public Vector3 weaponAimDirectionVector;
}
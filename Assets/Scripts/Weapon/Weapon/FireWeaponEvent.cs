using System;
using System.Collections.Generic;
using UnityEngine;

public class FireWeaponEvent : MonoBehaviour
{
    public event Action<FireWeaponEvent, FireWeaponEventArgs> OnFireWeapon;

    public void CallFireWeaponEvent(Weapon weapon, float aimAngle, Vector3 weaponAimDirectionVector)
    {
        OnFireWeapon?.Invoke(this, new FireWeaponEventArgs()
        {
            weapon = weapon,
            aimAngle = aimAngle,
            weaponAimDirectionVector = weaponAimDirectionVector
        });
    }
}

public class FireWeaponEventArgs : EventArgs
{
    public Weapon weapon;
    public float aimAngle;
    public Vector3 weaponAimDirectionVector;
}
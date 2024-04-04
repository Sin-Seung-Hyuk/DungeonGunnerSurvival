using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAimEvent : MonoBehaviour
{
    public event Action<WeaponAimEvent, WeaponAimEventArgs> OnWeaponAim;

    public void CallWeaponAim(AimDirection _aimDirection, float _aimAngle, Vector3 _aimDirectionVector)
    {
        OnWeaponAim?.Invoke(this, new WeaponAimEventArgs()
        {
            aimDirection = _aimDirection,
            aimAngle = _aimAngle,
            aimDirectionVector = _aimDirectionVector
        });
    }
}

public class WeaponAimEventArgs : EventArgs
{
    public AimDirection aimDirection;
    public float aimAngle;
    public Vector3 aimDirectionVector;
}
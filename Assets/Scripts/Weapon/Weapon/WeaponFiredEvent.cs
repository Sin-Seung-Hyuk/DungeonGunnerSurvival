using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponFiredEvent : MonoBehaviour
{
    public event Action<WeaponFiredEvent, WeaponFiredEventArgs> OnWeaponFired;

    public void CallWeaponFiredEvent(Weapon weapon, int weaponIndex)
    {
        OnWeaponFired?.Invoke(this, new WeaponFiredEventArgs() { weapon = weapon, weaponIndex = weaponIndex });
    }
}

public class WeaponFiredEventArgs : EventArgs
{
    public Weapon weapon;
    public int weaponIndex; // 플레이어의 몇번째 무기인지
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReloadedEvent : MonoBehaviour
{
    public event Action<WeaponReloadedEvent, WeaponReloadedEventArgs> OnWeaponReloaded;

    public void CallWeaponReloadedEvent(Weapon weapon,int weaponIndex)
    {
        OnWeaponReloaded?.Invoke(this, new WeaponReloadedEventArgs() { weapon = weapon , weaponIndex  = weaponIndex });
    }
}

public class WeaponReloadedEventArgs : EventArgs
{
    public Weapon weapon;
    public int weaponIndex; // 플레이어의 몇번째 무기인지
}

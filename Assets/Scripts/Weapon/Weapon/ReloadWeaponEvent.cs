using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReloadWeaponEvent : MonoBehaviour
{
    public event Action<ReloadWeaponEvent, ReloadWeaponEventArgs> OnReloadWeapon;

    public void CallReloadWeaponEvent(Weapon weapon, int weaponIndex)
    {
        OnReloadWeapon?.Invoke(this, new ReloadWeaponEventArgs() { weapon = weapon, weaponIndex = weaponIndex });
    }
}

public class ReloadWeaponEventArgs : EventArgs
{
    public Weapon weapon;
    public int weaponIndex; // 플레이어의 몇번째 무기인지
}

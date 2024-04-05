using System;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeaponEvent : MonoBehaviour
{
    public event Action<ActiveWeaponEvent, ActiveWeaponEventArgs> OnActiveWeapon;

    public void CallActiveWeaponEvent(Weapon weapon, int idx)
    {
        OnActiveWeapon?.Invoke(this, new ActiveWeaponEventArgs() { weapon = weapon, weaponIndex = idx });
    }
}

public class ActiveWeaponEventArgs : EventArgs
{
    public Weapon weapon;
    public int weaponIndex; // 플레이어의 몇번째 무기인지
}
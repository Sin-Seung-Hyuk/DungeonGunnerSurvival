using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFireable 
{
    void InitializeAmmo(float aimAngle, Weapon weapon, bool isAmmoPattern = false);

    GameObject GetGameObject();
}

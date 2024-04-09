using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFireable 
{
    void InitializeAmmo(float aimAngle, Vector3 aimDirectionVector,
        Weapon weapon);

    GameObject GetGameObject();
}

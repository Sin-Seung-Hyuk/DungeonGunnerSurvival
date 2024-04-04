using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFireable 
{
    void InitializeAmmo(float aimAngle, Vector3 aimDirectionVector,
        float ammoRange, float ammoSpeed, int ammoDamage);

    GameObject GetGameObject();
}

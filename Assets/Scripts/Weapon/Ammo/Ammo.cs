using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ammo : MonoBehaviour, IFireable
{
    private float ammoRange;
    private float ammoSpeed;
    private Vector3 fireDirectionVector;
    private float fireDirectionAngle;
    private int ammoDamage;
    private bool isColliding = false;

    
    void Update()
    {
        Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime;

        transform.position += distanceVector;

        ammoRange -= distanceVector.magnitude;

        if (ammoRange < 0f)
        {
            gameObject.SetActive(false);
        }
    }

    public void InitializeAmmo(float aimAngle, Vector3 aimDirectionVector, float ammoRange, float ammoSpeed, int ammoDamage)
    {
        isColliding = false;

        SetFireDirection(aimAngle, aimDirectionVector);

        this.ammoRange = ammoRange;
        this.ammoSpeed = ammoSpeed;
        this.ammoDamage = ammoDamage;

        gameObject.SetActive(true);
    }

    private void SetFireDirection(float aimAngle, Vector3 aimDirectionVector)
    {
        transform.eulerAngles = new Vector3(0f, 0f, aimAngle);
        fireDirectionVector = Utilities.GetDirectionVectorFromAngle(aimAngle);
    }

    GameObject IFireable.GetGameObject()
    {
        return gameObject;
    }

}

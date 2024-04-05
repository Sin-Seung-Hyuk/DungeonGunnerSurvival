using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ammo : MonoBehaviour, IFireable // 사격 인터페이스
{
    private float ammoRange;
    private float ammoSpeed;
    private int ammoDamage;
    private Vector3 fireDirectionVector;
    private float fireDirectionAngle;
    private bool isColliding = false;

    
    void Update()
    {
        Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime;

        transform.position += distanceVector;

        ammoRange -= distanceVector.magnitude;

        if (ammoRange < 0f)
        {
            gameObject.SetActive(false); // 유효거리 벗어남
        }
    }

    public void InitializeAmmo(float aimAngle, Vector3 aimDirectionVector, float ammoRange, float ammoSpeed, int ammoDamage)
    {
        isColliding = false;

        SetFireDirection(aimAngle, aimDirectionVector); // 탄 진행방향

        this.ammoRange = ammoRange;
        this.ammoSpeed = ammoSpeed;
        this.ammoDamage = ammoDamage;

        gameObject.SetActive(true); // 탄 초기화 후 활성화
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ammo : MonoBehaviour, IFireable // 사격 인터페이스
{
    // 오브젝트가 움직일떄 그 뒤를 따라 움직이는 효과 (총알 날라가는 효과 등)
    [SerializeField] private TrailRenderer trailRenderer;
    private float ammoRange;
    private float ammoSpeed;
    private int ammoDamage;
    private Vector3 fireDirectionVector;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isColliding) return;

        Health health = collision.GetComponent<Health>();

        if (health != null)
        {
            isColliding = true; // 충돌 중
            health.TakeDamage(ammoDamage);
        }

        gameObject.SetActive(false);
    }

    public void InitializeAmmo(float aimAngle, Vector3 aimDirectionVector, Weapon weapon)
    {
        isColliding = false;

        SetFireDirection(aimAngle, aimDirectionVector); // 탄 진행방향

        this.ammoRange = weapon.weaponDetail.weaponRange;
        this.ammoSpeed = weapon.weaponDetail.weaponAmmoSpeed;
        this.ammoDamage = weapon.weaponBaseDamage; // 데미지는 변하므로 SO에서 받아오면 안됨

        gameObject.SetActive(true); // 탄 초기화 후 활성화

        // trail 
        if (weapon.weaponDetail.isTrail)
        {
            trailRenderer.gameObject.SetActive(true);
            trailRenderer.emitting = true;
            trailRenderer.material = weapon.weaponDetail.ammoTrailMaterial;
            trailRenderer.startWidth = weapon.weaponDetail.ammoTrailStartWidth;
            trailRenderer.endWidth = weapon.weaponDetail.ammoTrailEndWidth;
            trailRenderer.time = weapon.weaponDetail.ammoTrailTime;
        }
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

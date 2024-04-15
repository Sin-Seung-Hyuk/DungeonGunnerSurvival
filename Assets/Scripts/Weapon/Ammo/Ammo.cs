using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ammo : MonoBehaviour, IFireable // 사격 인터페이스
{
    // 오브젝트가 움직일떄 그 뒤를 따라 움직이는 효과 (총알 날라가는 효과 등)
    [SerializeField] private TrailRenderer trailRenderer;
    private Weapon weapon;
    private float ammoRange;
    private float ammoSpeed;
    private int ammoDamage;
    private Vector3 fireDirectionVector;
    private bool isColliding = false; // 충돌 여부
    private bool isCritic = false; // 치명타 여부

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

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (isColliding) return;

        Health health = collision.gameObject.GetComponent<Health>();

        if (health != null)
        {
            isColliding = true; // 충돌 중
            this.ammoDamage = GetAmmoDamage(); // 데미지 계산
            health.TakeDamage(ammoDamage);
            AmmoHitText(ammoDamage, isCritic);
        }

        AmmoHitEffect();

        gameObject.SetActive(false);
    }

    private void AmmoHitText(int damageAmount, bool isCritic)
    {
        DamageTextUI hitText = (DamageTextUI)ObjectPoolManager.Instance.Release(GameResources.Instance.ammoHitText, transform.position, Quaternion.identity);

        hitText.InitializeDamageText(damageAmount, isCritic, transform.position.y);
    }

    private void AmmoHitEffect()
    {
        AmmoHitEffect hitEffect = (AmmoHitEffect)ObjectPoolManager.Instance.Release(GameResources.Instance.ammoHitEffect, transform.position,Quaternion.identity);

        hitEffect.gameObject.SetActive(true);
    }

    public void InitializeAmmo(float aimAngle, Vector3 aimDirectionVector, Weapon weapon)
    {
        isColliding = false;

        SetFireDirection(aimAngle, aimDirectionVector); // 탄 진행방향

        this.weapon = weapon;
        this.ammoRange = weapon.weaponDetail.weaponRange;
        this.ammoSpeed = weapon.weaponDetail.weaponAmmoSpeed;

        gameObject.SetActive(true); // 탄 초기화 후 활성화

        // Trail 렌더러 여부
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

    private int GetAmmoDamage()
    {
        // 치명타
        if (Utilities.isSuccess(weapon.weaponCriticChance))
        {
            isCritic = true;                                 // 치명타 피해량만큼 추가피해
            ammoDamage = Utilities.IncreaseByPercent(weapon.weaponBaseDamage, weapon.weaponCriticDamage);
        }
        else
        {
            isCritic = false;
            ammoDamage = weapon.weaponBaseDamage;
        }

        return ammoDamage;
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

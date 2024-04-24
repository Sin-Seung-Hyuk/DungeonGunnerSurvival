using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 추상클래스
public abstract class Ammo : MonoBehaviour, IFireable // 사격 인터페이스
{
    // 오브젝트가 움직일떄 그 뒤를 따라 움직이는 효과 (총알 날라가는 효과 등)
    [SerializeField] private TrailRenderer trailRenderer;
    private Weapon weapon;
    private float ammoRange;
    private float ammoSpeed;
    protected int ammoDamage; // protected로 상속 가능
    private Vector3 fireDirectionVector;
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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌체에 데미지를 입히고 피격텍스트 표시 (모든 탄이 가져야하는 공통적인 기능)
        IHealthObject health = collision.GetComponent<IHealthObject>();

        if (health != null)
        {
            int damageAmount = health.TakeDamage(ammoDamage);
            // 플레이어가 회피시 피격텍스트 X, 방어력만큼 깎인 수치 반영
            if (damageAmount > 0) AmmoHitText(damageAmount, isCritic);
        }
        else gameObject.SetActive(false); 

        AmmoHitEffect(); // 부딪힌 곳에 피격이펙트 남기기 (모든 탄이 가져야하는 공통적인 기능)
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
        SetFireDirection(aimAngle, aimDirectionVector); // 탄 진행방향

        this.weapon = weapon;
        this.ammoRange = weapon.weaponDetail.weaponRange;
        this.ammoSpeed = weapon.weaponDetail.weaponAmmoSpeed;
        this.ammoDamage = GetAmmoDamage();

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

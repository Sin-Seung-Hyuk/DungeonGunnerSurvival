using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ammo : MonoBehaviour, IFireable // ��� �������̽�
{
    // ������Ʈ�� �����ϋ� �� �ڸ� ���� �����̴� ȿ�� (�Ѿ� ���󰡴� ȿ�� ��)
    [SerializeField] private TrailRenderer trailRenderer;
    private Weapon weapon;
    private float ammoRange;
    private float ammoSpeed;
    private int ammoDamage;
    private Vector3 fireDirectionVector;
    private bool isColliding = false; // �浹 ����
    private bool isCritic = false; // ġ��Ÿ ����

    void Update()
    {
        Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime;

        transform.position += distanceVector;

        ammoRange -= distanceVector.magnitude;

        if (ammoRange < 0f)
        {
            gameObject.SetActive(false); // ��ȿ�Ÿ� ���
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (isColliding) return;

        Health health = collision.gameObject.GetComponent<Health>();

        if (health != null)
        {
            isColliding = true; // �浹 ��
            this.ammoDamage = GetAmmoDamage(); // ������ ���
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

        SetFireDirection(aimAngle, aimDirectionVector); // ź �������

        this.weapon = weapon;
        this.ammoRange = weapon.weaponDetail.weaponRange;
        this.ammoSpeed = weapon.weaponDetail.weaponAmmoSpeed;

        gameObject.SetActive(true); // ź �ʱ�ȭ �� Ȱ��ȭ

        // Trail ������ ����
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
        // ġ��Ÿ
        if (Utilities.isSuccess(weapon.weaponCriticChance))
        {
            isCritic = true;                                 // ġ��Ÿ ���ط���ŭ �߰�����
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

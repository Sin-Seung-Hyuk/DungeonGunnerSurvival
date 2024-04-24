using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �߻�Ŭ����
public abstract class Ammo : MonoBehaviour, IFireable // ��� �������̽�
{
    // ������Ʈ�� �����ϋ� �� �ڸ� ���� �����̴� ȿ�� (�Ѿ� ���󰡴� ȿ�� ��)
    [SerializeField] private TrailRenderer trailRenderer;
    private Weapon weapon;
    private float ammoRange;
    private float ammoSpeed;
    protected int ammoDamage; // protected�� ��� ����
    private Vector3 fireDirectionVector;
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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // �浹ü�� �������� ������ �ǰ��ؽ�Ʈ ǥ�� (��� ź�� �������ϴ� �������� ���)
        IHealthObject health = collision.GetComponent<IHealthObject>();

        if (health != null)
        {
            int damageAmount = health.TakeDamage(ammoDamage);
            // �÷��̾ ȸ�ǽ� �ǰ��ؽ�Ʈ X, ���¸�ŭ ���� ��ġ �ݿ�
            if (damageAmount > 0) AmmoHitText(damageAmount, isCritic);
        }
        else gameObject.SetActive(false); 

        AmmoHitEffect(); // �ε��� ���� �ǰ�����Ʈ ����� (��� ź�� �������ϴ� �������� ���)
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
        SetFireDirection(aimAngle, aimDirectionVector); // ź �������

        this.weapon = weapon;
        this.ammoRange = weapon.weaponDetail.weaponRange;
        this.ammoSpeed = weapon.weaponDetail.weaponAmmoSpeed;
        this.ammoDamage = GetAmmoDamage();

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

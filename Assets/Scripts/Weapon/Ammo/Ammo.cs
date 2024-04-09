using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ammo : MonoBehaviour, IFireable // ��� �������̽�
{
    // ������Ʈ�� �����ϋ� �� �ڸ� ���� �����̴� ȿ�� (�Ѿ� ���󰡴� ȿ�� ��)
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
            gameObject.SetActive(false); // ��ȿ�Ÿ� ���
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isColliding) return;

        Health health = collision.GetComponent<Health>();

        if (health != null)
        {
            isColliding = true; // �浹 ��
            health.TakeDamage(ammoDamage);
        }

        gameObject.SetActive(false);
    }

    public void InitializeAmmo(float aimAngle, Vector3 aimDirectionVector, Weapon weapon)
    {
        isColliding = false;

        SetFireDirection(aimAngle, aimDirectionVector); // ź �������

        this.ammoRange = weapon.weaponDetail.weaponRange;
        this.ammoSpeed = weapon.weaponDetail.weaponAmmoSpeed;
        this.ammoDamage = weapon.weaponBaseDamage; // �������� ���ϹǷ� SO���� �޾ƿ��� �ȵ�

        gameObject.SetActive(true); // ź �ʱ�ȭ �� Ȱ��ȭ

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

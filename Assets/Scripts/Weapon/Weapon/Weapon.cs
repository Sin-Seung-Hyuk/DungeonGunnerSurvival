using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public string weaponName { get; private set; }
    public Sprite weaponSprite { get; private set; }
    public WeaponDetailsSO weaponDetail { get; private set; }

    // ������ ���� (�������Ͽ� ���� ��� ����)
    public int weaponLevel; // ���� ����
    public int weaponBaseDamage; // �⺻������
    public int weaponAmmoCapacity; // źâ
    public float weaponFireRate; // ����ӵ�
    public float weaponReloadTime; // ������ �ӵ�
    public List<Ammo> weaponAmmoList; // ���� ź
    public int weaponRange { get; private set; } // ��Ÿ� (�����Ұ���)
    public int weaponAmmoSpeed { get; private set; } // ź �ӵ� (�����Ұ���)


    // �ǽð����� �ΰ��ӿ��� ����ϸ鼭 �ٲ�� ������
    public int weaponAmmoRemaining; // ���� ź��
    public bool isWeaponReloading;  // ������ ����
    public float weaponReloadTimer; // ������ �����ð�
    public float weaponFireRateTimer; // ��� �����ð�


    public void InitializeWeapon(WeaponDetailsSO weaponDetails) // ���� �ʱ�ȭ
    {
        weaponDetail = weaponDetails; // �Ҹ�,����Ʈ � �����ϱ� ����
        weaponName = weaponDetails.weaponName;
        weaponSprite = weaponDetails.weaponSprite;

        weaponLevel = 1;
        weaponBaseDamage = weaponDetails.weaponBaseDamage;
        weaponAmmoCapacity = weaponDetails.weaponAmmoCapacity;
        weaponFireRate = weaponDetails.weaponFireRate;
        weaponReloadTime = weaponDetails.weaponReloadTime;
        weaponRange = weaponDetails.weaponRange;
        weaponAmmoSpeed = weaponDetails.weaponAmmoSpeed;
        weaponAmmoList = weaponDetails.weaponAmmo;

        weaponAmmoRemaining = weaponDetails.weaponAmmoCapacity;
        weaponReloadTimer = weaponDetails.weaponReloadTime;
        weaponFireRateTimer = weaponDetails.weaponFireRate;
    }

    private void Update()
    {
        // ����ӵ� 
        if (weaponFireRateTimer > 0f)
            weaponFireRateTimer -= Time.deltaTime;
        else weaponFireRateTimer = weaponFireRate;
    }

    public Ammo GetCurrentAmmo(int level)
    {
        int idx = level / 10; // ���� 10���� ������ ź ���׷��̵�
        return weaponAmmoList[idx];
    }
}

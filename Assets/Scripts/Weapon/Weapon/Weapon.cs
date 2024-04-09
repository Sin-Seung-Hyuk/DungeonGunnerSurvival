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
    public List<GameObject> weaponAmmoList; // ���� ź


    // �ǽð����� �ΰ��ӿ��� ����ϸ鼭 �ٲ�� ������
    public int weaponAmmoRemaining; // ���� ź��
    public bool isWeaponReloading;  // ������ ����
    public float weaponReloadTimer = 0f; // ������ �����ð�
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
        weaponAmmoList = weaponDetails.weaponAmmo;

        weaponAmmoRemaining = weaponDetails.weaponAmmoCapacity;
        weaponFireRateTimer = weaponDetails.weaponFireRate;
    }

    private void Update()
    {
        // ����ӵ� 
        if (weaponFireRateTimer > 0f)
            weaponFireRateTimer -= Time.deltaTime;
        else weaponFireRateTimer = weaponFireRate;


        // �׽�Ʈ 
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            //weaponFireRateTimer -= 0.5f;
        }
    }

    public GameObject GetCurrentAmmo(int level)
    {
        int idx = level / 10; // ���� 10���� ������ ź ���׷��̵�
        return weaponAmmoList[idx];
    }
}
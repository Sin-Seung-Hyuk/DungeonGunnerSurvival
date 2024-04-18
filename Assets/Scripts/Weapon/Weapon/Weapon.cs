using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public string weaponName { get; private set; }
    public Sprite weaponSprite { get; private set; }
    public WeaponDetailsSO weaponDetail { get; private set; }
    public int weaponAmmoCapacity { get; private set; } // źâ �뷮
    public List<GameObject> weaponAmmoList { get; private set; } // ���� ź
    

    // ������ ���� (�������Ͽ� ���� ��� ����)
    public int weaponLevel; // ���� ����
    public int weaponBaseDamage; // �⺻������
    public int weaponCriticChance; // ġ��Ÿ Ȯ�� (%)
    public int weaponCriticDamage; // ġ��Ÿ ���� (%)
    public float weaponFireRate; // ����ӵ�
    public float weaponReloadTime; // ������ �ӵ�

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
        weaponCriticChance = weaponDetails.weaponCriticChance;
        weaponCriticDamage = weaponDetails.weaponCriticDamage;
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

    }

    public GameObject GetCurrentAmmo(int level)
    {
        int idx = level / 10; // ���� 10���� ������ ź ���׷��̵�
        return weaponAmmoList[idx];
    }
                                                                          // % ��� ����
    public void ChangeWeaponStat(PlayerStatType statType, float value, bool isPercent)
    {
        switch (statType)
        {
            case PlayerStatType.BaseDamage:
                if (isPercent)
                    weaponBaseDamage = Utilities.IncreaseByPercent(weaponBaseDamage, value);
                else weaponBaseDamage += (int)value;
                break;

            case PlayerStatType.CriticChance:
                weaponCriticChance += (int)value;
                break;

            case PlayerStatType.CriticDamage:
                weaponCriticDamage += (int)value;
                break;

            case PlayerStatType.FireRate:
                weaponFireRate = Utilities.DecreaseByPercent(weaponFireRate, value);
                break;

            case PlayerStatType.ReloadSpeed:
                weaponReloadTime = Utilities.DecreaseByPercent(weaponReloadTime, value);
                break;

            default:
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName { get; private set; }
    public Sprite weaponSprite { get; private set; }
    public WeaponDetailsSO weaponDetail { get; private set; }

    public int weaponBaseDamage; // �⺻������
    public int weaponAmmoCapacity; // źâ
    public float weaponFireRate; // ����ӵ�
    public float weaponReloadTime; // ������ �ӵ�


    public void InitializeWeapon(WeaponDetailsSO weaponDetails)
    {
        weaponDetail = weaponDetails; // �Ҹ�,����Ʈ � �����ϱ� ����
        weaponName = weaponDetails.weaponName;
        weaponSprite = weaponDetails.weaponSprite;

        weaponBaseDamage = weaponDetails.weaponBaseDamage;
        weaponAmmoCapacity = weaponDetails.weaponAmmoCapacity;
        weaponFireRate = weaponDetails.weaponFireRate;
        weaponReloadTime = weaponDetails.weaponReloadTime;
    }
}

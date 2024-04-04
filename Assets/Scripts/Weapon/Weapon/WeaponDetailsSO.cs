using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDetails_", menuName = "Scriptable Objects/Weapon/Weapon")]
public class WeaponDetailsSO : ScriptableObject
{
    [Header("weapon base details")]
    public string weaponName;
    public Sprite weaponSprite;

    [Header("weapon operating values")]
    public int weaponBaseDamage = 20;   // �⺻������
    public int weaponAmmoCapacity = 30; // źâ
    public float weaponFireRate = 0.5f; // ����ӵ�
    public float weaponReloadTime = 0f; // ������ �ӵ�
    public int weaponRange = 0; // ��Ÿ�

    [Header("weapon configuration")]
    public List<Ammo> weaponAmmo;
    //public SoundEffectSO weaponFiringSoundEffect;
    //public SoundEffectSO weaponReloadingSoundEffect;
}

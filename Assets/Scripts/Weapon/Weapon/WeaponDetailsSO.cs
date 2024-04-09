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
    public int weaponAmmoSpeed = 0; // ź �ӵ�

    [Header("weapon configuration")]
    public List<GameObject> weaponAmmo;
    public bool isTrail; // Trail ������ ����
    public Material ammoTrailMaterial;
    public float ammoTrailStartWidth;
    public float ammoTrailEndWidth;
    public float ammoTrailTime;
    public SoundEffectSO weaponFiringSoundEffect;
    public SoundEffectSO weaponReloadingSoundEffect;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDetails_", menuName = "Scriptable Objects/Weapon/Weapon")]
public class WeaponDetailsSO : ScriptableObject
{
    [Header("weapon base details")]
    public string weaponName;
    public WeaponType weaponType;
    public Sprite weaponSprite;

    [Header("weapon operating values")]
    public int weaponBaseDamage = 20;   // �⺻������
    public int weaponCriticChance = 10; // ġ��Ÿ Ȯ�� (%)
    public int weaponCriticDamage = 150; // ġ��Ÿ ���� (%)
    public int weaponAmmoCapacity = 30; // źâ
    public float weaponFireRate = 0.5f; // ����ӵ�
    public float weaponReloadTime = 0f; // ������ �ӵ�
    public int weaponRange = 0; // ��Ÿ�
    public int weaponAmmoSpeed = 0; // ź �ӵ�

    [Header("weapon configuration")]
    public List<GameObject> weaponAmmo;
    [TextArea] public string upgradeDescription;
    public bool isTrail; // Trail ������ ����
    public Material ammoTrailMaterial;
    public float ammoTrailStartWidth;
    public float ammoTrailEndWidth;
    public float ammoTrailTime;
    public SoundEffectSO weaponFiringSoundEffect;
}

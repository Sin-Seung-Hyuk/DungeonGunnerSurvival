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
    public int weaponBaseDamage = 20;   // 기본데미지
    public int weaponAmmoCapacity = 30; // 탄창
    public float weaponFireRate = 0.5f; // 연사속도
    public float weaponReloadTime = 0f; // 재장전 속도
    public int weaponRange = 0; // 사거리
    public int weaponAmmoSpeed = 0; // 탄 속도

    [Header("weapon configuration")]
    public List<GameObject> weaponAmmo;
    public bool isTrail; // Trail 렌더러 여부
    public Material ammoTrailMaterial;
    public float ammoTrailStartWidth;
    public float ammoTrailEndWidth;
    public float ammoTrailTime;
    public SoundEffectSO weaponFiringSoundEffect;
    public SoundEffectSO weaponReloadingSoundEffect;
}

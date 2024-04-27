using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyDetails_", menuName = "Scriptable Objects/Enemy/Enemy Details")]
public class EnemyDetailsSO : ScriptableObject
{
    [Header("Base Enemy Details")]
    public string enemyName;
    public GameObject enemyPrefab;
    public Sprite sprite;
    public Color spriteColor; // 스프라이트 색상
    public RuntimeAnimatorController runtimeAnimatorController;

    public int speed; // 이동속도
    public float chaseDistance = 50f; // 플레이어와의 최대 간격
    public int contactDamageAmount = 10; // 플레이어 접촉 데미지

    [Header("Material")]
    public Material enemyStandardMaterial;
    public float enemyMaterializeTime;
    public Shader enemyMaterializeShader;
    public Color enemyMaterializeColor;

    [Header("weapon setting")]
    public WeaponDetailsSO enemyWeapon;
    public float firingInterval = 0.1f;
    public float firingDuration = 1f;
    public bool firingLineOfSightRequired; // 플레이어가 시야에 있어야 쏘는지,없어도 쏘는지

    [Header("enemy health")]
    public EnemyHealthDetails[] enemyHealthDetailsArray; // 던전레벨별 체력
}

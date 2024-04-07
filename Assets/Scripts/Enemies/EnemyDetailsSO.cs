using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyDetails_", menuName = "Scriptable Objects/Enemy/Enemy Details")]
public class EnemyDetailsSO : ScriptableObject
{
    [Header("Base Enemy Details")]
    public string enemyName;
    public GameObject enemyPrefab;

    public int speed; // �̵��ӵ�
    public float chaseDistance = 50f; // �÷��̾���� �ִ� ����

    [Header("Material")]
    public Material enemyStandardMaterial;

    [Header("Materialize setting")]
    public float enemyMaterializeTime;
    public Shader enemyMaterializeShader;
    public Color enemyMaterializeColor;

    [Header("weapon setting")]
    public WeaponDetailsSO enemyWeapon;
    public float firingInterval = 0.1f;
    public float firingDuration = 1f;
    public bool firingLineOfSightRequired; // �÷��̾ �þ߿� �־�� �����,��� �����

    [Header("enemy health")]
    public EnemyHealthDetails[] enemyHealthDetailsArray; // ���������� ü��
    public bool isHealthBarDisplayed = false;
}

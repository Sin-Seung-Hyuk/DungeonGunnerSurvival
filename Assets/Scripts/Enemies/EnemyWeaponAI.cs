
using System;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyWeaponAI : MonoBehaviour
{
    // LayerMask : 32��Ʈ int��. �ν����Ϳ��� ����ϴ� ���̾ ��Ʈ����ũ�� ���� ���
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private Transform weaponShootPos;
    private Enemy enemy;
    private EnemyDetailsSO enemyDetails;
    private float firingIntervalTimer;
    private float firingDurationTimer;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        enemyDetails = enemy.enemyDetails;
        firingIntervalTimer = enemyDetails.firingInterval;
        firingDurationTimer = enemyDetails.firingDuration;
    }
    private void Update()
    {
        firingIntervalTimer -= Time.deltaTime;

        if (firingIntervalTimer < 0f)
        {
            if (firingDurationTimer >= 0f)
            {
                firingDurationTimer -= Time.deltaTime;
                // ��� ���ݰ� ��� ���ӽð� ��� ������ ��� ����Լ� ����
                FireWeapon();
            }
            else
            {
                firingIntervalTimer = enemyDetails.firingInterval;
                firingDurationTimer = enemyDetails.firingDuration;
            }
        }
    }

    private void FireWeapon()
    {
        // �����κ��� �÷��̾ ���ϴ� ���⺤��
        Vector3 playerDirectionVector = GameManager.Instance.GetPlayerPosition() - transform.position;
        // ���� ����κ��� �÷��̾ ���ϴ� ���⺤��
        Vector3 weaponDirection = (GameManager.Instance.GetPlayerPosition() - weaponShootPos.position);
        // ��,���� ������ ���� ���ϱ�
        float weaponAngleDegrees = Utilities.GetAngleFromVector(weaponDirection);
        // ���� ������ ���� ���� ���� ���ϱ�
        AimDirection aimDirection = Utilities.GetAimDirectionFromAngle(weaponAngleDegrees);

        enemy.weaponAimEvent.CallWeaponAim(aimDirection, weaponAngleDegrees, weaponDirection);

        if (enemyDetails.enemyWeapon != null)
        {
            float enemyAmmoRange = enemyDetails.enemyWeapon.weaponRange; // �� ������ źȯ ��ȿ�Ÿ�

            if (playerDirectionVector.magnitude <= enemyAmmoRange)
            {
                if (enemyDetails.firingLineOfSightRequired && !IsPlayerInLineOfSight(weaponDirection, enemyAmmoRange))
                    return; // ���� �þ߿� �÷��̾ �־�߸� ����ϴ� ���

                // �� ������ SO�� ������ ����� ����̺�Ʈ ȣ��
                enemy.fireWeaponEvent.CallFireWeaponEvent(enemy.GetEnemyWeapon(),  weaponAngleDegrees, weaponDirection, -1);
            }
        }
    }

    private bool IsPlayerInLineOfSight(Vector3 weaponDirection, float enemyAmmoRange)
    {
        // 2D ����ĳ��Ʈ ����,                     ��������                        ���� ����           �Ÿ�         ������ ���̾�
        RaycastHit2D raycast = Physics2D.Raycast(weaponShootPos.position, (Vector2)weaponDirection, enemyAmmoRange, layerMask);

        if (raycast && raycast.transform.CompareTag(Settings.playerTag))
        {   // ����ĳ��Ʈ ���� ������Ʈ �±� �� (���ڿ� ���縦 �����ϱ� ���� Setting Ȱ��)
            return true;
        }
        return false;
    }
}

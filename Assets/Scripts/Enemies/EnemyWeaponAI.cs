
using System;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyWeaponAI : MonoBehaviour
{
    // LayerMask : 32비트 int형. 인스펙터에서 사용하는 레이어를 비트마스크로 쉽게 사용
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
                // 사격 간격과 사격 지속시간 모두 만족할 경우 사격함수 실행
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
        // 적으로부터 플레이어를 향하는 방향벡터
        Vector3 playerDirectionVector = GameManager.Instance.GetPlayerPosition() - transform.position;
        // 적의 무기로부터 플레이어를 향하는 방향벡터
        Vector3 weaponDirection = (GameManager.Instance.GetPlayerPosition() - weaponShootPos.position);
        // 적,적의 무기의 각도 구하기
        float weaponAngleDegrees = Utilities.GetAngleFromVector(weaponDirection);
        // 현재 각도를 통해 에임 방향 구하기
        AimDirection aimDirection = Utilities.GetAimDirectionFromAngle(weaponAngleDegrees);

        enemy.weaponAimEvent.CallWeaponAim(aimDirection, weaponAngleDegrees, weaponDirection);

        if (enemyDetails.enemyWeapon != null)
        {
            float enemyAmmoRange = enemyDetails.enemyWeapon.weaponRange; // 적 무기의 탄환 유효거리

            if (playerDirectionVector.magnitude <= enemyAmmoRange)
            {
                if (enemyDetails.firingLineOfSightRequired && !IsPlayerInLineOfSight(weaponDirection, enemyAmmoRange))
                    return; // 적이 시야에 플레이어가 있어야만 사격하는 경우

                // 적 디테일 SO에 구현된 무기로 사격이벤트 호출
                enemy.fireWeaponEvent.CallFireWeaponEvent(enemy.GetEnemyWeapon(),  weaponAngleDegrees, weaponDirection, -1);
            }
        }
    }

    private bool IsPlayerInLineOfSight(Vector3 weaponDirection, float enemyAmmoRange)
    {
        // 2D 레이캐스트 생성,                     시작지점                        레이 방향           거리         감지할 레이어
        RaycastHit2D raycast = Physics2D.Raycast(weaponShootPos.position, (Vector2)weaponDirection, enemyAmmoRange, layerMask);

        if (raycast && raycast.transform.CompareTag(Settings.playerTag))
        {   // 레이캐스트 맞은 오브젝트 태그 비교 (문자열 복사를 방지하기 위해 Setting 활용)
            return true;
        }
        return false;
    }
}

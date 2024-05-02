using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;
using System.Collections.Generic;

#region REQUIRE COMPONENTS
[RequireComponent(typeof(DealContactDamage))]
[RequireComponent(typeof(HealthEvent))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(DestroyedEvent))]
[RequireComponent(typeof(Destroyed))]
[RequireComponent(typeof(EnemyWeaponAI))]
[RequireComponent(typeof(FireWeaponEvent))]
[RequireComponent(typeof(FireWeapon))]
[RequireComponent(typeof(WeaponFiredEvent))]
[RequireComponent(typeof(ReloadWeaponEvent))]
[RequireComponent(typeof(ReloadWeapon))]
[RequireComponent(typeof(EnemyMovementAI))]
[RequireComponent(typeof(MovementToPositionEvent))]
[RequireComponent(typeof(MovementToPosition))]
[RequireComponent(typeof(IdleEvent))]
[RequireComponent(typeof(Idle))]
[RequireComponent(typeof(MaterializeEffect))]
[RequireComponent(typeof(SortingGroup))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(PolygonCollider2D))]
#endregion REQUIRE COMPONENTS

[DisallowMultipleComponent]
public class Enemy : MonoBehaviour, IHealthObject, IDebuff
{
    [HideInInspector] public EnemyDetailsSO enemyDetails;
    [HideInInspector] public MovementToPositionEvent movementToPositionEvent;
    [HideInInspector] public IdleEvent idleEvent;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;
    [HideInInspector] public WeaponAimEvent weaponAimEvent;
    [HideInInspector] public FireWeaponEvent fireWeaponEvent;

    private CircleCollider2D circleCollider2D;
    private PolygonCollider2D polygonCollider2D;
    private DealContactDamage dealContactDamage;
    private EnemyMovementAI enemyMovementAI;
    private EnemyWeaponAI enemyWeaponAI;
    private MaterializeEffect materializeEffect;
    private FireWeapon fireWeapon;
    private Weapon weapon;
    private Health health;
    private HealthEvent healthEvent;



    private void Awake()
    {
        enemyMovementAI = GetComponent<EnemyMovementAI>();
        enemyWeaponAI = GetComponent<EnemyWeaponAI>();
        movementToPositionEvent = GetComponent<MovementToPositionEvent>();
        idleEvent = GetComponent<IdleEvent>();
        circleCollider2D = GetComponentInChildren<CircleCollider2D>(); // 총알을 무시하는 충돌체 (몸끼리 부딪히는 충돌)
        polygonCollider2D = GetComponent<PolygonCollider2D>();         // 탄 피격판정 충돌체
        dealContactDamage = GetComponent<DealContactDamage>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        materializeEffect = GetComponent<MaterializeEffect>();
        weaponAimEvent = GetComponent<WeaponAimEvent>();
        fireWeaponEvent = GetComponent<FireWeaponEvent>();
        fireWeapon = GetComponent<FireWeapon>();
        health = GetComponent<Health>();
        healthEvent = GetComponent<HealthEvent>();
    }

    private void OnEnable()
    {
        healthEvent.OnHealthChanged += HealthEvent_OnHealthLost;

        StaticEventHandler.OnRoomTimeout += StaticEventHandler_OnRoomTimeout;
    }

    private void OnDisable()
    {
        healthEvent.OnHealthChanged -= HealthEvent_OnHealthLost;

        StaticEventHandler.OnRoomTimeout -= StaticEventHandler_OnRoomTimeout;
    }

    private void StaticEventHandler_OnRoomTimeout(RoomTimeoutArgs obj)
    {
        StartCoroutine(EnemyDestroyedRoutine()); // 타임아웃으로 적 파괴
    }

    private void HealthEvent_OnHealthLost(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
    {
        if (healthEventArgs.damageAmount == 0) return;

        if (healthEventArgs.healthAmount <= 0) StartCoroutine(EnemyDestroyedRoutine()); // 적 파괴코루틴 실행
    }

    public void EnemyInitialization(EnemyDetailsSO enemyDetails, DungeonLevelSO dungeonLevel)
    {
        // ============= **** 오브젝트 풀에 반환하고나서 다시 스폰될때 완벽하게 모든 정보를 다 명시해주면서 초기화해야함 =================================
        this.enemyDetails = enemyDetails;
        
        EnemyEnable(true); // 비활성화되어있던 상태를 다시 활성화

        SetDealContactDamage();
        SetEnemyAnimateSpeed();
        SetEnemyMovementUpdateFrame();
        SetEnemyStartingHealth(dungeonLevel);
        SetEnemyStartingWeapon();
        SetEnemyWeaponAI(); // 무기 AI 재설정 (처음에 근거리몬스터가 나왔어도 이후 원거리가 나올때 원거리 무기AI를 가져야함)

        spriteRenderer.sprite = enemyDetails.sprite;
        spriteRenderer.color = enemyDetails.spriteColor;

        List<Vector2> spritePhysicsShapePointsList = new List<Vector2>();

        spriteRenderer.sprite.GetPhysicsShape(0, spritePhysicsShapePointsList); // 스프라이트 테두리 따오기
        polygonCollider2D.points = spritePhysicsShapePointsList.ToArray(); // 피격판정 충돌체 그리기

        animator.runtimeAnimatorController = enemyDetails.runtimeAnimatorController;

        gameObject.SetActive(true);
    }

    private IEnumerator EnemyDestroyedRoutine()
    {
        // 적 비활성화, 속도 0
        dealContactDamage.InitializedContactDamage(0);
        EnemyEnable(false);
        if (weapon != null) Destroy(weapon); // 무기를 가진 적이라면 파괴되면서 무기를 파괴해야함 (다음에 생성될 적을 위해)
        enemyMovementAI.moveSpeed = 0f;

        // MaterializeRoutine 코루틴이 끝날때까지 기다림 (중첩 코루틴)
        yield return StartCoroutine(materializeEffect.MaterializeRoutine( // 머테리얼 DissolveAmount 속성 코루틴
            enemyDetails.enemyMaterializeShader, enemyDetails.enemyMaterializeColor,
            enemyDetails.enemyMaterializeTime, spriteRenderer, enemyDetails.enemyStandardMaterial, false));

        // 머테리얼 코루틴이 끝난 후 풀에 반환하기 위해 파괴이벤트 호출
        DestroyedEvent destroyedEvent = GetComponent<DestroyedEvent>();
        destroyedEvent.CallDestroyedEvent(true, this.transform.position); // 풀에 반환해야하므로 true

        StatisticsManager.Instance.TotalEnemiesKill++; // 처치한 적 수 증가
    }

    private void EnemyEnable(bool isEnable) // 적 활성화/비활성화 (콜라이더,움직임,사격)
    {
        circleCollider2D.enabled = isEnable;
        polygonCollider2D.enabled = isEnable;

        enemyMovementAI.enabled = isEnable;
        fireWeapon.enabled = isEnable;
    }

    public Weapon GetEnemyWeapon()
    {
        return weapon;
    }

    // ========================== Set Enemy Initialize ================================
    #region Set Enemy Initialize
    private void SetEnemyStartingWeapon() // Start 지점에서 수행됨
    {
        if (enemyDetails.enemyWeapon != null)
        {
            weapon = gameObject.AddComponent<Weapon>();
            weapon.InitializeWeapon(enemyDetails.enemyWeapon);
            fireWeapon.enabled = true;
        }
        else
        {
            weapon = null;
            fireWeapon.enabled = false;
        }
    }
    private void SetEnemyWeaponAI()
    {
        // 적이 생성되면서 WeaponAI에 무기 데이터를 새로 초기화
        enemyWeaponAI.enemyDetails = enemyDetails;
        enemyWeaponAI.firingIntervalTimer = enemyDetails.firingInterval;
        enemyWeaponAI.firingDurationTimer = enemyDetails.firingDuration;
    }
    private void SetEnemyStartingHealth(DungeonLevelSO dungeonLevel)
    {
        foreach (EnemyHealthDetails enemyHealth in enemyDetails.enemyHealthDetailsArray)
        {   // 현재 레벨에 맞는 체력정보 가져오기
            if (enemyHealth.dungeonLevel == dungeonLevel)
            {   // health 컴포넌트 내에 구현된 체력설정 함수로 체력세팅
                health.SetStartingHealth(enemyHealth.enemyHealthAmount);
                return;
            }
        }
        health.SetStartingHealth(Settings.defaultEnemyHealth);
    }
    private void SetEnemyMovementUpdateFrame()
    {
        enemyMovementAI.moveSpeed = enemyDetails.speed;
        enemyMovementAI.chaseDistance = enemyDetails.chaseDistance;

        enemyMovementAI.SetUpdateFrameNumber(60 % Settings.targetFrameRateToSpreadPathFindingOver);
    }
    private void SetEnemyAnimateSpeed()
    {   // AI의 애니메이션 재생속도 설정
        animator.speed = enemyDetails.speed / 3f;
    }
    private void SetDealContactDamage()
    {
        dealContactDamage.InitializedContactDamage(enemyDetails.contactDamageAmount);
    }
    #endregion


    // =================== Interface 구현 =============================================
    #region Interface
    public int TakeDamage(int ammoDamage, out int damageAmount)
    {
        damageAmount = ammoDamage;
        health.SetCurrentHealth(damageAmount);
        health.CallHealthEvent(damageAmount);

        return damageAmount;
    }

    public void Debuff_Slow()
    {
        spriteRenderer.color = Settings.blue; // 스프라이트 색상 파랗게 변화

        enemyMovementAI.moveSpeed = Utilities.DecreaseByPercent(enemyDetails.speed,50); // 속도 50% 감소
    }
    public void Debuff_Burn(int ammoDamage)
    {
        spriteRenderer.color = Settings.red;
        StartCoroutine(Burn(ammoDamage));
    }
    private IEnumerator Burn(int ammoDamage)
    {
        int burnDamage = (int)(ammoDamage * Settings.burnDamage);

        int count = 0;
        while (count < 3) // 한발 맞으면 3번 불타면서 틱데미지
        {
            TakeDamage(burnDamage,out int damageAmount);

            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1f, 0f);
            DamageTextUI hitText = (DamageTextUI)ObjectPoolManager.Instance.Release(GameResources.Instance.ammoHitText, pos, Quaternion.identity);

            hitText.InitializeDamageText(burnDamage, false, pos.y);

            yield return new WaitForSeconds(Settings.burnDistance); // 1초마다 
            count++;
        }

        spriteRenderer.color = enemyDetails.spriteColor; // 화상이 끝나면 원래 색상으로
    }
    #endregion
}
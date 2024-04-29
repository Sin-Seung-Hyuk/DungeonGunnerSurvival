using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;
using System.Collections.Generic;

#region REQUIRE COMPONENTS
//[RequireComponent(typeof(DealContactDamage))]
//[RequireComponent(typeof(HealthEvent))]
//[RequireComponent(typeof(Health))]
//[RequireComponent(typeof(DestroyedEvent))]
//[RequireComponent(typeof(Destroyed))]
//[RequireComponent(typeof(EnemyWeaponAI))]
//[RequireComponent(typeof(AimWeaponEvent))]
//[RequireComponent(typeof(AimWeapon))]
//[RequireComponent(typeof(FireWeaponEvent))]
//[RequireComponent(typeof(FireWeapon))]
//[RequireComponent(typeof(SetActiveWeaponEvent))]
//[RequireComponent(typeof(ActiveWeapon))]
//[RequireComponent(typeof(WeaponFiredEvent))]
//[RequireComponent(typeof(ReloadWeaponEvent))]
//[RequireComponent(typeof(ReloadWeapon))]
//[RequireComponent(typeof(WeaponRelodedEvent))]
//[RequireComponent(typeof(EnemyMovementAI))]
//[RequireComponent(typeof(MovementToPositionEvent))]
//[RequireComponent(typeof(MovementToPosition))]
//[RequireComponent(typeof(IdleEvent))]
//[RequireComponent(typeof(Idle))]
//[RequireComponent(typeof(EnemyAnimate))]
//[RequireComponent(typeof(MaterializeEffect))]
//[RequireComponent(typeof(SortingGroup))]
//[RequireComponent(typeof(SpriteRenderer))]
//[RequireComponent(typeof(Animator))]
//[RequireComponent(typeof(Rigidbody2D))]
//[RequireComponent(typeof(CircleCollider2D))]
//[RequireComponent(typeof(PolygonCollider2D))]
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
    private MaterializeEffect materializeEffect;
    private FireWeapon fireWeapon;
    private Weapon weapon;
    private Health health;
    private HealthEvent healthEvent;



    private void Awake()
    {
        enemyMovementAI = GetComponent<EnemyMovementAI>();
        movementToPositionEvent = GetComponent<MovementToPositionEvent>();
        idleEvent = GetComponent<IdleEvent>();
        circleCollider2D = GetComponentInChildren<CircleCollider2D>(); // 총알을 무시하는 충돌체 (몸끼리 부딪히는 충돌)
        polygonCollider2D = GetComponent<PolygonCollider2D>();
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
        this.enemyDetails = enemyDetails;



        SetDealContactDamage();
        SetEnemyAnimateSpeed();
        SetEnemyMovementUpdateFrame();
        SetEnemyStartingHealth(dungeonLevel);
        SetEnemyStartingWeapon();

        spriteRenderer.sprite = enemyDetails.sprite;
        spriteRenderer.color = enemyDetails.spriteColor;

        List <Vector2> spritePhysicsShapePointsList = new List<Vector2>();

        spriteRenderer.sprite.GetPhysicsShape(0, spritePhysicsShapePointsList); // 스프라이트 테두리 따오기
        polygonCollider2D.points = spritePhysicsShapePointsList.ToArray(); // 피격판정 충돌체 그리기

        animator.runtimeAnimatorController = enemyDetails.runtimeAnimatorController;

        EnemyEnable(true);
        gameObject.SetActive(true);


        if (weapon != null)
            Debug.Log(enemyDetails.enemyName + " 무기 : "+weapon.weaponName + " 무기 여부: "+fireWeapon.enabled);
        else Debug.Log(enemyDetails.enemyName+"무기없음, 무기 여부: " + fireWeapon.enabled);
    }

    private IEnumerator EnemyDestroyedRoutine()
    {
        // 적 비활성화, 속도 0
        EnemyEnable(false);
        if (weapon != null)
            Destroy(weapon);
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
        health.SetStartingHealth(150);
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
        spriteRenderer.color = Settings.blue;

        enemyMovementAI.moveSpeed = 2;
    }
    public void Debuff_Burn(int ammoDamage)
    {
        spriteRenderer.color = Settings.red;
        StartCoroutine(Burn(ammoDamage));
    }
    private IEnumerator Burn(int ammoDamage)
    {
        int burnDamage = (int)(ammoDamage * 0.1f);

        int count = 0;
        while (count < 3)
        {
            TakeDamage(burnDamage,out int damageAmount);

            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1f, 0f);
            DamageTextUI hitText = (DamageTextUI)ObjectPoolManager.Instance.Release(GameResources.Instance.ammoHitText, pos, Quaternion.identity);

            hitText.InitializeDamageText(burnDamage, false, pos.y);

            yield return new WaitForSeconds(1.0f);
            count++;
        }

        spriteRenderer.color = Color.white;
    }
    #endregion
}
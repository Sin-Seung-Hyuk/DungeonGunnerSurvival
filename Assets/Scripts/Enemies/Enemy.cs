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
public class Enemy : MonoBehaviour, IHealthObject
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
        circleCollider2D = GetComponent<CircleCollider2D>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
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
        StartCoroutine(EnemyDestroyedRoutine()); // Ÿ�Ӿƿ����� �� �ı�
    }

    private void HealthEvent_OnHealthLost(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
    {
        if (healthEventArgs.damageAmount == 0) return;

        if (healthEventArgs.healthAmount <= 0) StartCoroutine(EnemyDestroyedRoutine()); // �� �ı��ڷ�ƾ ����
    }

    public void EnemyInitialization(EnemyDetailsSO enemyDetails, DungeonLevelSO dungeonLevel)
    {
        this.enemyDetails = enemyDetails;

        SetEnemyAnimateSpeed();
        SetEnemyMovementUpdateFrame();
        SetEnemyStartingHealth(dungeonLevel);
        SetEnemyStartingWeapon();

        spriteRenderer.sprite = enemyDetails.sprite;

        List<Vector2> spritePhysicsShapePointsList = new List<Vector2>(); 

        spriteRenderer.sprite.GetPhysicsShape(0, spritePhysicsShapePointsList); // ��������Ʈ �׵θ� ������
        polygonCollider2D.points = spritePhysicsShapePointsList.ToArray(); // �ǰ����� �浹ü �׸���

        animator.runtimeAnimatorController = enemyDetails.runtimeAnimatorController;

        gameObject.SetActive(true);
    }

    private IEnumerator EnemyDestroyedRoutine()
    {
        // �� ��Ȱ��ȭ, �ӵ� 0
        EnemyEnable(false);
        enemyMovementAI.moveSpeed = 0f;

        // MaterializeRoutine �ڷ�ƾ�� ���������� ��ٸ� (��ø �ڷ�ƾ)
        yield return StartCoroutine(materializeEffect.MaterializeRoutine( // ���׸��� DissolveAmount �Ӽ� �ڷ�ƾ
            enemyDetails.enemyMaterializeShader, enemyDetails.enemyMaterializeColor,
            enemyDetails.enemyMaterializeTime, spriteRenderer, enemyDetails.enemyStandardMaterial, false));

        // ���׸��� �ڷ�ƾ�� ���� �� Ǯ�� ��ȯ�ϱ� ���� �ı��̺�Ʈ ȣ��
        DestroyedEvent destroyedEvent = GetComponent<DestroyedEvent>();
        destroyedEvent.CallDestroyedEvent(true, this.transform.position); // Ǯ�� ��ȯ�ؾ��ϹǷ� true
    }

    private void EnemyEnable(bool isEnable) // �� Ȱ��ȭ/��Ȱ��ȭ (�ݶ��̴�,������,���)
    {
        circleCollider2D.enabled = isEnable;
        polygonCollider2D.enabled = isEnable;

        enemyMovementAI.enabled = isEnable;
        fireWeapon.enabled = isEnable;
    }

    private void SetEnemyStartingWeapon() // Start �������� �����
    {
        if (enemyDetails.enemyWeapon != null)
        {
            weapon = gameObject.AddComponent<Weapon>();
            weapon.InitializeWeapon(enemyDetails.enemyWeapon);
        }
        else
            fireWeapon.enabled = false;
    }

    public Weapon GetEnemyWeapon()
    {
        return weapon;
    }
    private void SetEnemyStartingHealth(DungeonLevelSO dungeonLevel)
    {
        foreach (EnemyHealthDetails enemyHealth in enemyDetails.enemyHealthDetailsArray)
        {   // ���� ������ �´� ü������ ��������
            if (enemyHealth.dungeonLevel == dungeonLevel)
            {   // health ������Ʈ ���� ������ ü�¼��� �Լ��� ü�¼���
                health.SetStartingHealth(enemyHealth.enemyHealthAmount);
                return;
            }
        }
        health.SetStartingHealth(150);
    }
    private void SetEnemyMovementUpdateFrame()
    {
        enemyMovementAI.SetUpdateFrameNumber(60 % Settings.targetFrameRateToSpreadPathFindingOver);
    }
    private void SetEnemyAnimateSpeed()
    {   // AI�� �ִϸ��̼� ����ӵ� ����
        animator.speed = enemyDetails.speed / 3f;
    }

    public int TakeDamage(int damageAmount)
    {
        health.SetCurrentHealth(damageAmount);
        health.CallHealthEvent(damageAmount);

        return damageAmount;
    }
}
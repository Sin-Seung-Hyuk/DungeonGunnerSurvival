using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

#region RequireComponents
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(SortingGroup))]
[RequireComponent(typeof(PlayerCtrl))]
[RequireComponent(typeof(IdleEvent))]
[RequireComponent(typeof(Idle))]
[RequireComponent(typeof(PlayerAnimate))]
[RequireComponent(typeof(MovementEvent))]
[RequireComponent(typeof(Movement))]
#endregion
public class Player : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public PlayerDetailsSO playerDetails;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public CircleCollider2D circleRange; // �ڼ�����
    [HideInInspector] public PlayerStat stat; // ĳ���� ����
    [HideInInspector] public Health health;

    // �÷��̾ ������ �̺�Ʈ
    [HideInInspector] public IdleEvent idleEvent;
    [HideInInspector] public MovementEvent movementEvent;
    [HideInInspector] public WeaponAimEvent weaponAimEvent;
    [HideInInspector] public FireWeaponEvent fireWeaponEvent;
    [HideInInspector] public WeaponFiredEvent weaponFiredEvent;
    [HideInInspector] public ReloadWeaponEvent reloadWeaponEvent;
    [HideInInspector] public ActiveWeaponEvent activeWeaponEvent;
    [HideInInspector] public HealthEvent healthEvent;
    [HideInInspector] public DestroyedEvent destroyedEvent;

    [HideInInspector] public List<Weapon> weaponList = new List<Weapon>(); // ���� ����Ʈ

    private PlayerInventoryHolder playerInventory; // �÷��̾� �κ��丮



    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventoryHolder>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleRange = GetComponent<CircleCollider2D>();
        health = GetComponent<Health>();

        idleEvent = GetComponent<IdleEvent>();
        movementEvent = GetComponent<MovementEvent>();
        weaponAimEvent = GetComponent<WeaponAimEvent>();
        fireWeaponEvent = GetComponent<FireWeaponEvent>();
        weaponFiredEvent = GetComponent<WeaponFiredEvent>();
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        activeWeaponEvent = GetComponent<ActiveWeaponEvent>();
        healthEvent = GetComponent<HealthEvent>();
        destroyedEvent = GetComponent<DestroyedEvent>();
    }

    private void OnEnable()
    {
        healthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;
    }
    private void OnDisable()
    {
        healthEvent.OnHealthChanged -= HealthEvent_OnHealthChanged;
    }

    private void HealthEvent_OnHealthChanged(HealthEvent healthEvent, HealthEventArgs args)
    {
        if (args.healthAmount <= 0f)
        {
            destroyedEvent.CallDestroyedEvent(true, 0);
        }
    }

    public void InitializePlayer(PlayerDetailsSO playerDetails)
    {
        this.playerDetails = playerDetails;

        spriteRenderer.sprite = playerDetails.playerSprite;
        animator.runtimeAnimatorController = playerDetails.runtimeAnimatorController;

        SetPlayerHealth(playerDetails.maxHp);

        stat.baseDamage = playerDetails.baseDamage;
        stat.criticChance = playerDetails.criticChance;
        stat.criticDamage = playerDetails.criticDamage;
        stat.reloadSpeed = playerDetails.reloadSpeed;
        stat.fireRateSpeed = playerDetails.fireRateSpeed;
        stat.moveSpeed = playerDetails.moveSpeed;
        stat.circleRange = playerDetails.circleRange;
        stat.dodgeChance = playerDetails.dodgeChance;
        stat.expGain = playerDetails.expGain;

        circleRange.radius = stat.circleRange;
    }

    private void Start()
    {
        foreach (var weapon in playerDetails.playerStartingWeapon)
        {
            AddWeaponToPlayer(weapon);
        }

    }

    public Weapon AddWeaponToPlayer(WeaponDetailsSO weaponDetails)
    {
        // �߰��� ���� �ʱ�ȭ
        Weapon playerWeapon = gameObject.AddComponent<Weapon>();
        playerWeapon.InitializeWeapon(weaponDetails);

        weaponList.Add(playerWeapon); // ���� ����Ʈ�� �߰�
        activeWeaponEvent.CallActiveWeaponEvent(playerWeapon, weaponList.Count-1); // ���� UI �߰�

        return playerWeapon;
    }

    private void SetPlayerHealth(int hp)
    {
        // playerDeatilsSO ���� ������ �ִ�ü������ ��Ÿ��ü�� ����
        health.SetStartingHealth(hp);
    }
}

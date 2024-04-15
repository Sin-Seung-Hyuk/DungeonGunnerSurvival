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
    [HideInInspector] public PolygonCollider2D test; // �ڼ�����
    [HideInInspector] public PlayerStat stat; // ĳ���� ����
    [HideInInspector] public Health health;
    [HideInInspector] public PlayerExp playerExp;

    // �÷��̾ ������ �̺�Ʈ
    [HideInInspector] public IdleEvent idleEvent;
    [HideInInspector] public MovementEvent movementEvent;
    [HideInInspector] public WeaponAimEvent weaponAimEvent;
    [HideInInspector] public FireWeapon fireWeapon;
    [HideInInspector] public FireWeaponEvent fireWeaponEvent;
    [HideInInspector] public WeaponFiredEvent weaponFiredEvent;
    [HideInInspector] public ReloadWeaponEvent reloadWeaponEvent;
    [HideInInspector] public ActiveWeaponEvent activeWeaponEvent;
    [HideInInspector] public HealthEvent healthEvent;
    [HideInInspector] public DestroyedEvent destroyedEvent;
    [HideInInspector] public PlayerStatChangedEvent playerStatChangedEvent;

    [HideInInspector] public List<Weapon> weaponList = new List<Weapon>(); // ���� ����Ʈ

    [HideInInspector] public PlayerInventoryHolder playerInventory; // �÷��̾� �κ��丮



    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventoryHolder>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleRange = GetComponent<CircleCollider2D>();
        test = GetComponent<PolygonCollider2D>();
        health = GetComponent<Health>();
        playerExp = GetComponent<PlayerExp>();

        idleEvent = GetComponent<IdleEvent>();
        movementEvent = GetComponent<MovementEvent>();
        weaponAimEvent = GetComponent<WeaponAimEvent>();
        fireWeaponEvent = GetComponent<FireWeaponEvent>();
        fireWeapon = GetComponent<FireWeapon>();
        weaponFiredEvent = GetComponent<WeaponFiredEvent>();
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        activeWeaponEvent = GetComponent<ActiveWeaponEvent>();
        healthEvent = GetComponent<HealthEvent>();
        destroyedEvent = GetComponent<DestroyedEvent>();
        playerStatChangedEvent = GetComponent<PlayerStatChangedEvent>();
    }

    public void InitializePlayer(PlayerDetailsSO playerDetails)
    {
        this.playerDetails = playerDetails;

        spriteRenderer.sprite = playerDetails.playerSprite;
        animator.runtimeAnimatorController = playerDetails.runtimeAnimatorController;

        SetPlayerHealth(playerDetails.maxHp);

        stat.SetPlayerStat(PlayerStatType.MaxHP, playerDetails.maxHp);
        stat.SetPlayerStat(PlayerStatType.BaseDamage, playerDetails.baseDamage);
        stat.SetPlayerStat(PlayerStatType.BaseArmor, playerDetails.baseArmor);
        stat.SetPlayerStat(PlayerStatType.Dodge, playerDetails.dodgeChance);
        stat.SetPlayerStat(PlayerStatType.ReloadSpeed, playerDetails.reloadSpeed);
        stat.SetPlayerStat(PlayerStatType.FireRate, playerDetails.fireRateSpeed);
        stat.SetPlayerStat(PlayerStatType.MoveSpeed, playerDetails.moveSpeed);
        stat.SetPlayerStat(PlayerStatType.CircleRadius, playerDetails.circleRange);
        stat.SetPlayerStat(PlayerStatType.ExpGain, playerDetails.expGain);

        circleRange.radius = stat.circleRange;
    }

    private void Start()
    {
        foreach (var weapon in playerDetails.playerStartingWeapon)
        {
            AddWeaponToPlayer(weapon);
        }
    }

    private void OnEnable()
    {
        healthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;
        playerStatChangedEvent.OnPlayerStatChanged += PlayerStatChangedEvent_OnPlayerStatChanged;
    }

    private void OnDisable()
    {
        healthEvent.OnHealthChanged -= HealthEvent_OnHealthChanged;
        playerStatChangedEvent.OnPlayerStatChanged -= PlayerStatChangedEvent_OnPlayerStatChanged;
    }

    private void HealthEvent_OnHealthChanged(HealthEvent healthEvent, HealthEventArgs args)
    {
        if (args.healthAmount <= 0f)
        {
            destroyedEvent.CallDestroyedEvent(true, this.transform.position);
        }
    }

    private void PlayerStatChangedEvent_OnPlayerStatChanged(PlayerStatChangedEvent arg1, PlayerStatChangedEventArgs args)
    {
        stat.ChangePlayerStat(args.statType, args.changeValue);

        circleRange.radius = stat.circleRange;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
      Debug.Log("player : " + collision.transform.position);
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

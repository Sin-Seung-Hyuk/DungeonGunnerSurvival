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
    [HideInInspector] public CircleCollider2D circleRange; // 자석범위
    [HideInInspector] public PolygonCollider2D test; // 자석범위
    [HideInInspector] public PlayerStat stat; // 캐릭터 스탯
    [HideInInspector] public Health health;
    [HideInInspector] public PlayerExp playerExp;

    // 플레이어가 가지는 이벤트
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

    [HideInInspector] public List<Weapon> weaponList = new List<Weapon>(); // 무기 리스트

    [HideInInspector] public PlayerInventoryHolder playerInventory; // 플레이어 인벤토리



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
        // 추가할 무기 초기화
        Weapon playerWeapon = gameObject.AddComponent<Weapon>();
        playerWeapon.InitializeWeapon(weaponDetails);

        weaponList.Add(playerWeapon); // 무기 리스트에 추가
        activeWeaponEvent.CallActiveWeaponEvent(playerWeapon, weaponList.Count-1); // 무기 UI 추가

        return playerWeapon;
    }

    private void SetPlayerHealth(int hp)
    {
        // playerDeatilsSO 에서 설정한 최대체력으로 스타팅체력 설정
        health.SetStartingHealth(hp);
    }
}

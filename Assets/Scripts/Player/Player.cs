using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

#region RequireComponents
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
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
public class Player : MonoBehaviour, IHealthObject
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public PlayerDetailsSO playerDetails;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public CircleCollider2D circleRange; // 자석범위
    [HideInInspector] public PlayerStat stat; // 캐릭터 스탯
    [HideInInspector] public PlayerCtrl ctrl; // 캐릭터 컨트롤러
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
    [HideInInspector] public PlayerLevelUpEvent playerLevelUpEvent;

    public List<Weapon> weaponList { get; private set; } // 무기 리스트
    public PlayerInventoryHolder playerInventory { get; private set; } // 플레이어 인벤토리



    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventoryHolder>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleRange = GetComponentInChildren<CircleCollider2D>();
        health = GetComponent<Health>();
        ctrl = GetComponent<PlayerCtrl>();
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
        playerLevelUpEvent = GetComponent<PlayerLevelUpEvent>();
    }

    public void InitializePlayer(PlayerDetailsSO playerDetails)
    {
        this.playerDetails = playerDetails;

        spriteRenderer.sprite = playerDetails.playerSprite;
        animator.runtimeAnimatorController = playerDetails.runtimeAnimatorController;
        weaponList = new List<Weapon>();

        health.SetStartingHealth(playerDetails.maxHp);

        stat.SetPlayerStat(PlayerStatType.MaxHP, playerDetails.maxHp);
        stat.SetPlayerStat(PlayerStatType.BaseDamage, playerDetails.baseDamage);
        stat.SetPlayerStat(PlayerStatType.BaseArmor, playerDetails.baseArmor);
        stat.SetPlayerStat(PlayerStatType.Dodge, playerDetails.dodgeChance);
        stat.SetPlayerStat(PlayerStatType.CriticChance, playerDetails.criticChance);
        stat.SetPlayerStat(PlayerStatType.CriticDamage, playerDetails.criticDamage);
        stat.SetPlayerStat(PlayerStatType.MoveSpeed, playerDetails.moveSpeed);
        stat.SetPlayerStat(PlayerStatType.CircleRadius, playerDetails.circleRange);
        stat.SetPlayerStat(PlayerStatType.ExpGain, playerDetails.expGain);

        circleRange.radius = stat.circleRange;
    }

    private void Start()
    {
        AddWeaponToPlayer(playerDetails.playerStartingWeapon);
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
        health.SetHealthBar(); // 체력이 변경되면 체력바 반영
    }

    private void PlayerStatChangedEvent_OnPlayerStatChanged(PlayerStatChangedEvent arg1, PlayerStatChangedEventArgs args)
    {
        stat.ChangePlayerStat(args.statType, args.changeValue);

        switch (args.statType)
        {
            case PlayerStatType.MaxHP:
                health.SetMaxHealth((int)args.changeValue); // 최대체력 변경
                break;

            case PlayerStatType.BaseDamage: // Weapon 클래스로 가서 무기의 스탯 변경
                ChangePlayerWeaponStat(PlayerStatType.BaseDamage, args.changeValue);
                break;
            case PlayerStatType.CriticChance:
                ChangePlayerWeaponStat(PlayerStatType.CriticChance, args.changeValue); 
                break;
            case PlayerStatType.CriticDamage:
                ChangePlayerWeaponStat(PlayerStatType.CriticDamage, args.changeValue);
                break;

            case PlayerStatType.MoveSpeed:
                ctrl.moveSpeed = stat.moveSpeed; // 컨트롤러의 이동속도 변경
                break;

            case PlayerStatType.CircleRadius:
                circleRange.radius = stat.circleRange; // 아이템 획득범위 조정
                break;

            default:
                break;
        }
    }

    public Weapon AddWeaponToPlayer(WeaponDetailsSO weaponDetails)
    {
        // 추가할 무기 초기화
        Weapon playerWeapon = gameObject.AddComponent<Weapon>();
        playerWeapon.InitializeWeapon(weaponDetails);

        // 무기 추가되면서 캐릭터 스탯 반영
        playerWeapon.ChangeWeaponStat(PlayerStatType.BaseDamage, stat.baseDamage, false);
        playerWeapon.ChangeWeaponStat(PlayerStatType.CriticChance, stat.criticChance, false);
        playerWeapon.ChangeWeaponStat(PlayerStatType.CriticDamage, stat.criticDamage, false);

        weaponList.Add(playerWeapon); // 무기 리스트에 추가
        activeWeaponEvent.CallActiveWeaponEvent(playerWeapon, weaponList.Count-1); // 무기 UI 추가

        return playerWeapon;
    }

    private void ChangePlayerWeaponStat(PlayerStatType statType, float value)
    {
        foreach (Weapon weapon in weaponList)   // 플레이어가 가진 모든 무기 스탯 변경
        {
            weapon.ChangeWeaponStat(statType, value, false);
        }
    }

    public int TakeDamage(int damageAmount)
    {
        if (stat.dodgeChance >= 1)
        {
            // 회피에 성공
            if (Utilities.isSuccess(stat.dodgeChance)) return -1;
        }
                                // 방어력만큼 데미지 % 깎기
        damageAmount = Utilities.DecreaseByPercent(damageAmount, stat.baseArmor);

        health.SetCurrentHealth(damageAmount);
        health.CallHealthEvent(damageAmount);
        health.SetHealthBar();

        return damageAmount;
    }
}

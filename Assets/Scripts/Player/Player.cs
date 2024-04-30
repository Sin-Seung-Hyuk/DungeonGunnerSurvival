using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    private HorizontalLayoutGroup potionState; // 포션 사용시 체력바 위에 나타날 상태창
    private bool isBluePotionUsed = false;
    private bool isGreenPotionUsed = false;
    private bool isLimePotionUsed = false;



    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventoryHolder>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleRange = GetComponentInChildren<CircleCollider2D>();
        health = GetComponent<Health>();
        ctrl = GetComponent<PlayerCtrl>();
        playerExp = GetComponent<PlayerExp>();
        potionState = GetComponentInChildren<HorizontalLayoutGroup>();

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
        stat.ChangePlayerStat(args.statType, args.changeValue); // 스탯변경 함수 호출

        switch (args.statType)
        {
            case PlayerStatType.MaxHP:
                health.SetMaxHealth((int)args.changeValue); // 최대체력 변경
                health.AddHealth((int)(args.changeValue * 0.5f));
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


    // =================== Interface 구현 =============================================
    #region Interface
    public int TakeDamage(int ammoDamage,out int damageAmount)
    {
        // 방어력만큼 데미지 % 깎기 (수치가 높아질수록 효율 감소)
        //(int)(value / (value + Settings.combatScalingConstant));
        int armor = Utilities.CombatScaling(stat.baseArmor);
        damageAmount = Utilities.DecreaseByPercent(ammoDamage, armor);

        int dodge = Utilities.CombatScaling(stat.dodgeChance);
        if (dodge >= 1)
        {
            // 회피에 성공
            if (Utilities.isSuccess(dodge)) return -1;
        }

        health.SetCurrentHealth(damageAmount);
        health.CallHealthEvent(damageAmount);
        health.SetHealthBar();

        Debug.Log("damage: " + damageAmount);
        Debug.Log("armor: "+armor);
        return damageAmount;
    }
    #endregion


    // ============================ Potion ======================================
    #region Potion
    public void UsePotion(InventoryItemData itemData)
    {
        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.healthPickUp);

        PlayerStatType statType = itemData.playerStatChangeList[0].statType;
        float changeValue = itemData.playerStatChangeList[0].changeValue;
        Sprite potionSprite = itemData.ItemSprite;

        // 포션은 스탯을 하나만 변경해줌
        switch (statType)
        {
            case PlayerStatType.MaxHP:
                health.AddHealth((int)changeValue); // 체력회복
                break;

            case PlayerStatType.MoveSpeed:
                StartCoroutine(PotionRoutine(statType, changeValue, potionSprite)); // 포션 사용
                break;
            case PlayerStatType.CriticChance:
                StartCoroutine(PotionRoutine(statType, changeValue, potionSprite)); // 포션 사용
                break;
            case PlayerStatType.CriticDamage:
                StartCoroutine(PotionRoutine(statType, changeValue, potionSprite)); // 포션 사용
                break;
        }
    }

    private IEnumerator PotionRoutine(PlayerStatType statType, float changeValue, Sprite potionSprite)
    {
        SetPotionUsed(statType, true);

        playerStatChangedEvent.CallPlayerStatChangedEvent(statType, changeValue);

        GameObject potionStateImage = Instantiate(GameResources.Instance.potionStateImage, potionState.transform);
        potionStateImage.GetComponent<SpriteRenderer>().sprite = potionSprite;

        yield return new WaitForSeconds(Settings.potionDuration); // 포션 지속시간

        playerStatChangedEvent.CallPlayerStatChangedEvent(statType, -changeValue);
        Destroy(potionStateImage);
        SetPotionUsed(statType, false);
    }
    private void SetPotionUsed(PlayerStatType statType, bool isUsed)
    {
        // 해당 타입의 포션을 사용했는지 여부 설정
        switch (statType)
        {
            case PlayerStatType.MoveSpeed:
                isBluePotionUsed = isUsed;
                break;

            case PlayerStatType.CriticChance:
                isGreenPotionUsed = isUsed;
                break;

            case PlayerStatType.CriticDamage:
                isLimePotionUsed = isUsed;
                break;
        }
    }
    public bool CanUsePotion(PlayerStatType statType)
    {
        // 해당 타입의 포션을 사용할 수 있는지 반환 (같은포션 중복사용X, 체력포션은 상관X)
        switch (statType)
        {
            case PlayerStatType.MaxHP:
                return true;

            case PlayerStatType.MoveSpeed:
                return !isBluePotionUsed;

            case PlayerStatType.CriticChance:
                return !isGreenPotionUsed;

            case PlayerStatType.CriticDamage:
                return !isLimePotionUsed;
        }

        return false;
    }
    #endregion
}

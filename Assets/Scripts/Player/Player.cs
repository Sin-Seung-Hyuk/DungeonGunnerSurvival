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
    [HideInInspector] public CircleCollider2D circleRange; // �ڼ�����
    [HideInInspector] public PlayerStat stat; // ĳ���� ����
    [HideInInspector] public PlayerCtrl ctrl; // ĳ���� ��Ʈ�ѷ�
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
    [HideInInspector] public PlayerLevelUpEvent playerLevelUpEvent;

    public List<Weapon> weaponList { get; private set; } // ���� ����Ʈ
    public PlayerInventoryHolder playerInventory { get; private set; } // �÷��̾� �κ��丮

    private HorizontalLayoutGroup potionState; // ���� ���� ü�¹� ���� ��Ÿ�� ����â
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
        health.SetHealthBar(); // ü���� ����Ǹ� ü�¹� �ݿ�
    }

    private void PlayerStatChangedEvent_OnPlayerStatChanged(PlayerStatChangedEvent arg1, PlayerStatChangedEventArgs args)
    {
        stat.ChangePlayerStat(args.statType, args.changeValue); // ���Ⱥ��� �Լ� ȣ��

        switch (args.statType)
        {
            case PlayerStatType.MaxHP:
                health.SetMaxHealth((int)args.changeValue); // �ִ�ü�� ����
                health.AddHealth((int)(args.changeValue * 0.5f));
                break;

            case PlayerStatType.BaseDamage: // Weapon Ŭ������ ���� ������ ���� ����
                ChangePlayerWeaponStat(PlayerStatType.BaseDamage, args.changeValue);
                break;
            case PlayerStatType.CriticChance:
                ChangePlayerWeaponStat(PlayerStatType.CriticChance, args.changeValue); 
                break;
            case PlayerStatType.CriticDamage:
                ChangePlayerWeaponStat(PlayerStatType.CriticDamage, args.changeValue);
                break;

            case PlayerStatType.MoveSpeed:
                ctrl.moveSpeed = stat.moveSpeed; // ��Ʈ�ѷ��� �̵��ӵ� ����
                break;

            case PlayerStatType.CircleRadius:
                circleRange.radius = stat.circleRange; // ������ ȹ����� ����
                break;

            default:
                break;
        }
    }

    public Weapon AddWeaponToPlayer(WeaponDetailsSO weaponDetails)
    {
        // �߰��� ���� �ʱ�ȭ
        Weapon playerWeapon = gameObject.AddComponent<Weapon>();
        playerWeapon.InitializeWeapon(weaponDetails);

        // ���� �߰��Ǹ鼭 ĳ���� ���� �ݿ�
        playerWeapon.ChangeWeaponStat(PlayerStatType.BaseDamage, stat.baseDamage, false);
        playerWeapon.ChangeWeaponStat(PlayerStatType.CriticChance, stat.criticChance, false);
        playerWeapon.ChangeWeaponStat(PlayerStatType.CriticDamage, stat.criticDamage, false);

        weaponList.Add(playerWeapon); // ���� ����Ʈ�� �߰�
        activeWeaponEvent.CallActiveWeaponEvent(playerWeapon, weaponList.Count-1); // ���� UI �߰�

        return playerWeapon;
    }

    private void ChangePlayerWeaponStat(PlayerStatType statType, float value)
    {
        foreach (Weapon weapon in weaponList)   // �÷��̾ ���� ��� ���� ���� ����
        {
            weapon.ChangeWeaponStat(statType, value, false);
        }
    }


    // =================== Interface ���� =============================================
    #region Interface
    public int TakeDamage(int ammoDamage,out int damageAmount)
    {
        // ���¸�ŭ ������ % ��� (��ġ�� ���������� ȿ�� ����)
        //(int)(value / (value + Settings.combatScalingConstant));
        int armor = Utilities.CombatScaling(stat.baseArmor);
        damageAmount = Utilities.DecreaseByPercent(ammoDamage, armor);

        int dodge = Utilities.CombatScaling(stat.dodgeChance);
        if (dodge >= 1)
        {
            // ȸ�ǿ� ����
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

        // ������ ������ �ϳ��� ��������
        switch (statType)
        {
            case PlayerStatType.MaxHP:
                health.AddHealth((int)changeValue); // ü��ȸ��
                break;

            case PlayerStatType.MoveSpeed:
                StartCoroutine(PotionRoutine(statType, changeValue, potionSprite)); // ���� ���
                break;
            case PlayerStatType.CriticChance:
                StartCoroutine(PotionRoutine(statType, changeValue, potionSprite)); // ���� ���
                break;
            case PlayerStatType.CriticDamage:
                StartCoroutine(PotionRoutine(statType, changeValue, potionSprite)); // ���� ���
                break;
        }
    }

    private IEnumerator PotionRoutine(PlayerStatType statType, float changeValue, Sprite potionSprite)
    {
        SetPotionUsed(statType, true);

        playerStatChangedEvent.CallPlayerStatChangedEvent(statType, changeValue);

        GameObject potionStateImage = Instantiate(GameResources.Instance.potionStateImage, potionState.transform);
        potionStateImage.GetComponent<SpriteRenderer>().sprite = potionSprite;

        yield return new WaitForSeconds(Settings.potionDuration); // ���� ���ӽð�

        playerStatChangedEvent.CallPlayerStatChangedEvent(statType, -changeValue);
        Destroy(potionStateImage);
        SetPotionUsed(statType, false);
    }
    private void SetPotionUsed(PlayerStatType statType, bool isUsed)
    {
        // �ش� Ÿ���� ������ ����ߴ��� ���� ����
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
        // �ش� Ÿ���� ������ ����� �� �ִ��� ��ȯ (�������� �ߺ����X, ü�������� ���X)
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

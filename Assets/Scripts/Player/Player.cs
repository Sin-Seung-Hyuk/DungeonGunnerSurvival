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
    [HideInInspector] public PlayerStat stat; // 캐릭터 스탯

    // 플레이어가 가지는 이벤트
    [HideInInspector] public IdleEvent idleEvent;
    [HideInInspector] public MovementEvent movementEvent;
    [HideInInspector] public WeaponAimEvent weaponAimEvent;
    [HideInInspector] public FireWeaponEvent fireWeaponEvent;
    [HideInInspector] public WeaponFiredEvent weaponFiredEvent;
    [HideInInspector] public WeaponReloadedEvent weaponReloadedEvent;
    [HideInInspector] public ReloadWeaponEvent reloadWeaponEvent;
    [HideInInspector] public ActiveWeaponEvent activeWeaponEvent;


    private PlayerInventoryHolder playerInventory; // 플레이어 인벤토리

    public List<Weapon> weaponList = new List<Weapon>(); // 무기 리스트


    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventoryHolder>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleRange = GetComponent<CircleCollider2D>();
        idleEvent = GetComponent<IdleEvent>();
        movementEvent = GetComponent<MovementEvent>();
        weaponAimEvent = GetComponent<WeaponAimEvent>();
        fireWeaponEvent = GetComponent<FireWeaponEvent>();
        weaponFiredEvent = GetComponent<WeaponFiredEvent>();
        weaponReloadedEvent = GetComponent<WeaponReloadedEvent>();
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        activeWeaponEvent = GetComponent<ActiveWeaponEvent>();
    }


    public void InitializePlayer(PlayerDetailsSO playerDetails)
    {
        this.playerDetails = playerDetails;

        spriteRenderer.sprite = playerDetails.playerSprite;
        animator.runtimeAnimatorController = playerDetails.runtimeAnimatorController;

        // SetHP(playerDetails.maxHp);

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
        // 추가할 무기 초기화
        Weapon playerWeapon = gameObject.AddComponent<Weapon>();
        playerWeapon.InitializeWeapon(weaponDetails);

        weaponList.Add(playerWeapon); // 무기 리스트에 추가
        activeWeaponEvent.CallActiveWeaponEvent(playerWeapon, weaponList.Count-1); // 무기 UI 추가

        return playerWeapon;
    }
}

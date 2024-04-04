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
    }

    private void Start()
    {
        Debug.Log(weaponList[0].weaponName);
        Debug.Log(weaponList[0].weaponBaseDamage);
        Debug.Log(weaponList[0].weaponAmmoCapacity);
        Debug.Log(weaponList[0].weaponFireRate);
        Debug.Log(weaponList[0].weaponReloadTime);
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
        stat.circleRange  = playerDetails.circleRange;
        stat.dodgeChance = playerDetails.dodgeChance;
        stat.expGain = playerDetails.expGain;

        circleRange.radius = stat.circleRange;

        Weapon weapon = gameObject.AddComponent<Weapon>();
        weapon.InitializeWeapon(playerDetails.playerStartingWeapon);
        weaponList.Add(weapon);
    }
}

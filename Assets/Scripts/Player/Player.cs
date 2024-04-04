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

    // �÷��̾ ������ �̺�Ʈ
    [HideInInspector] public IdleEvent idleEvent;
    [HideInInspector] public MovementEvent movementEvent;
    [HideInInspector] public WeaponAimEvent weaponAimEvent;
    [HideInInspector] public FireWeaponEvent fireWeaponEvent;


    private PlayerInventoryHolder playerInventory; // �÷��̾� �κ��丮

    public List<Weapon> weaponList = new List<Weapon>(); // ���� ����Ʈ


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
    }

    private void Start()
    {

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

        foreach (var weapon in playerDetails.playerStartingWeapon)
        {
            Weapon playerWeapon = gameObject.AddComponent<Weapon>();
            playerWeapon.InitializeWeapon(weapon);

            weaponList.Add(playerWeapon);
        }

    }
}

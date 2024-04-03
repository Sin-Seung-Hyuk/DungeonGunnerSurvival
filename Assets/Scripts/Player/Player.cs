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
#endregion
public class Player : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public PlayerDetailsSO playerDetails;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public CircleCollider2D circleRange;

    [HideInInspector] public PlayerStat stat;

    private PlayerInventoryHolder playerInventory; // 플레이어 인벤토리

    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventoryHolder>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleRange = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {

    }

    public void InitializePlayer(PlayerDetailsSO playerDetails)
    {
        this.playerDetails = playerDetails;

        spriteRenderer.sprite = playerDetails.playerSprite;

        stat.BaseDamage = playerDetails.baseDamage;
        stat.CriticChance = playerDetails.criticChance;
        stat.CriticDamage = playerDetails.criticDamage;
        stat.ReloadSpeed = playerDetails.reloadSpeed;
        stat.FireRateSpeed = playerDetails.fireRateSpeed;
        stat.MoveSpeed = playerDetails.moveSpeed;
        stat.CircleRange  = playerDetails.circleRange;
        stat.DodgeChance = playerDetails.dodgeChance;
        stat.ExpGain = playerDetails.expGain;

        circleRange.radius = stat.CircleRange;
    }
}

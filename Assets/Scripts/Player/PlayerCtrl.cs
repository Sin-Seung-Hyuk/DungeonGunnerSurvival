using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] private Transform weaponShootPoint;
    private Player player;
    private PlayerStat playerStat;
    private Rigidbody2D rigid;

    private float moveSpeed;
    public Vector3 moveDirection { get; private set; }

    private void Awake()
    {
        player = GetComponent<Player>();
        rigid = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        playerStat = player.stat;
        moveSpeed = playerStat.moveSpeed;
    }

    void Update()
    {
        PlayerWeaponAim(); // 무기 조준, 사격
    }


    // ======================= Player Movement ==================================
    #region Player Movement
    private void FixedUpdate()
    {
        Move();
    }

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveDirection = new Vector3(input.x, input.y, 0f);
    }
    
    private void Move()
    {
        rigid.velocity = moveDirection * moveSpeed;

        if (moveDirection != Vector3.zero)
            player.movementEvent.CallPlayerMovement();
        else player.idleEvent.CallIdle();
    }
    #endregion

    // ======================= Weapon ==================================
    #region Weapon
    private void PlayerWeaponAim()
    {
        AimDirection playerAimDirection;
        float playerAimAngle;
        Vector3 playerAimDirectionVector;

        AimWeapon(out playerAimDirection, out playerAimAngle, out playerAimDirectionVector);
        FireWeapon(playerAimDirection, playerAimAngle, playerAimDirectionVector);
        
    }

    private void AimWeapon(out AimDirection playerAimDirection, out float playerAimAngle, out Vector3 playerAimDirectionVector)
    {
        Vector3 mousePos = Utilities.GetMouseCursorPos();

        playerAimDirectionVector = mousePos - weaponShootPoint.position;
        playerAimAngle = Utilities.GetAngleFromVector(playerAimDirectionVector);
        playerAimDirection = Utilities.GetAimDirectionFromAngle(playerAimAngle);

        player.weaponAimEvent.CallWeaponAim(playerAimDirection, playerAimAngle, playerAimDirectionVector);
    }

    private void FireWeapon(AimDirection playerAimDirection, float playerAimAngle, Vector3 playerAimDirectionVector)
    {
        foreach (Weapon weapon in player.weaponList)
        {
            player.fireWeaponEvent.CallFireWeaponEvent(weapon, playerAimDirection, playerAimAngle, playerAimDirectionVector);
        }
    }
    #endregion
}

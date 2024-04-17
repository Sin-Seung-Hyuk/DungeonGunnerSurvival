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
    private Vector3 moveDirection;

    public float moveSpeed;


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
        AimDirection playerAimDirection; // 에임 방향
        float playerAimAngle;            // 에임 각도
        Vector3 playerAimDirectionVector;// 에임 방향벡터

        AimWeapon(out playerAimDirection, out playerAimAngle, out playerAimDirectionVector); 
        FireWeapon( playerAimAngle, playerAimDirectionVector);
        
    }

    private void AimWeapon(out AimDirection playerAimDirection, out float playerAimAngle, out Vector3 playerAimDirectionVector)
    {
        Vector3 mousePos = Utilities.GetMouseCursorPos(); // 마우스 위치 구하기

        playerAimDirectionVector = mousePos - weaponShootPoint.position; // 캐릭터->마우스 방향벡터 구하기
        playerAimAngle = Utilities.GetAngleFromVector(playerAimDirectionVector); // 방향벡터로 에임각도 구하기
        playerAimDirection = Utilities.GetAimDirectionFromAngle(playerAimAngle); // 에임각도로 에임방향 구하기

        player.weaponAimEvent.CallWeaponAim(playerAimDirection, playerAimAngle, playerAimDirectionVector); // 해당 위치로 조준
    }

    private void FireWeapon(float playerAimAngle, Vector3 playerAimDirectionVector)
    {
        for (int i=0;i< player.weaponList.Count; ++i) // 플레이어의 무기 리스트 각각 사격이벤트 호출
        {
            player.fireWeaponEvent.CallFireWeaponEvent(player.weaponList[i], playerAimAngle, playerAimDirectionVector, i);
        }
    }
    #endregion
}

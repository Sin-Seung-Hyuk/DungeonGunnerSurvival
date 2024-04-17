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
        PlayerWeaponAim(); // ���� ����, ���
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
        AimDirection playerAimDirection; // ���� ����
        float playerAimAngle;            // ���� ����
        Vector3 playerAimDirectionVector;// ���� ���⺤��

        AimWeapon(out playerAimDirection, out playerAimAngle, out playerAimDirectionVector); 
        FireWeapon( playerAimAngle, playerAimDirectionVector);
        
    }

    private void AimWeapon(out AimDirection playerAimDirection, out float playerAimAngle, out Vector3 playerAimDirectionVector)
    {
        Vector3 mousePos = Utilities.GetMouseCursorPos(); // ���콺 ��ġ ���ϱ�

        playerAimDirectionVector = mousePos - weaponShootPoint.position; // ĳ����->���콺 ���⺤�� ���ϱ�
        playerAimAngle = Utilities.GetAngleFromVector(playerAimDirectionVector); // ���⺤�ͷ� ���Ӱ��� ���ϱ�
        playerAimDirection = Utilities.GetAimDirectionFromAngle(playerAimAngle); // ���Ӱ����� ���ӹ��� ���ϱ�

        player.weaponAimEvent.CallWeaponAim(playerAimDirection, playerAimAngle, playerAimDirectionVector); // �ش� ��ġ�� ����
    }

    private void FireWeapon(float playerAimAngle, Vector3 playerAimDirectionVector)
    {
        for (int i=0;i< player.weaponList.Count; ++i) // �÷��̾��� ���� ����Ʈ ���� ����̺�Ʈ ȣ��
        {
            player.fireWeaponEvent.CallFireWeaponEvent(player.weaponList[i], playerAimAngle, playerAimDirectionVector, i);
        }
    }
    #endregion
}

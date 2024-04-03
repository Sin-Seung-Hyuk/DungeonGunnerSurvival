using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimate : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void OnDisable()
    {
        player.idleEvent.OnIdle -= Idle_OnIdle;
        player.movementEvent.OnPlayerMovement -= Movement_OnPlayerMovement;
    }

    private void Start()
    {
        player.idleEvent.OnIdle += Idle_OnIdle;
        player.movementEvent.OnPlayerMovement += Movement_OnPlayerMovement;
    }


    // ======================================================= IDLE ==============================================
    #region Idle �̺�Ʈ�� ������ �Լ�
    private void Idle_OnIdle(IdleEvent obj)
    {
        SetIdleParameter();
    }

    private void SetIdleParameter()
    {
        // Settings Ŭ�������� StringToHash �Լ��� �Ķ���Ϳ� ������ ���ٰ���
        player.animator.SetBool(Settings.isMoving, false);
        player.animator.SetBool(Settings.isIdle, true);
    }
    #endregion


    // ======================================================= Movement ==============================================
    #region Movement �̺�Ʈ�� ������ �Լ�
    private void Movement_OnPlayerMovement(MovementEvent arg)
    {
        SetMovementParameter();
    }

    private void SetMovementParameter()
    {
        player.animator.SetBool(Settings.isMoving, true);
        player.animator.SetBool(Settings.isIdle, false);
    }
    #endregion

}

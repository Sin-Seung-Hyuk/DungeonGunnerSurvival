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

    private void Start() // �÷��̾� Awake���� Animate�� OnEnable�� ���� �����
    {
        player.idleEvent.OnIdle += Idle_OnIdle;
        player.movementEvent.OnPlayerMovement += Movement_OnPlayerMovement;
        player.weaponAimEvent.OnWeaponAim += WeaponAim_OnWeaponAim;
    }

    private void OnDisable()
    {
        player.idleEvent.OnIdle -= Idle_OnIdle;
        player.movementEvent.OnPlayerMovement -= Movement_OnPlayerMovement;
        player.weaponAimEvent.OnWeaponAim -= WeaponAim_OnWeaponAim;
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


    // ======================================================= Weapon ==============================================
    #region Weapon �̺�Ʈ�� ������ �Լ�
    private void WeaponAim_OnWeaponAim(WeaponAimEvent arg1, WeaponAimEventArgs args)
    {
        InitializeAimAnimationParameters(); // ���ӹ��� �켱 false�� ����
        SetWeaponAimParameter(args.aimDirection);
    }

    private void InitializeAimAnimationParameters()
    {
        player.animator.SetBool(Settings.aimUp, false);
        player.animator.SetBool(Settings.aimUpRight, false);
        player.animator.SetBool(Settings.aimUpLeft, false);
        player.animator.SetBool(Settings.aimRight, false);
        player.animator.SetBool(Settings.aimLeft, false);
        player.animator.SetBool(Settings.aimDown, false);
    }

    // AimDirection ���� �ִϸ����� �Ķ���� ����
    private void SetWeaponAimParameter(AimDirection aimDirection)
    {
        switch (aimDirection)
        {
            case AimDirection.Up:
                player.animator.SetBool(Settings.aimUp, true);
                break;

            case AimDirection.UpRight:
                player.animator.SetBool(Settings.aimUpRight, true);
                break;

            case AimDirection.UpLeft:
                player.animator.SetBool(Settings.aimUpLeft, true);
                break;

            case AimDirection.Right:
                player.animator.SetBool(Settings.aimRight, true);
                break;

            case AimDirection.Left:
                player.animator.SetBool(Settings.aimLeft, true);
                break;

            case AimDirection.Down:
                player.animator.SetBool(Settings.aimDown, true);
                break;
        }
    }
    #endregion
}

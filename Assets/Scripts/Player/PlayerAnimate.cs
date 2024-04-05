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

    private void Start() // 플레이어 Awake보다 Animate의 OnEnable이 먼저 실행됨
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
    #region Idle 이벤트에 구독한 함수
    private void Idle_OnIdle(IdleEvent obj)
    {
        SetIdleParameter();
    }

    private void SetIdleParameter()
    {
        // Settings 클래스에서 StringToHash 함수로 파라미터에 빠르게 접근가능
        player.animator.SetBool(Settings.isMoving, false);
        player.animator.SetBool(Settings.isIdle, true);
    }
    #endregion


    // ======================================================= Movement ==============================================
    #region Movement 이벤트에 구독한 함수
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
    #region Weapon 이벤트에 구독한 함수
    private void WeaponAim_OnWeaponAim(WeaponAimEvent arg1, WeaponAimEventArgs args)
    {
        InitializeAimAnimationParameters(); // 에임방향 우선 false로 시작
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

    // AimDirection 으로 애니메이터 파라미터 변경
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

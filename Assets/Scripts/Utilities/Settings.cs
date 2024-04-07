using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{


    #region Animator Parameter
    public static int aimUp = Animator.StringToHash("aimUp");
    public static int aimDown = Animator.StringToHash("aimDown");
    public static int aimUpRight = Animator.StringToHash("aimUpRight");
    public static int aimUpLeft = Animator.StringToHash("aimUpLeft");
    public static int aimRight = Animator.StringToHash("aimRight");
    public static int aimLeft = Animator.StringToHash("aimLeft");
    public static int isIdle = Animator.StringToHash("isIdle");
    public static int isMoving = Animator.StringToHash("isMoving");
    public static int rollUp = Animator.StringToHash("rollUp");
    public static int rollDown = Animator.StringToHash("rollDown");
    public static int rollRight = Animator.StringToHash("rollRight");
    public static int rollLeft = Animator.StringToHash("rollLeft");
    #endregion


    #region ASTAR PARAMETERS
    public const int defaultAStarMovementPenalty = 40;
    public const int preferredPathAStarMovementPenalty = 1;
    public const float playerMoveDistanceToRebuildPath = 3f;
    public const float enemyPathRebuildCooldown = 2f;
    // 목표 프레임 수
    public const int targetFrameRateToSpreadPathFindingOver = 60;
    #endregion


    #region GAMEOBJECT TAGS
    public const string playerTag = "Player";
    public const string playerWeapon = "playerWeapon";
    #endregion


    #region ENEMY PARAMETERS
    public const int defaultEnemyHealth = 20;
    #endregion

    #region ENEMY CONTACT DAMAGE PARAMETERS
    public const float contactsDamageDelay = 0.5f; // 0.5초마다 부딪히면 데미지입힘
    #endregion

    #region UI PARAMETERS
    public const float uiHeartSpacing = 16f;
    #endregion

    #region AUDIO
    public const float musicFadeOutTime = 0.5f;
    public const float musicFadeInTime = 0.5f;
    #endregion

}

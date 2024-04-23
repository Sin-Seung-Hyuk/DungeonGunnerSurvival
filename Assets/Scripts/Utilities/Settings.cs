using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    #region PLAYER PARAMETER
    public static float potionDuration = 30f; // 포션 지속시간
    public static int rerollGold = 100; // 상점 새로고침 비용
    public static int startExp = 50; // 1레벨 최대경험치
    #endregion


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

    public static int use = Animator.StringToHash("use");
    public static int destroy = Animator.StringToHash("destroy");
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

    #region DUNGEON PARAMETERS
    public const int dungeonTimer = 30; // 던전 타이머
    #endregion

    #region ENEMY PARAMETERS
    public const int defaultEnemyHealth = 20;
    #endregion

    #region ENEMY CONTACT DAMAGE PARAMETERS
    public const float contactsDamageDelay = 0.5f; // 0.5초마다 부딪히면 데미지입힘
    #endregion

    #region UI PARAMETERS
    #endregion

    #region AUDIO
    public const float musicFadeOutTime = 0.5f;
    public const float musicFadeInTime = 0.5f;
    #endregion

}

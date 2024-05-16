using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    #region PLAYER PARAMETER
    public static float potionDuration = 60f; // 포션 지속시간
    public static int rerollGold = 1000; // 상점 새로고침 비용
    public static int startExp = 25; // 1레벨 최대경험치
    public static int weaponUpgrade = 10; // 무기 업그레이드 레벨
    public static float combatScalingConstant = 50f; // 방어력,회피율 계산을 위한 상수 (수치가 높아질수록 효율이 감소하는 커브)
    public static float burnDamage = 0.5f; // 틱데미지 비율 (공격력의 n%)
    public static float burnDistance = 0.5f; // 틱데미지 간격
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
    public const int dungeonTimer = 150; // 던전 타이머
    public const int potionDropChacne = 50; // 포션 드랍확률 50%
    public const int lastLegendItemID = 1032; // 유니크,전설 아이템의 마지막 ID +1 (랜덤범위 지정용)
    #endregion

    #region ENEMY PARAMETERS
    public const int defaultEnemyHealth = 20;
    #endregion

    #region ENEMY CONTACT DAMAGE PARAMETERS
    public const float contactsDamageDelay = 0.5f; // 0.5초마다 부딪히면 데미지입힘
    #endregion

    #region UI PARAMETERS
    public static Color32 blue = new Color32(76 ,115, 209,255);
    public static Color32 red = new Color32(255 ,112, 120, 255);
    public static Color32 legend = new Color32(226 ,125, 19, 255);
    public static Color32 critical = new Color32(255, 102, 2, 255);
    #endregion

    #region AUDIO
    public const float musicFadeOutTime = 0.5f;
    public const float musicFadeInTime = 0.5f;
    #endregion

}

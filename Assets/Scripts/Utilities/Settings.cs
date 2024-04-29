using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    #region PLAYER PARAMETER
    public static float potionDuration = 60f; // ���� ���ӽð�
    public static int rerollGold = 100; // ���� ���ΰ�ħ ���
    public static int startExp = 30; // 1���� �ִ����ġ
    public static int weaponUpgrade = 15; // ���� ���׷��̵� ����
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
    // ��ǥ ������ ��
    public const int targetFrameRateToSpreadPathFindingOver = 60;
    #endregion


    #region GAMEOBJECT TAGS
    public const string playerTag = "Player";
    public const string playerWeapon = "playerWeapon";
    #endregion

    #region DUNGEON PARAMETERS
    public const int dungeonTimer = 150; // ���� Ÿ�̸�
    #endregion

    #region ENEMY PARAMETERS
    public const int defaultEnemyHealth = 20;
    #endregion

    #region ENEMY CONTACT DAMAGE PARAMETERS
    public const float contactsDamageDelay = 0.5f; // 0.5�ʸ��� �ε����� ����������
    #endregion

    #region UI PARAMETERS
    public static Color32 blue = new Color32(76 ,115, 209,255);
    public static Color32 red = new Color32(255 ,112, 120, 255);
    public static Color32 legend = new Color32(226 ,125, 19, 255);
    #endregion

    #region AUDIO
    public const float musicFadeOutTime = 0.5f;
    public const float musicFadeInTime = 0.5f;
    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    

    // ===== Animator Parameter =======================================================
    #region
    public static int isMoving = Animator.StringToHash("isMoving");
    public static int isIdle = Animator.StringToHash("isIdle");
    public static int aimUp = Animator.StringToHash("aimUp");
    public static int aimUpRight = Animator.StringToHash("aimUpRight");
    public static int aimUpLeft = Animator.StringToHash("aimUpLeft");
    public static int aimDown = Animator.StringToHash("aimDown");
    public static int aimRight = Animator.StringToHash("aimRight");
    public static int aimLeft = Animator.StringToHash("aimLeft");
    #endregion
}

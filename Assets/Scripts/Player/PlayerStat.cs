using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStat
{
    private int baseDamage;
    private int criticChance;
    private int criticDamage;
    private int reloadSpeed;
    private int fireRateSpeed;
    private float moveSpeed;
    private float circleRange; // 자석 범위
    private int dodgeChance;
    private int expGain;

    public int BaseDamage
    {
        get { return baseDamage; }
        set { baseDamage = value; }
    }

    public int CriticChance
    {
        get { return criticChance; }
        set { criticChance = value; }
    }

    public int CriticDamage
    {
        get { return criticDamage; }
        set { criticDamage = value; }
    }

    public int ReloadSpeed
    {
        get { return reloadSpeed; }
        set { reloadSpeed = value; }
    }

    public int FireRateSpeed
    {
        get { return fireRateSpeed; }
        set { fireRateSpeed = value; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    public float CircleRange
    {
        get { return circleRange; }
        set { circleRange = value; }
    }

    public int DodgeChance
    {
        get { return dodgeChance; }
        set { dodgeChance = value; }
    }

    public int ExpGain
    {
        get { return expGain; }
        set { expGain = value; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStat 
{
    public int maxHp { get; private set; }
    public int baseDamage    {get; private set; }
    public int baseArmor { get; private set; }
    public int dodgeChance { get; private set; }
    public float reloadSpeed   {get; private set; }
    public float fireRateSpeed {get; private set; }
    public float moveSpeed   {get; private set; }
    public float circleRange {get; private set;}
    public int expGain       { get; private set; }



    // Set : �ش� value�� ������ �����ع��� (��,���� �ƴ�)
    #region Set Stat
    public void SetPlayerStat(PlayerStatType statType, int value)
    {
        switch (statType)
        {
            case PlayerStatType.MaxHP:
                maxHp = value;
                break;
            case PlayerStatType.BaseDamage:
                baseDamage = value;
                break;
            case PlayerStatType.BaseArmor:
                baseArmor = value;
                break;
            case PlayerStatType.Dodge:
                dodgeChance = value;
                break;
            case PlayerStatType.ExpGain:
                expGain = value;
                break;
            default:
                break;
        }
    }
    public void SetPlayerStat(PlayerStatType statType, float value)
    {
        switch (statType)
        {
            case PlayerStatType.ReloadSpeed:
                reloadSpeed = value;
                break;
            case PlayerStatType.FireRate:
                fireRateSpeed = value;
                break;
            case PlayerStatType.MoveSpeed:
                moveSpeed = value;
                break;
            case PlayerStatType.CircleRadius:
                circleRange = value;
                break;
            default:
                break;
        }
    }
    #endregion


    // �÷��̾� ���� ���� (�ش� value��ŭ ��ȭ)
    #region
    public void ChangePlayerStat(PlayerStatType statType, float value)
    {
        switch (statType)
        {
            case PlayerStatType.MaxHP:
                maxHp += (int)value;
                break;
            case PlayerStatType.BaseDamage:
                baseDamage += (int)value;
                break;
            case PlayerStatType.BaseArmor:
                baseArmor += (int)value;
                break;
            case PlayerStatType.Dodge:
                dodgeChance += (int)value;
                break;
            case PlayerStatType.ExpGain:
                expGain += (int)value;
                break;
            case PlayerStatType.ReloadSpeed:
                reloadSpeed += value;
                break;
            case PlayerStatType.FireRate:
                fireRateSpeed += value;
                break;
            case PlayerStatType.MoveSpeed:
                moveSpeed += value;
                break;
            case PlayerStatType.CircleRadius:
                circleRange += value;
                break;
            default:
                break;
        }
    }
    #endregion
}


using UnityEngine;

[System.Serializable]
public class PlayerLevelUpList
{
    //스탯타입,수치,텍스트string,컬러,플레이어 여부
    public PlayerStatType statType;
    public float changeValue;
    public string choiceText; // UI에 표시할 텍스트
    public string changeValueText; // UI에 표시할 텍스트
    public Color txtColor; // UI에 표시될 컬러
    public bool isPlayerStat; // 플레이어 스탯인지, 무기 스탯인지 여부
}

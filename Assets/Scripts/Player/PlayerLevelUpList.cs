
using UnityEngine;

[System.Serializable]
public class PlayerLevelUpList
{
    //����Ÿ��,��ġ,�ؽ�Ʈstring,�÷�,�÷��̾� ����
    public PlayerStatType statType;
    public float changeValue;
    public string choiceText; // UI�� ǥ���� �ؽ�Ʈ
    public string changeValueText; // UI�� ǥ���� �ؽ�Ʈ
    public Color txtColor; // UI�� ǥ�õ� �÷�
    public bool isPlayerStat; // �÷��̾� ��������, ���� �������� ����
}

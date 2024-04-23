using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExp : MonoBehaviour
{
    [SerializeField] private ExpBarUI expBar;
    private int level;
    private int maxExp;
    private int currentExp;
    private Player player;


    private void Start()
    {
        player = GetComponent<Player>();
        level = 1;

        currentExp = 0;
        maxExp = Settings.startExp; // ���� ����ġ 

        TakeExp(0);
        expBar.SetLevel(level);
    }

    public void TakeExp(int expAmount)
    {
        currentExp += expAmount;

        if (currentExp >= maxExp)
        {
            LevelUp();
        }
        
        if (expBar != null)
            expBar.SetExpBar((float)currentExp / (float)maxExp);
        
    }

    private void LevelUp()
    {
        currentExp -= maxExp;
        maxExp = maxExp + (int)(maxExp * 0.05f); // �ִ����ġ 5% ����

        expBar.SetLevel(++level);

        player.playerLevelUpEvent.CallPlayerLevelUpEvent(); // ������ �̺�Ʈ ȣ��
    }
}


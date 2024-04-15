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
        maxExp = 100; // 시작 경험치 100

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
        maxExp = maxExp + (int)(maxExp * 0.05f); // 최대경험치 5% 증가

        expBar.SetLevel(++level);
    }
}

[System.Serializable]
public class PlayerLevelExp
{
    int level;  // 레벨
    int maxExp; // 다음 레벨까지 필요 경험치
}
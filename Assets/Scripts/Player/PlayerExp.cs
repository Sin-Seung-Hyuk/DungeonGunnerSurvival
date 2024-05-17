using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExp : MonoBehaviour
{
    [SerializeField] private ExpBarUI expBar;
    public int level { get; private set; }
    private int maxExp;
    private int currentExp;
    private Player player;


    private void Start()
    {
        player = GetComponent<Player>();
        level = 1;

        currentExp = 0;
        maxExp = Settings.startExp; // 시작 경험치 

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
        maxExp = maxExp + (int)(maxExp * 0.1f); // 최대경험치 5% 증가

        expBar.SetLevel(++level);

        player.playerLevelUpEvent.CallPlayerLevelUpEvent(); // 레벨업 이벤트 호출

        Debug.Log("언제 호출되는지 타이밍 검사");
    }
}


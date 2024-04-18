using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerStatUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] playerStatArray;
    [SerializeField] private Player player;


    private void Start()
    {
        player.playerStatChangedEvent.OnPlayerStatChanged += PlayerStatChangedEvent_OnPlayerStatChanged;
    }

    private void OnEnable()
    {
        SetPlayerStatText();
    }

    private void PlayerStatChangedEvent_OnPlayerStatChanged(PlayerStatChangedEvent arg1, PlayerStatChangedEventArgs arg2)
    {
        SetPlayerStatText();
    }

    private void SetPlayerStatText()
    {
        playerStatArray[(int)PlayerStatType.MaxHP].text = player.stat.maxHp.ToString();
        playerStatArray[(int)PlayerStatType.BaseDamage].text = player.stat.baseDamage.ToString();
        playerStatArray[(int)PlayerStatType.BaseArmor].text = player.stat.baseArmor.ToString();
        playerStatArray[(int)PlayerStatType.Dodge].text = player.stat.dodgeChance.ToString();
        playerStatArray[(int)PlayerStatType.CriticChance].text = player.stat.criticChance.ToString();
        playerStatArray[(int)PlayerStatType.CriticDamage].text = player.stat.criticDamage.ToString();
        playerStatArray[(int)PlayerStatType.MoveSpeed].text = player.stat.moveSpeed.ToString();
        playerStatArray[(int)PlayerStatType.CircleRadius].text = player.stat.circleRange.ToString();
        playerStatArray[(int)PlayerStatType.ExpGain].text = player.stat.expGain.ToString();
    }
}

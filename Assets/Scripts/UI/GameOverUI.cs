using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameOverUI : MonoBehaviour
{
    [SerializeField] private List<GameOverUIComponents> GameOverUIComponentList;
    [SerializeField] private Image playerImage;
    [SerializeField] private TextMeshProUGUI TxtPlayerLevel;
    [SerializeField] private TextMeshProUGUI TxtTotalKills;
    [SerializeField] private TextMeshProUGUI TxtTotalSpentGold;

    private Player player;


    private void OnEnable()
    {
        player = GameManager.Instance.GetPlayer();

        SetGameOverUIText();
    }

    private void SetGameOverUIText()
    {
        for (int i = 0; i < player.weaponList.Count; ++i)
        {
            Weapon weapon = player.weaponList[i];

            if (weapon != null)
            {
                GameOverUIComponentList[i].weaponSprite.color = new Color(1f, 1f, 1f, 1f);
                GameOverUIComponentList[i].weaponSprite.sprite = weapon.weaponSprite;
                GameOverUIComponentList[i].TxtLevel.text = weapon.weaponLevel.ToString();
                GameOverUIComponentList[i].TxtDamage.text = StatisticsManager.Instance.GetDamageStatistics(weapon.weaponDetail.weaponType).ToString();
            }
        }

        playerImage.sprite = player.playerDetails.playerSprite;
        TxtPlayerLevel.text = "Level : "+ player.playerExp.level.ToString();
    }
}

[System.Serializable]
public class GameOverUIComponents
{
    public Image weaponSprite;
    public TextMeshProUGUI TxtLevel;
    public TextMeshProUGUI TxtDamage;
}
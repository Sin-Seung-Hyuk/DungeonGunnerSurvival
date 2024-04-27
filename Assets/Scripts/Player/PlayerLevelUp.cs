
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelUp : MonoBehaviour
{

    [SerializeField] List<SpawnableObjectRatio<PlayerLevelUpList>> levelUpList; // 레벨업 선택지 리스트
    [SerializeField] PlayerLevelUpUI levelUpUI; // 레벨업 UI
    private RandomSpawnableObject<PlayerLevelUpList> playerLevelUpList; // 랜덤으로 선택될 레벨업 선택지
    private PlayerLevelUpEvent playerLevelUpEvent;

    private void Awake()
    {
        playerLevelUpEvent = GetComponent<PlayerLevelUpEvent>();
        playerLevelUpList = new RandomSpawnableObject<PlayerLevelUpList>(levelUpList);
    }

    private void OnEnable()
    {
        playerLevelUpEvent.OnPlayerLevelUp += PlayerLevelUpEvent_OnPlayerLevelUp;
    }
    private void OnDisable()
    {
        playerLevelUpEvent.OnPlayerLevelUp -= PlayerLevelUpEvent_OnPlayerLevelUp;
    }

    private void PlayerLevelUpEvent_OnPlayerLevelUp(PlayerLevelUpEvent obj)
    {
        if (levelUpList.Count > 0)
        {
            List<int> randomChoice = new List<int>();

            while (randomChoice.Count < 3)
            {
                int randomNumber = Random.Range(0, playerLevelUpList.ratioValueTotal); // 레벨업 종류 결정 
                if (!randomChoice.Contains(randomNumber))
                {
                    randomChoice.Add(randomNumber);
                }
            }

            for (int i = 0; i < 3; i++)
            {                      // 랜덤으로 뽑은 번호의 레벨업 선택지 매개변수로 UI 초기화
                levelUpUI.InitializeLevelUpUI(playerLevelUpList.GetItem(randomChoice[i]), i);
            }

            levelUpUI.gameObject.SetActive(true); // 레벨업 UI 모두 초기화 후 활성화
        }
    }
}


using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelUp : MonoBehaviour
{

    [SerializeField] List<SpawnableObjectRatio<PlayerLevelUpList>> levelUpList; // ������ ������ ����Ʈ
    [SerializeField] PlayerLevelUpUI levelUpUI; // ������ UI
    private RandomSpawnableObject<PlayerLevelUpList> playerLevelUpList; // �������� ���õ� ������ ������
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
                int randomNumber = Random.Range(0, playerLevelUpList.ratioValueTotal); // ������ ���� ���� 
                if (!randomChoice.Contains(randomNumber))
                {
                    randomChoice.Add(randomNumber);
                }
            }

            for (int i = 0; i < 3; i++)
            {                      // �������� ���� ��ȣ�� ������ ������ �Ű������� UI �ʱ�ȭ
                levelUpUI.InitializeLevelUpUI(playerLevelUpList.GetItem(randomChoice[i]), i);
            }

            levelUpUI.gameObject.SetActive(true); // ������ UI ��� �ʱ�ȭ �� Ȱ��ȭ
        }
    }
}

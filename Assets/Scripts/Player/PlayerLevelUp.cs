
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelUp : MonoBehaviour
{

    [SerializeField] List<SpawnableObjectRatio<PlayerLevelUpList>> levelUpList; // ������ ������ ����Ʈ
    [SerializeField] PlayerLevelUpUI levelUpUI; // ������ UI
    private RandomSpawnableObject<PlayerLevelUpList> playerLevelUpList; // �������� ���õ� ������ ������
    private Player player;
    private PlayerLevelUpEvent playerLevelUpEvent;

    private void Awake()
    {
        player = GetComponent<Player>();
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
            int[] randomChoice = new int[3];

            for (int i = 0; i < 3; i++)
            {
                randomChoice[i] = Random.Range(0, playerLevelUpList.ratioValueTotal); // �������� ���� ���� 
                for (int j = 0; j < i; j++)
                {
                    if (randomChoice[i] == randomChoice[j]) i--;
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

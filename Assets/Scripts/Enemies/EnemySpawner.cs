
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    private int enemiesToSpawn;
    private int currentEnemyCount;
    private int enemiesSpawnedSoFar;
    private int enemyMaxConcurrentSpawnNumber;
    private Room currentRoom;
    //private RoomEnemySpawnParameters spawnParameters;

    [SerializeField] private GameObject enemyPrefab; // �����ϱ� ���� �������� ������
    [SerializeField] private GameObject itemPrefab;

    private RandomSpawnableObject<EnemyDetailsSO> enemySpawnable;
    private List<int> randomEnemy;
    private RandomSpawnableObject<InventoryItemData> itemSpawnable;
    private List<int> randomItem;
    private int randomItemIdx = 0;


    private void OnEnable()
    {
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;
    }
    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;
    }

    private void StaticEventHandler_OnRoomChanged(RoomChangedArgs args)
    {
        currentRoom = args.room;
        if (args.room.spawnPositionArray.Count < 1) return; // ���������� ���� ���� ����

        SetRandomNumberList(); // ������ ����������Ʈ �̸� ���صα�
         
        StartCoroutine(SpawnEnemiesRoutine()); // ������ ������ �������ڴ�� ���� (�����ɰ� �̸� ������ ����)
    }

    private void SetRandomNumberList()
    {
        // ������ �������� ������ ��,������ ����Ʈ�� �޾ƿ� �������� �Ű������� �־���

        enemySpawnable = new RandomSpawnableObject<EnemyDetailsSO>(currentRoom.spawnableEnemyList);
        randomEnemy = new List<int>(100);
        for (int i = 0; i < 100; ++i)
        {
            randomEnemy.Add(Random.Range(0, enemySpawnable.ratioValueTotal)); // 100���� �������� ����
        }

        itemSpawnable = new RandomSpawnableObject<InventoryItemData>(currentRoom.spawnableItemList);
        randomItem = new List<int>(100);
        for (int i = 0; i < 100; ++i)
        {
            randomItem.Add(Random.Range(0, itemSpawnable.ratioValueTotal)); // 100���� �������� ����
        }
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        for (int i = 0; i < 40; i++) // �� ����Ƚ������ �ݺ�
        {
            // �׸��� ���� �������� �迭���� �������� ������ �������� �����
            Vector3Int cellPos = (Vector3Int)currentRoom.spawnPositionArray[Random.Range(0, currentRoom.spawnPositionArray.Count)];

            Enemy enemyObj = (Enemy)ObjectPoolManager.Instance.Release(enemyPrefab, cellPos, Quaternion.identity);

            // �������� ���� ���� 100�� �߿��� �Ѱ��� ��� �ش� ������ �� ����
            enemyObj.GetComponent<Enemy>().EnemyInitialization(enemySpawnable.GetItem(randomEnemy[i%100]), GameManager.instance.GetCurrentDungeonLevelSO());

            // ���� �ı� �̺�Ʈ ����
            enemyObj.GetComponent<DestroyedEvent>().OnDestroyed += Enemy_OnDestroyed;

            yield return new WaitForSeconds(2.0f); // �����Ķ���Ϳ��� �������� ���ؿ���
        }

    }

    private void Enemy_OnDestroyed(DestroyedEvent arg1, DestroyedEventArgs args)
    {                                                                                 // �ı��� ��ġ
        ItemPickUp itemObj = (ItemPickUp)ObjectPoolManager.Instance.Release(itemPrefab, args.point, Quaternion.identity);
        
        // �������� ���� ���� 100�� �߿��� �Ѱ��� ��� �ش� ������ ������ ����
        itemObj.GetComponent<ItemPickUp>().InitializeItem(itemSpawnable.GetItem(randomItem[randomItemIdx++%100]));
    }
}

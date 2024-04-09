
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

    // Test
    [SerializeField] private EnemyDetailsSO enemyDetails;
    [SerializeField] private GameObject enemy;

    [SerializeField] private InventoryItemData[] inventoryItemData;
    [SerializeField] private GameObject item;

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

        if (args.room.spawnPositionArray.Count < 1) return;

        InventoryItemData data = inventoryItemData[Random.Range(0, inventoryItemData.Length)];

        // 그리드 기준 스폰가능 배열에서 랜덤으로 가져와 스폰지점 만들기
        Vector3Int cellPos = (Vector3Int)currentRoom.spawnPositionArray[Random.Range(0, currentRoom.spawnPositionArray.Count)];

        ItemPickUp enemyObj = (ItemPickUp)ObjectPoolManager.Instance.Release(item, cellPos, Quaternion.identity);
        // 현재까지 생성된 적의 수와 던전레벨,디테일 스크립터블 옵젝을 넘겨 Enemy 초기화
        enemyObj.GetComponent<ItemPickUp>().InitializeItem(data);

        //StartCoroutine(SpawnEnemiesRoutine());
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        if (currentRoom.spawnPositionArray.Count > 0)
        {
            for (int i = 0; i < 40; i++) // 총 스폰횟수까지 반복
            {
                // 그리드 기준 스폰가능 배열에서 랜덤으로 가져와 스폰지점 만들기
                Vector3Int cellPos = (Vector3Int)currentRoom.spawnPositionArray[Random.Range(0, currentRoom.spawnPositionArray.Count)];

                Enemy enemyObj = (Enemy)ObjectPoolManager.Instance.Release(enemy, cellPos, Quaternion.identity);
                // 현재까지 생성된 적의 수와 던전레벨,디테일 스크립터블 옵젝을 넘겨 Enemy 초기화
                enemyObj.GetComponent<Enemy>().EnemyInitialization(enemyDetails, GameManager.instance.GetCurrentDungeonLevelSO());

                yield return new WaitForSeconds(1.0f); // 스폰파라미터에서 스폰간격 구해오기
            }
        }
    }
}

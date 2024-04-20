
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    private Player player;
    private int enemiesToSpawn;
    private int currentEnemyCount;
    private int enemiesSpawnedSoFar;
    private int enemyMaxConcurrentSpawnNumber;
    private Room currentRoom;
    //private RoomEnemySpawnParameters spawnParameters;

    [SerializeField] private GameObject enemyPrefab; // 스폰하기 위한 오리지널 프리팹
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Database Database; // 보스 드랍템

    private RandomSpawnableObject<EnemyDetailsSO> enemySpawnable;
    private List<int> randomEnemy;
    private RandomSpawnableObject<InventoryItemData> itemSpawnable;
    private List<int> randomItem;
    private int randomItemIdx = 0;


    private void OnEnable()
    {
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;
        StaticEventHandler.OnRoomTimeout += StaticEventHandler_OnRoomTimeout;
    }
    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;
        StaticEventHandler.OnRoomTimeout -= StaticEventHandler_OnRoomTimeout;
    }

    private void StaticEventHandler_OnRoomTimeout(RoomTimeoutArgs obj)
    {
        StopAllCoroutines(); // 타임아웃 호출시 코루틴 올스탑 (스폰 중지)
    }

    private void StaticEventHandler_OnRoomChanged(RoomChangedArgs args)
    {
        currentRoom = args.room;
        if (args.room.isEntrance) return; // 변경된 방이 입구라면 그대로 리턴

        player = GameManager.Instance.GetPlayer(); // 플레이어 받아오기

        SetRandomNumberList(); // 스폰할 랜덤오브젝트 미리 정해두기         
        StartCoroutine(SpawnEnemiesRoutine()); // 위에서 생성한 랜덤숫자대로 스폰 (스폰될게 미리 정해진 상태)

        if (args.room.isBossRoom) SpawnBoss(args.room); // 보스방이라면 보스 생성
    }

    private void SetRandomNumberList()
    {
        // 생성된 던전에서 스폰될 적,아이템 리스트를 받아와 생성자의 매개변수로 넣어줌

        enemySpawnable = new RandomSpawnableObject<EnemyDetailsSO>(currentRoom.spawnableEnemyList);
        randomEnemy = new List<int>(100);
        for (int i = 0; i < 100; ++i)
        {
            randomEnemy.Add(Random.Range(0, enemySpawnable.ratioValueTotal)); // 100개의 랜덤숫자 생성
        }

        itemSpawnable = new RandomSpawnableObject<InventoryItemData>(currentRoom.spawnableItemList);
        randomItem = new List<int>(100);
        for (int i = 0; i < 100; ++i)
        {
            randomItem.Add(Random.Range(0, itemSpawnable.ratioValueTotal)); // 100개의 랜덤숫자 생성
        }
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        for (int i = 0; i < 40; i++) // 총 스폰횟수까지 반복
        {
            // 그리드 기준 스폰가능 배열에서 랜덤으로 가져와 스폰지점 만들기
            Vector3Int cellPos = (Vector3Int)currentRoom.spawnPositionArray[Random.Range(0, currentRoom.spawnPositionArray.Count)];

            Enemy enemyObj = (Enemy)ObjectPoolManager.Instance.Release(enemyPrefab, cellPos, Quaternion.identity);

            // 랜덤으로 뽑은 숫자 100개 중에서 한개를 골라 해당 범위의 적 생성
            enemyObj.GetComponent<Enemy>().EnemyInitialization(enemySpawnable.GetItem(randomEnemy[i%100]), GameManager.Instance.GetCurrentDungeonLevelSO());

            // 적의 파괴 이벤트 구독
            enemyObj.GetComponent<DestroyedEvent>().OnDestroyed += Enemy_OnDestroyed;

            yield return new WaitForSeconds(2f); // 스폰파라미터에서 스폰간격 구해오기
        }
    }

    private void Enemy_OnDestroyed(DestroyedEvent arg1, DestroyedEventArgs args)
    {                                                                                 // 파괴된 위치
        ItemPickUp itemObj = (ItemPickUp)ObjectPoolManager.Instance.Release(itemPrefab, args.point, Quaternion.identity);
        
        // 랜덤으로 뽑은 숫자 100개 중에서 한개를 골라 해당 범위의 아이템 생성
        itemObj.GetComponent<ItemPickUp>().InitializeItem(itemSpawnable.GetItem(randomItem[randomItemIdx++%100]));
    }


    // ==================== 보스방일 경우 보스 생성 함수 ===========================
    private void SpawnBoss(Room room)
    {
        // 방이 가진 보스리스트의 보스 생성
        foreach (var spawnableBoss in room.spawnableBossList)
        {
            // 그리드 기준 스폰가능 배열에서 랜덤으로 가져와 스폰지점 만들기
            Vector3Int cellPos = (Vector3Int)currentRoom.spawnPositionArray[Random.Range(0, currentRoom.spawnPositionArray.Count)];

            GameObject enemyBoss = Instantiate(enemyPrefab, cellPos, Quaternion.identity);
            enemyBoss.GetComponent<Enemy>().EnemyInitialization(spawnableBoss, GameManager.Instance.GetCurrentDungeonLevelSO());

            enemyBoss.GetComponent<DestroyedEvent>().OnDestroyed += Boss_Destroyed;
        }

    }

    private void Boss_Destroyed(DestroyedEvent arg1, DestroyedEventArgs args)
    {
        int randomItem = Random.Range(1000, 1028);

        ItemPickUp itemObj = (ItemPickUp)ObjectPoolManager.Instance.Release(itemPrefab, args.point, Quaternion.identity);
        
        itemObj.GetComponent<ItemPickUp>().InitializeItem(Database.GetItem(randomItem));
    }
}

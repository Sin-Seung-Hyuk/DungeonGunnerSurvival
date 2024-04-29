
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    private float spawnTimer;
    private float waveTimer;
    private Room currentRoom;
    private SpawnParameter spawnParameters;

    [SerializeField] private GameObject enemyPrefab; // 스폰하기 위한 오리지널 프리팹
    [SerializeField] private GameObject itemPrefab;

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
        MusicManager.Instance.PlayMusic(GameResources.Instance.ambientMusic, 0.2f, 2f); // 방 진입 음악재생

        currentRoom = args.room;
        if (args.room.isEntrance) return; // 변경된 방이 입구라면 그대로 리턴

        // 전투 음악재생
        MusicManager.Instance.PlayMusic(GameResources.Instance.battleMusic, 0.2f, 0.5f);

        spawnParameters = args.room.spawnParameter; // 방의 스폰파라미터 받아오기

        SetRandomNumberList(); // 스폰할 랜덤오브젝트 미리 정해두기         
        StartCoroutine(SpawnEnemiesRoutine()); // 위에서 생성한 랜덤숫자대로 스폰 (스폰될게 미리 정해진 상태)
        StartCoroutine(SpawnParameterRoutine()); // 웨이브마다 스폰간격 변경 코루틴

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


    private IEnumerator SpawnParameterRoutine()
    {
        spawnTimer = spawnParameters.spawnDistance;

        // 웨이브 간격마다 웨이브 지속시간 동안 스폰간격 설정, 웨이브 끝나면 스폰간격 복구
        while (true)
        {
            yield return new WaitForSeconds(spawnParameters.waveDistance); // 웨이브 간격동안 대기

            spawnTimer = spawnParameters.spawnDistanceInWave; // 웨이브 시작 (스폰간격 짧아짐)

            yield return new WaitForSeconds(spawnParameters.waveDuration); // 웨이브 지속시간

            spawnTimer = spawnParameters.spawnDistance; // 웨이브 종료 (스폰간격 길어짐)
        }
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        int i = 0;
        while (true) // 계속 반복
        {
            i++;

            // 그리드 기준 스폰가능 배열에서 랜덤으로 가져와 스폰지점 만들기 (플레이어 스폰좌표만큼 맵이 이동되어있으므로 더해줘야함)
            Vector3Int cellPos = (Vector3Int)currentRoom.spawnPositionArray[Random.Range(0, currentRoom.spawnPositionArray.Count)] + currentRoom.playerSpawnPos;

            Enemy enemyObj = (Enemy)ObjectPoolManager.Instance.Release(enemyPrefab, cellPos, Quaternion.identity);

            // 랜덤으로 뽑은 숫자 100개 중에서 한개를 골라 해당 범위의 적 생성
            enemyObj.GetComponent<Enemy>().EnemyInitialization(enemySpawnable.GetItem(randomEnemy[i%100]), GameManager.Instance.GetCurrentDungeonLevelSO());

            // 적의 파괴 이벤트 구독
            enemyObj.GetComponent<DestroyedEvent>().OnDestroyed += Enemy_OnDestroyed;

            yield return new WaitForSeconds(spawnTimer); // 스폰파라미터에서 스폰간격 구해오기
        }
    }

    private void Enemy_OnDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs args)
    {
        // 적의 파괴 이벤트 구독해지
        destroyedEvent.OnDestroyed -= Enemy_OnDestroyed;

        // 파괴된 위치
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
        
                                                        // 데이터베이스에서 아이템 랜덤으로 하나 가져오기
        itemObj.GetComponent<ItemPickUp>().InitializeItem(GameResources.Instance.database.GetItem(randomItem));
    }
}

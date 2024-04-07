
using System;
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
        Vector3 pos = new Vector3(args.room.spawnPositionArray[0].x, args.room.spawnPositionArray[0].y, 0);
        // 마지막 transform 매개변수는 부모 오브젝트
        GameObject enemy = Instantiate(enemyDetails.enemyPrefab, pos, Quaternion.identity, transform);

        // 현재까지 생성된 적의 수와 던전레벨,디테일 스크립터블 옵젝을 넘겨 Enemy 초기화
        enemy.GetComponent<Enemy>().EnemyInitialization(enemyDetails, GameManager.instance.GetCurrentDungeonLevelSO());
    }
}

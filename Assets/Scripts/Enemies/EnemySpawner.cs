
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
        // ������ transform �Ű������� �θ� ������Ʈ
        GameObject enemy = Instantiate(enemyDetails.enemyPrefab, pos, Quaternion.identity, transform);

        // ������� ������ ���� ���� ��������,������ ��ũ���ͺ� ������ �Ѱ� Enemy �ʱ�ȭ
        enemy.GetComponent<Enemy>().EnemyInitialization(enemyDetails, GameManager.instance.GetCurrentDungeonLevelSO());
    }
}

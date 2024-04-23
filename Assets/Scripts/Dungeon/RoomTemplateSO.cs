using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Room_", menuName = "Scriptable Objects/Dungeon/Room")]
public class RoomTemplateSO : ScriptableObject
{
    public GameObject roomPrefab;

    public string roomName;

    public bool isEntrance;
    public bool isBossRoom;

    public Vector2Int lowerBounds; // 좌측 하단 좌표
    public Vector2Int upperBounds; // 우측 상단 좌표

    public Vector3Int playerSpawnPos;
    public List<Vector2Int> spawnPositionArray; // 적 스폰 좌표

    public List<SpawnableObjectRatio<EnemyDetailsSO>> spawnableEnemyList; // 스폰될 적 종류
    public List<SpawnableObjectRatio<InventoryItemData>> spawnableItemList; // 스폰될 아이템 종류
    public SpawnParameter spawnParameter; // 스폰될 아이템 종류

    public List<EnemyDetailsSO> spawnableBossList; // 보스방일 경우 스폰될 보스 리스트
}

[System.Serializable] 
public class SpawnParameter
{
    public float waveDistance; // 웨이브 간격
    public float waveDuration; // 웨이브 지속시간
    public float spawnDistanceInWave; // 웨이브 스폰간격 
    public float spawnDistance; // 평상시 스폰간격
}
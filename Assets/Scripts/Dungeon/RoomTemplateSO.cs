using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Room_", menuName = "Scriptable Objects/Dungeon/Room")]
public class RoomTemplateSO : ScriptableObject
{
    public GameObject roomPrefab;

    public bool isEntrance;

    public Vector2Int lowerBounds; // 좌측 하단 좌표
    public Vector2Int upperBounds; // 우측 상단 좌표

    public Vector3Int playerSpawnPos;
    public List<Vector2Int> spawnPositionArray; // 적 스폰 좌표

    public List<SpawnableObjectRatio<EnemyDetailsSO>> spawnableEnemyList; // 스폰될 적 종류
    public List<SpawnableObjectRatio<InventoryItemData>> spawnableItemList; // 스폰될 아이템 종류


}
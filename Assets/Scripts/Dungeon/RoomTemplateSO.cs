using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Room_", menuName = "Scriptable Objects/Dungeon/Room")]
public class RoomTemplateSO : ScriptableObject
{
    public GameObject roomPrefab;

    public bool isEntrance;

    public Vector2Int lowerBounds; // ���� �ϴ� ��ǥ
    public Vector2Int upperBounds; // ���� ��� ��ǥ

    public Vector3Int playerSpawnPos;
    public List<Vector2Int> spawnPositionArray; // �� ���� ��ǥ

    public List<SpawnableObjectRatio<EnemyDetailsSO>> spawnableEnemyList; // ������ �� ����
    public List<SpawnableObjectRatio<InventoryItemData>> spawnableItemList; // ������ ������ ����


}
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

    public Vector2Int lowerBounds; // ���� �ϴ� ��ǥ
    public Vector2Int upperBounds; // ���� ��� ��ǥ

    public Vector3Int playerSpawnPos;
    public List<Vector2Int> spawnPositionArray; // �� ���� ��ǥ

    public List<SpawnableObjectRatio<EnemyDetailsSO>> spawnableEnemyList; // ������ �� ����
    public List<SpawnableObjectRatio<InventoryItemData>> spawnableItemList; // ������ ������ ����
    public SpawnParameter spawnParameter; // ������ ������ ����

    public List<EnemyDetailsSO> spawnableBossList; // �������� ��� ������ ���� ����Ʈ
}

[System.Serializable] 
public class SpawnParameter
{
    public float waveDistance; // ���̺� ����
    public float waveDuration; // ���̺� ���ӽð�
    public float spawnDistanceInWave; // ���̺� �������� 
    public float spawnDistance; // ���� ��������
}
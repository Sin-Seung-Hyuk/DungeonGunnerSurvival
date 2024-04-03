using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [HideInInspector] public Grid grid; // �濡 ���Ե� �׸���

    [HideInInspector] public bool isEntrance; // �Ա����� �Ǻ�

    [HideInInspector] public Vector2Int lowerBounds;
    [HideInInspector] public Vector2Int upperBounds;

    [HideInInspector] public Vector3Int playerSpawnPos;
    [HideInInspector] public List<Vector2Int> spawnPositionArray; // �� ���� ��ǥ

    public void InitializedRoom(RoomTemplateSO roomTemplate)
    {
        grid = GetComponentInChildren<Grid>();

        isEntrance = roomTemplate.isEntrance;
        lowerBounds = roomTemplate.lowerBounds;
        upperBounds = roomTemplate.upperBounds;
        playerSpawnPos = roomTemplate.playerSpawnPos;
        spawnPositionArray = roomTemplate.spawnPositionArray;
    }
}

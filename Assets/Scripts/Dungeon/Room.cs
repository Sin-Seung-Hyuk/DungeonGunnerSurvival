using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [HideInInspector] public Grid grid; // 방에 포함된 그리드

    [HideInInspector] public bool isEntrance; // 입구인지 판별

    [HideInInspector] public Vector2Int lowerBounds;
    [HideInInspector] public Vector2Int upperBounds;

    [HideInInspector] public Vector3Int playerSpawnPos;
    [HideInInspector] public List<Vector2Int> spawnPositionArray; // 적 스폰 좌표

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    [HideInInspector] public Grid grid; // 방에 포함된 그리드
    [HideInInspector] public Tilemap groundTilemap;
    [HideInInspector] public Tilemap decoration1Tilemap;
    [HideInInspector] public Tilemap decoration2Tilemap;
    [HideInInspector] public Tilemap frontTilemap;
    [HideInInspector] public Tilemap collisionTilemap;
    [HideInInspector] public Tilemap minimapTilemap; // 방에 포함된 타일맵들 (그리드 자식오브젝트)

    [HideInInspector] public bool isEntrance; // 입구인지 판별

    [HideInInspector] public Vector2Int lowerBounds; // 방의 좌하단 (3사분면)
    [HideInInspector] public Vector2Int upperBounds; // 방의 우상단 (1사분면)

    [HideInInspector] public Vector3Int playerSpawnPos;
    [HideInInspector] public List<Vector2Int> spawnPositionArray; // 적 스폰 좌표

    [HideInInspector] public int[,] aStarMovementPenalty; // 벽은 G값이 낮음
    [HideInInspector] public int[,] aStarItemObstacles; // 움직이는 장애물 위치 저장


    public void InitializedRoom(RoomTemplateSO roomTemplate, GameObject roomObject)
    {
        grid = GetComponentInChildren<Grid>();

        isEntrance = roomTemplate.isEntrance;
        lowerBounds = roomTemplate.lowerBounds;
        upperBounds = roomTemplate.upperBounds;
        playerSpawnPos = roomTemplate.playerSpawnPos;
        spawnPositionArray = roomTemplate.spawnPositionArray;

        PopulateTilemapMemberVariables(roomObject); // 타일맵 초기화
        AddObstaclesAndPreferredPaths(); // A* 그리드별 G값 초기화
    }

    private void PopulateTilemapMemberVariables(GameObject roomGameobject)
    {
        // 방의 자식오브젝트로 있는 타일맵 배열로 가져오기
        Tilemap[] tilemaps = roomGameobject.GetComponentsInChildren<Tilemap>();

        foreach (Tilemap tilemap in tilemaps)   // 각 타일맵 태그별로 가져오기
        {
            if (tilemap.gameObject.tag == "groundTilemap")
            {
                groundTilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == "decoration1Tilemap")
            {
                decoration1Tilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == "decoration2Tilemap")
            {
                decoration2Tilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == "frontTilemap")
            {
                frontTilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == "collisionTilemap")
            {
                collisionTilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == "minimapTilemap")
            {
                minimapTilemap = tilemap;
            }
        }
    }

    private void AddObstaclesAndPreferredPaths()
    {
        // 방의 크기만큼 A* 배열 초기화
        aStarMovementPenalty = new int[upperBounds.x - lowerBounds.x + 1, upperBounds.y - lowerBounds.y + 1];

        // 방의 모든 그리드 노드를 순회
        for (int x = 0; x < (upperBounds.x - lowerBounds.x + 1); x++)
        {
            for (int y = 0; y < (upperBounds.y - lowerBounds.y + 1); y++)
            {
                // 각 노드별 기본 G값 40
                aStarMovementPenalty[x, y] = 40;

                // 콜리젼 타일맵에서 타일을 GetTile로 가져옴 (콜리젼이 있는 타일은 못가는 타일이므로)
                TileBase tile = collisionTilemap.GetTile(new Vector3Int(x + lowerBounds.x, y + lowerBounds.y, 0));

                foreach (TileBase collisionTile in GameResources.Instance.enemyUnwalkableTilesArray)
                {
                    if (tile == collisionTile)
                    {
                        aStarMovementPenalty[x, y] = 0; // 콜리젼타일은 g값 0으로 (이동불가)
                        break;
                    }
                }

                // 적이 선호하는 경로(초록색 타일)이면 해당 그리드의 g값을 1로 설정
                if (tile == GameResources.Instance.preferredEnemyPathTile)
                    aStarMovementPenalty[x, y] = Settings.preferredPathAStarMovementPenalty;
            }

        }
    }

 }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    [HideInInspector] public Grid grid; // �濡 ���Ե� �׸���
    [HideInInspector] public Tilemap groundTilemap;
    [HideInInspector] public Tilemap decoration1Tilemap;
    [HideInInspector] public Tilemap decoration2Tilemap;
    [HideInInspector] public Tilemap frontTilemap;
    [HideInInspector] public Tilemap collisionTilemap;
    [HideInInspector] public Tilemap minimapTilemap; // �濡 ���Ե� Ÿ�ϸʵ� (�׸��� �ڽĿ�����Ʈ)

    [HideInInspector] public bool isEntrance; // �Ա����� �Ǻ�

    [HideInInspector] public Vector2Int lowerBounds; // ���� ���ϴ� (3��и�)
    [HideInInspector] public Vector2Int upperBounds; // ���� ���� (1��и�)

    [HideInInspector] public Vector3Int playerSpawnPos;
    [HideInInspector] public List<Vector2Int> spawnPositionArray; // �� ���� ��ǥ

    [HideInInspector] public int[,] aStarMovementPenalty; // ���� G���� ����
    [HideInInspector] public int[,] aStarItemObstacles; // �����̴� ��ֹ� ��ġ ����


    public void InitializedRoom(RoomTemplateSO roomTemplate, GameObject roomObject)
    {
        grid = GetComponentInChildren<Grid>();

        isEntrance = roomTemplate.isEntrance;
        lowerBounds = roomTemplate.lowerBounds;
        upperBounds = roomTemplate.upperBounds;
        playerSpawnPos = roomTemplate.playerSpawnPos;
        spawnPositionArray = roomTemplate.spawnPositionArray;

        PopulateTilemapMemberVariables(roomObject); // Ÿ�ϸ� �ʱ�ȭ
        AddObstaclesAndPreferredPaths(); // A* �׸��庰 G�� �ʱ�ȭ
    }

    private void PopulateTilemapMemberVariables(GameObject roomGameobject)
    {
        // ���� �ڽĿ�����Ʈ�� �ִ� Ÿ�ϸ� �迭�� ��������
        Tilemap[] tilemaps = roomGameobject.GetComponentsInChildren<Tilemap>();

        foreach (Tilemap tilemap in tilemaps)   // �� Ÿ�ϸ� �±׺��� ��������
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
        // ���� ũ�⸸ŭ A* �迭 �ʱ�ȭ
        aStarMovementPenalty = new int[upperBounds.x - lowerBounds.x + 1, upperBounds.y - lowerBounds.y + 1];

        // ���� ��� �׸��� ��带 ��ȸ
        for (int x = 0; x < (upperBounds.x - lowerBounds.x + 1); x++)
        {
            for (int y = 0; y < (upperBounds.y - lowerBounds.y + 1); y++)
            {
                // �� ��庰 �⺻ G�� 40
                aStarMovementPenalty[x, y] = 40;

                // �ݸ��� Ÿ�ϸʿ��� Ÿ���� GetTile�� ������ (�ݸ����� �ִ� Ÿ���� ������ Ÿ���̹Ƿ�)
                TileBase tile = collisionTilemap.GetTile(new Vector3Int(x + lowerBounds.x, y + lowerBounds.y, 0));

                foreach (TileBase collisionTile in GameResources.Instance.enemyUnwalkableTilesArray)
                {
                    if (tile == collisionTile)
                    {
                        aStarMovementPenalty[x, y] = 0; // �ݸ���Ÿ���� g�� 0���� (�̵��Ұ�)
                        break;
                    }
                }

                // ���� ��ȣ�ϴ� ���(�ʷϻ� Ÿ��)�̸� �ش� �׸����� g���� 1�� ����
                if (tile == GameResources.Instance.preferredEnemyPathTile)
                    aStarMovementPenalty[x, y] = Settings.preferredPathAStarMovementPenalty;
            }

        }
    }

 }

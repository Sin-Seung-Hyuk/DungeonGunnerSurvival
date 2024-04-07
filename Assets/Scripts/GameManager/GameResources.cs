using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Audio;

public class GameResources : MonoBehaviour
{
    private static GameResources instance;

    public static GameResources Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<GameResources>("GameResources");
            }
            return instance;
        }
    }
    // ===================== ����� �ڿ��� ==============================

    [Space(10)]
    [Header("Player")]
    public CurrentPlayerSO currentPlayer;

    [Space(10)]
    [Header("Tilemap Tiles for AStar")]
    public TileBase[] enemyUnwalkableTilesArray; // ���� ������ Ÿ�� �迭
    public TileBase preferredEnemyPathTile; // ���� ��ȣ�ϴ� Ÿ��

}

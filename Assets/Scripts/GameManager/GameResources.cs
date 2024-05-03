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
    public GameObject playerSelectionPrefab;
    public List<PlayerDetailsSO> playerDetailsList;
    public List<WeaponDetailsSO> weaponList;
    public GameObject potionStateImage;

    [Space(10)]
    [Header("Music")]
    public AudioMixerGroup musicMasterMixerGroup;
    public AudioMixerSnapshot musicOnFullSnapshot;
    public AudioMixerSnapshot musicLowSnapshot;
    public AudioMixerSnapshot musicOffSnapshot;
    public MusicTrackSO mainMusic;
    public MusicTrackSO battleMusic;
    public MusicTrackSO entranceMusic;

    [Space(10)]
    [Header("Sounds")]
    public AudioMixerGroup soundsMasterMixerGroup;
    public SoundEffectSO chestOpen;
    public SoundEffectSO healthPickUp;
    public SoundEffectSO weaponPickUp;
    public SoundEffectSO dungeonClear;
    public SoundEffectSO cancle;
    public SoundEffectSO gold;

    [Space(10)]
    [Header("Tilemap Tiles for AStar")]
    public TileBase[] enemyUnwalkableTilesArray; // ���� ������ Ÿ�� �迭
    public TileBase preferredEnemyPathTile; // ���� ��ȣ�ϴ� Ÿ��

    [Space(10)]
    [Header("Ammo Hit Effect/Text")]
    public GameObject ammoHitEffect;
    public GameObject ammoHitText;

    [Space(10)]
    [Header("Materials")]
    public Material litMaterial;
    public Shader materializeShader;

    [Header("Chests")]
    public GameObject chestItemPrefab;
    public GameObject chestPrefab;

    [Header("Game Data")]
    public Database database;
}

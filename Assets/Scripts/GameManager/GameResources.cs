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
    // ===================== 등록할 자원들 ==============================

    [Space(10)]
    [Header("Player")]
    public CurrentPlayerSO currentPlayer;
    public GameObject playerSelectionPrefab;
    public List<PlayerDetailsSO> playerDetailsList;

    [Space(10)]
    [Header("Music")]
    public AudioMixerGroup musicMasterMixerGroup;
    public AudioMixerSnapshot musicOnFullSnapshot;
    public AudioMixerSnapshot musicLowSnapshot;
    public AudioMixerSnapshot musicOffSnapshot;
    //public MusicTrackSO mainMusic;

    [Space(10)]
    [Header("Sounds")]
    public AudioMixerGroup soundsMasterMixerGroup;
    public SoundEffectSO chestOpen;
    public SoundEffectSO healthPickUp;
    public SoundEffectSO weaponPickUp;
    public SoundEffectSO ammoPickUp;

    [Space(10)]
    [Header("Tilemap Tiles for AStar")]
    public TileBase[] enemyUnwalkableTilesArray; // 적이 못가는 타일 배열
    public TileBase preferredEnemyPathTile; // 적이 선호하는 타일

    [Space(10)]
    [Header("Ammo Hit Effect/Text")]
    public GameObject ammoHitEffect;
    public GameObject ammoHitText;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private List<DungeonLevelSO> dungeonLevelList; // 던전의 레벨이 들어갈 리스트
    private int currentDungeonLevel = 0; // 플레이어가 입장할 던전의 레벨
    private Room instantiatedRoom; // 지금 생성된 던전의 방

    [SerializeField] private Player player;
    private PlayerDetailsSO playerDetails;

    [HideInInspector] public GameState gameState;
    [HideInInspector] public GameState prevGameState;


    protected override void Awake()
    {
        base.Awake();

        InstantiatePlayer();
    }

    private void OnEnable()
    {
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;
    }
    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;
    }

    void Start()
    {
        // 게임이 시작되면서 입구 생성
        CreateDungeonLevel(currentDungeonLevel++);
        gameState = GameState.InEntrance;
        prevGameState = GameState.InEntrance;
    }

    void Update()
    {
        HandleGameState();
    }


    // ======================= 던전 관리 ========================================
    #region DUNGEON
    public void CreateDungeonLevel(int currentDungeonLevel)
    {
        DungeonBuilder.Instance.CreateDungeonRoom(dungeonLevelList[currentDungeonLevel]);

        player.transform.position = new Vector3Int(0, 0, 0);
    }
    public void InstantiatePlayer()
    {
        // 게임리소스에 등록된 선택된 캐릭터 정보 받아와 초기화
        playerDetails = GameResources.Instance.currentPlayer.currentPlayerDetailsSO;

        //GameObject playerObject = Instantiate(playerDetails.playerPrefab);
        //player = playerObject.GetComponent<Player>();

        player.InitializePlayer(playerDetails);
    }

    private IEnumerator DungeonRoomCleared() // 방 하나 클리어 
    {
        gameState = GameState.InDungeon;

        while (!Input.GetKeyDown(KeyCode.Return))
        {   // 리턴 키를 입력할때까지 반복
            yield return null;
        }

        yield return null;

        // 던전의 다음 방 플레이
        CreateDungeonLevel(currentDungeonLevel);
    }

    private IEnumerator DungeonCompleted() // 해당 레벨의 모든 방 클리어
    {
        gameState = GameState.InDungeon;

        while (!Input.GetKeyDown(KeyCode.Return))
        {   // 리턴 키를 입력할때까지 반복
            yield return null;
        }

        yield return null;

        // 던전 레벨 증가시켜 다음레벨 던전 플레이
        currentDungeonLevel++;
        CreateDungeonLevel(currentDungeonLevel);
    }

    private IEnumerator BackToEntrance() // 던전입구로 돌아가기
    {
        gameState = GameState.InEntrance;

        while (!Input.GetKeyDown(KeyCode.Return))
        {   // 리턴 키를 입력할때까지 반복
            yield return null;
        }

        yield return null;

        CreateDungeonLevel(0);
    }
    #endregion



    // ======================= 게임상태 관리 ========================================
    #region GAME STATE
    private void HandleGameState()
    {
        switch (gameState)
        {
            case GameState.InEntrance:
                // 사격 이벤트 호출 X 
                break;

            case GameState.InDungeon:
                StartCoroutine(BackToEntrance());
                break;

            case GameState.DungeonRoomClear: // 타이머가 끝나면 이벤트 호출 (게임상태 변화)
                // 같은 레벨의 던전 내에서 다음 방으로 이동 (코루틴,엔터입력까지,캐릭터 움직임 막기)
                StartCoroutine(DungeonRoomCleared());
                break;

            case GameState.DungeonCompleted: // 보스를 처치하면 레벨완료 이벤트 호출 (상자생성+게임상태 변화)
                // 입구로 이동 후 던전레벨 증가 (코루틴,엔터입력까지)
                StartCoroutine(DungeonCompleted());
                break;

            case GameState.GameCompleted:
                break;

            case GameState.Paused:
                break;

            default:
                break;
        }
    }

    private void StaticEventHandler_OnRoomChanged(RoomChangedArgs args)
    {
        instantiatedRoom = args.room;

        if (args.room.isEntrance) // 변경된 방 입구
        {
            prevGameState = gameState;
            gameState = GameState.InEntrance;
            player.fireWeapon.enabled = false;
            player.health.AddHealth(100); // 입구로 돌아오면 체력 100% 회복 (한번만)
        } else
        {
            prevGameState = gameState;
            gameState = GameState.InDungeon; // 입구 아니면 던전밖에 없음
            player.fireWeapon.enabled = true;
        }
    }
    #endregion



    // =================== 유틸 ============================

    public Player GetPlayer()
    {
        return player;
    }
    public Vector3 GetPlayerPosition()
    {
        return player.transform.position;
    }
    public Vector3Int GetPlayerCellPosition()
    {
        return instantiatedRoom.grid.WorldToCell(player.transform.position);
    }
    public Sprite GetPlayerMinimapIcon()
    {
        return playerDetails.minimapIcon;
    }
    public int GetCurrentDungeonLevel()
    {
        return currentDungeonLevel;
    }
    public DungeonLevelSO GetCurrentDungeonLevelSO()
    {
        return dungeonLevelList[currentDungeonLevel];
    }
    public Room GetCurrentRoom()
    {
        return instantiatedRoom;
    }

}

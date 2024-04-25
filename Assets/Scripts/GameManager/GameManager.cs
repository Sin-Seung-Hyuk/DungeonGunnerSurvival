using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private List<DungeonLevelSO> dungeonLevelList; // 던전의 레벨이 들어갈 리스트
    private int currentDungeonLevel = 0; // 플레이어가 입장할 던전의 레벨
    private Room instantiatedRoom; // 지금 생성된 던전의 방
    [SerializeField] private ChestSpawner chestSpawner; // 입구에 배치된 상자스포너 (보상상자)

    [SerializeField] private Player player;
    private PlayerDetailsSO playerDetails;

    [SerializeField] private TextMeshProUGUI TxtFade; // 페이드 텍스트
    [SerializeField] private Image bossRoomImage; // 페이드 텍스트
    [SerializeField] private CanvasGroup canvasGroup;    // 페이드 이미지

    [SerializeField] private CanvasGroup gameOverUI;    // 게임실패,클리어시 나오는 UI

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
        StaticEventHandler.OnRoomTimeout += StaticEventHandler_OnRoomTimeout;

        player.destroyedEvent.OnDestroyed += Player_OnDestroyed;
    }
    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;
        StaticEventHandler.OnRoomTimeout -= StaticEventHandler_OnRoomTimeout;

        player.destroyedEvent.OnDestroyed -= Player_OnDestroyed;
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


    // ======================= 던전(씬) 관리 ========================================
    #region DUNGEON
    public void CreateDungeonLevel(int currentDungeonLevel)
    {
        DungeonBuilder.Instance.CreateDungeonRoom(dungeonLevelList[currentDungeonLevel]);

        player.transform.position = new Vector3Int(0, 0, 0);

        CreateDungeonFade();
    }

    public void InstantiatePlayer()
    {
        // 게임리소스에 등록된 선택된 캐릭터 정보 받아와 초기화
        playerDetails = GameResources.Instance.currentPlayer.currentPlayerDetailsSO;

        player.InitializePlayer(playerDetails);
    }

    private IEnumerator DungeonRoomCleared() // 방 하나 클리어 
    {
        gameState = GameState.InDungeon;

        yield return new WaitForSeconds(2f);
        StartCoroutine(Fade(0.75f, 2f));

        TxtFade.text = "PRESS 'ENTER' TO NEXT DUNGEON!";

        while (!Input.GetKeyDown(KeyCode.Return))
        {   // 리턴 키를 입력할때까지 반복
            yield return null;
        }

        yield return null;

        // 던전의 다음 방 플레이
        CreateDungeonLevel(currentDungeonLevel);
    }

    private IEnumerator DungeonLevelCompleted() // 해당 레벨의 모든 방 클리어
    {
        gameState = GameState.InDungeon;

        yield return new WaitForSeconds(2f);
        StartCoroutine(Fade(0.75f, 2f));

        TxtFade.text = "LEVEL CLEAR!\nPRESS 'ENTER' TO ENTRANCE!";

        while (!Input.GetKeyDown(KeyCode.Return))
        {   // 리턴 키를 입력할때까지 반복
            yield return null;
        }

        yield return null;

        // 던전 레벨 증가시켜 다음레벨 던전 플레이
        currentDungeonLevel++;
        CreateDungeonLevel(0); // 입구로 복귀

        chestSpawner.SpawnChest(); // 보상상자 스폰
    }

    private IEnumerator GameLost() // 플레이어 던전에서 사망
    {
        prevGameState = GameState.GameLost;
        gameState = GameState.InDungeon;

        TxtFade.text = "";
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(Fade(0.75f, 2f));

        gameOverUI.gameObject.SetActive(true);

        while (!Input.GetKeyDown(KeyCode.Return))
        {   // 리턴 키를 입력할때까지 반복
            yield return null;
        }

        yield return null;

        player.playerInventory.ClearPlayerInventory();
        gameState = GameState.RestartGame;
    }

    private void CreateDungeonFade() // 던전입장시 까만화면에서 점점 밝아짐 (페이드인)
    {
        canvasGroup.alpha = 1f;
        TxtFade.text = instantiatedRoom.roomTemplate.roomName;
        if (instantiatedRoom.isBossRoom) bossRoomImage.gameObject.SetActive(true);
        else bossRoomImage.gameObject.SetActive(false);
        StartCoroutine(Fade(0, 3f));
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
                player.fireWeapon.enabled = false;
                player.health.AddHealth(100); // 입구로 돌아오면 체력 100% 회복 (한번만)
                break;

            case GameState.InDungeon:
                player.fireWeapon.enabled = true;
                break;

            case GameState.DungeonRoomClear: // 타이머가 끝나면 이벤트 호출 (게임상태 변화)
                // 같은 레벨의 던전 내에서 다음 방으로 이동 (코루틴,엔터입력까지,캐릭터 움직임 막기)
                StartCoroutine(DungeonRoomCleared());
                break;

            case GameState.DungeonCompleted: // 보스를 처치하면 레벨완료 이벤트 호출 (상자생성+게임상태 변화)
                // 입구로 이동 후 던전레벨 증가 (코루틴,엔터입력까지)
                StartCoroutine(DungeonLevelCompleted());
                break;

            case GameState.GameCompleted:
                break;

            case GameState.GameLost:
                StartCoroutine(GameLost());
                break;

            case GameState.Paused:
                break;

            case GameState.RestartGame:
                SceneManager.LoadScene("MainMenuScene");
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
        } else
        {
            prevGameState = gameState;
            gameState = GameState.InDungeon; // 입구 아니면 던전밖에 없음
        }

        player.ctrl.EnablePlayer();
    }

    private void StaticEventHandler_OnRoomTimeout(RoomTimeoutArgs args)
    {
        if (prevGameState == GameState.GameLost) return; // 게임 실패한 상태이면 시간이 지나도 클리어 X

        player.ctrl.DisablePlayer(); // 타임아웃 이벤트 호출되면 플레이어 이동 불가

        prevGameState = gameState;

        if (args.room.isBossRoom) // 보스방 클리어인지 확인
            gameState = GameState.DungeonCompleted;
        else gameState = GameState.DungeonRoomClear;
    }

    private void Player_OnDestroyed(DestroyedEvent arg1, DestroyedEventArgs arg2)
    {
        prevGameState = gameState;
        gameState = GameState.GameLost;
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

    private IEnumerator Fade(float goal, float time)
    {
        // 캔버스 그룹의 알파값을 time에 걸쳐서 goal만큼 보간 (캔버스 그룹이므로 자식도 같이적용)
        var tween = DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x,
            goal, time).SetEase(Ease.InQuart); // 천천히 증가하다 가파르게 증가함

        yield return tween.WaitForCompletion();
    }
}

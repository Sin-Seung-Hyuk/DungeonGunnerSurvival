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
    [SerializeField] private List<DungeonLevelSO> dungeonLevelList; // ������ ������ �� ����Ʈ
    private int currentDungeonLevel = 0; // �÷��̾ ������ ������ ����
    private Room instantiatedRoom; // ���� ������ ������ ��
    [SerializeField] private ChestSpawner chestSpawner; // �Ա��� ��ġ�� ���ڽ����� (�������)

    [SerializeField] private Player player;
    private PlayerDetailsSO playerDetails;

    [SerializeField] private TextMeshProUGUI TxtFade; // ���̵� �ؽ�Ʈ
    [SerializeField] private Image bossRoomImage; // ���̵� �ؽ�Ʈ
    [SerializeField] private CanvasGroup canvasGroup;    // ���̵� �̹���

    [SerializeField] private CanvasGroup gameOverUI;    // ���ӽ���,Ŭ����� ������ UI

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
        // ������ ���۵Ǹ鼭 �Ա� ����
        CreateDungeonLevel(currentDungeonLevel++);
        gameState = GameState.InEntrance;
        prevGameState = GameState.InEntrance;
    }

    void Update()
    {
        HandleGameState();
    }


    // ======================= ����(��) ���� ========================================
    #region DUNGEON
    public void CreateDungeonLevel(int currentDungeonLevel)
    {
        DungeonBuilder.Instance.CreateDungeonRoom(dungeonLevelList[currentDungeonLevel]);

        player.transform.position = new Vector3Int(0, 0, 0);

        CreateDungeonFade();
    }

    public void InstantiatePlayer()
    {
        // ���Ӹ��ҽ��� ��ϵ� ���õ� ĳ���� ���� �޾ƿ� �ʱ�ȭ
        playerDetails = GameResources.Instance.currentPlayer.currentPlayerDetailsSO;

        player.InitializePlayer(playerDetails);
    }

    private IEnumerator DungeonRoomCleared() // �� �ϳ� Ŭ���� 
    {
        gameState = GameState.InDungeon;

        yield return new WaitForSeconds(2f);
        StartCoroutine(Fade(0.75f, 2f));

        TxtFade.text = "PRESS 'ENTER' TO NEXT DUNGEON!";

        while (!Input.GetKeyDown(KeyCode.Return))
        {   // ���� Ű�� �Է��Ҷ����� �ݺ�
            yield return null;
        }

        yield return null;

        // ������ ���� �� �÷���
        CreateDungeonLevel(currentDungeonLevel);
    }

    private IEnumerator DungeonLevelCompleted() // �ش� ������ ��� �� Ŭ����
    {
        gameState = GameState.InDungeon;

        yield return new WaitForSeconds(2f);
        StartCoroutine(Fade(0.75f, 2f));

        TxtFade.text = "LEVEL CLEAR!\nPRESS 'ENTER' TO ENTRANCE!";

        while (!Input.GetKeyDown(KeyCode.Return))
        {   // ���� Ű�� �Է��Ҷ����� �ݺ�
            yield return null;
        }

        yield return null;

        // ���� ���� �������� �������� ���� �÷���
        currentDungeonLevel++;
        CreateDungeonLevel(0); // �Ա��� ����

        chestSpawner.SpawnChest(); // ������� ����
    }

    private IEnumerator GameLost() // �÷��̾� �������� ���
    {
        prevGameState = GameState.GameLost;
        gameState = GameState.InDungeon;

        TxtFade.text = "";
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(Fade(0.75f, 2f));

        gameOverUI.gameObject.SetActive(true);

        while (!Input.GetKeyDown(KeyCode.Return))
        {   // ���� Ű�� �Է��Ҷ����� �ݺ�
            yield return null;
        }

        yield return null;

        player.playerInventory.ClearPlayerInventory();
        gameState = GameState.RestartGame;
    }

    private void CreateDungeonFade() // ��������� �ȭ�鿡�� ���� ����� (���̵���)
    {
        canvasGroup.alpha = 1f;
        TxtFade.text = instantiatedRoom.roomTemplate.roomName;
        if (instantiatedRoom.isBossRoom) bossRoomImage.gameObject.SetActive(true);
        else bossRoomImage.gameObject.SetActive(false);
        StartCoroutine(Fade(0, 3f));
    }
    #endregion


    // ======================= ���ӻ��� ���� ========================================
    #region GAME STATE
    private void HandleGameState()
    {
        switch (gameState)
        {
            case GameState.InEntrance:
                // ��� �̺�Ʈ ȣ�� X 
                player.fireWeapon.enabled = false;
                player.health.AddHealth(100); // �Ա��� ���ƿ��� ü�� 100% ȸ�� (�ѹ���)
                break;

            case GameState.InDungeon:
                player.fireWeapon.enabled = true;
                break;

            case GameState.DungeonRoomClear: // Ÿ�̸Ӱ� ������ �̺�Ʈ ȣ�� (���ӻ��� ��ȭ)
                // ���� ������ ���� ������ ���� ������ �̵� (�ڷ�ƾ,�����Է±���,ĳ���� ������ ����)
                StartCoroutine(DungeonRoomCleared());
                break;

            case GameState.DungeonCompleted: // ������ óġ�ϸ� �����Ϸ� �̺�Ʈ ȣ�� (���ڻ���+���ӻ��� ��ȭ)
                // �Ա��� �̵� �� �������� ���� (�ڷ�ƾ,�����Է±���)
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

        if (args.room.isEntrance) // ����� �� �Ա�
        {
            prevGameState = gameState;
            gameState = GameState.InEntrance;
        } else
        {
            prevGameState = gameState;
            gameState = GameState.InDungeon; // �Ա� �ƴϸ� �����ۿ� ����
        }

        player.ctrl.EnablePlayer();
    }

    private void StaticEventHandler_OnRoomTimeout(RoomTimeoutArgs args)
    {
        if (prevGameState == GameState.GameLost) return; // ���� ������ �����̸� �ð��� ������ Ŭ���� X

        player.ctrl.DisablePlayer(); // Ÿ�Ӿƿ� �̺�Ʈ ȣ��Ǹ� �÷��̾� �̵� �Ұ�

        prevGameState = gameState;

        if (args.room.isBossRoom) // ������ Ŭ�������� Ȯ��
            gameState = GameState.DungeonCompleted;
        else gameState = GameState.DungeonRoomClear;
    }

    private void Player_OnDestroyed(DestroyedEvent arg1, DestroyedEventArgs arg2)
    {
        prevGameState = gameState;
        gameState = GameState.GameLost;
    }
    #endregion



    // =================== ��ƿ ============================
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
        // ĵ���� �׷��� ���İ��� time�� ���ļ� goal��ŭ ���� (ĵ���� �׷��̹Ƿ� �ڽĵ� ��������)
        var tween = DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x,
            goal, time).SetEase(Ease.InQuart); // õõ�� �����ϴ� ���ĸ��� ������

        yield return tween.WaitForCompletion();
    }
}

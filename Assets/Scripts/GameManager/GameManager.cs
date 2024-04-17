using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private List<DungeonLevelSO> dungeonLevelList; // ������ ������ �� ����Ʈ
    private int currentDungeonLevel = 0; // �÷��̾ ������ ������ ����
    private Room instantiatedRoom; // ���� ������ ������ ��

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
        // ������ ���۵Ǹ鼭 �Ա� ����
        CreateDungeonLevel(currentDungeonLevel++);
        gameState = GameState.InEntrance;
        prevGameState = GameState.InEntrance;
    }

    void Update()
    {
        HandleGameState();
    }


    // ======================= ���� ���� ========================================
    #region DUNGEON
    public void CreateDungeonLevel(int currentDungeonLevel)
    {
        DungeonBuilder.Instance.CreateDungeonRoom(dungeonLevelList[currentDungeonLevel]);

        player.transform.position = new Vector3Int(0, 0, 0);
    }
    public void InstantiatePlayer()
    {
        // ���Ӹ��ҽ��� ��ϵ� ���õ� ĳ���� ���� �޾ƿ� �ʱ�ȭ
        playerDetails = GameResources.Instance.currentPlayer.currentPlayerDetailsSO;

        //GameObject playerObject = Instantiate(playerDetails.playerPrefab);
        //player = playerObject.GetComponent<Player>();

        player.InitializePlayer(playerDetails);
    }

    private IEnumerator DungeonRoomCleared() // �� �ϳ� Ŭ���� 
    {
        gameState = GameState.InDungeon;

        while (!Input.GetKeyDown(KeyCode.Return))
        {   // ���� Ű�� �Է��Ҷ����� �ݺ�
            yield return null;
        }

        yield return null;

        // ������ ���� �� �÷���
        CreateDungeonLevel(currentDungeonLevel);
    }

    private IEnumerator DungeonCompleted() // �ش� ������ ��� �� Ŭ����
    {
        gameState = GameState.InDungeon;

        while (!Input.GetKeyDown(KeyCode.Return))
        {   // ���� Ű�� �Է��Ҷ����� �ݺ�
            yield return null;
        }

        yield return null;

        // ���� ���� �������� �������� ���� �÷���
        currentDungeonLevel++;
        CreateDungeonLevel(currentDungeonLevel);
    }

    private IEnumerator BackToEntrance() // �����Ա��� ���ư���
    {
        gameState = GameState.InEntrance;

        while (!Input.GetKeyDown(KeyCode.Return))
        {   // ���� Ű�� �Է��Ҷ����� �ݺ�
            yield return null;
        }

        yield return null;

        CreateDungeonLevel(0);
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
                break;

            case GameState.InDungeon:
                StartCoroutine(BackToEntrance());
                break;

            case GameState.DungeonRoomClear: // Ÿ�̸Ӱ� ������ �̺�Ʈ ȣ�� (���ӻ��� ��ȭ)
                // ���� ������ ���� ������ ���� ������ �̵� (�ڷ�ƾ,�����Է±���,ĳ���� ������ ����)
                StartCoroutine(DungeonRoomCleared());
                break;

            case GameState.DungeonCompleted: // ������ óġ�ϸ� �����Ϸ� �̺�Ʈ ȣ�� (���ڻ���+���ӻ��� ��ȭ)
                // �Ա��� �̵� �� �������� ���� (�ڷ�ƾ,�����Է±���)
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

        if (args.room.isEntrance) // ����� �� �Ա�
        {
            prevGameState = gameState;
            gameState = GameState.InEntrance;
            player.fireWeapon.enabled = false;
            player.health.AddHealth(100); // �Ա��� ���ƿ��� ü�� 100% ȸ�� (�ѹ���)
        } else
        {
            prevGameState = gameState;
            gameState = GameState.InDungeon; // �Ա� �ƴϸ� �����ۿ� ����
            player.fireWeapon.enabled = true;
        }
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

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBuilder : Singleton<DungeonBuilder>
{
    // ���Ŀ� ������Ʈ : �������� �� �����ϱ� ���� ���������� ���� �������� ���� ����Ʈ ����
    private GameObject roomObject = null;
    private int currentDungeonLevel = 0;
    private int currentRoomNum = 0;


    protected override void Awake()
    {
        base.Awake();

      
    }

    public void CreateDungeonRoom(DungeonLevelSO dungeonLevel)
    {
        if (roomObject != null) Destroy(roomObject);

        // ���� ������ ����Ǿ����� ���� ������ ������ ���� ���� �� 0�� �� ����
        if (currentDungeonLevel != dungeonLevel.level)
        {
            currentDungeonLevel = dungeonLevel.level;
            currentRoomNum = 0;
        }

        RoomTemplateSO roomTemplate = dungeonLevel.roomTemplateList[currentRoomNum];
        currentRoomNum++;

        roomObject = Instantiate(roomTemplate.roomPrefab, roomTemplate.playerSpawnPos , Quaternion.identity);
        roomObject.transform.SetParent(this.transform,false);

        Room instantiatedRoom = GetComponentInChildren<Room>();
        instantiatedRoom.InitializedRoom(roomTemplate);

        StaticEventHandler.CallRoomChanged(instantiatedRoom);
    }
}

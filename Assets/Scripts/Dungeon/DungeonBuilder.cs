using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBuilder : Singleton<DungeonBuilder>
{
    // 추후에 업데이트 : 랜덤으로 방 생성하기 위해 던전레벨의 방목록 랜덤으로 넣을 리스트 구현
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

        // 던전 레벨이 변경되었으면 현재 생성할 던전의 레벨 변경 후 0번 방 생성
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

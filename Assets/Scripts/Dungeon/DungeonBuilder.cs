using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBuilder : Singleton<DungeonBuilder>
{
    private GameObject roomObject = null;
    private int currentDungeonLevel = 0; // 지금 던전레벨
    private int currentRoomNum = 0;      // 지금 던전의 방


    protected override void Awake()
    {
        base.Awake();

      
    }

    public void CreateDungeonRoom(DungeonLevelSO dungeonLevel) // 던전 생성
    {
        if (roomObject != null) Destroy(roomObject); // 기존에 생성된 방 파괴

        // 던전 레벨이 변경되었으면 현재 생성할 던전의 레벨 변경 후 0번 방 생성
        if (currentDungeonLevel != dungeonLevel.level)
        {
            currentDungeonLevel = dungeonLevel.level;
            currentRoomNum = 0;
        }

        RoomTemplateSO roomTemplate = dungeonLevel.roomTemplateList[currentRoomNum];
        currentRoomNum++;

        roomObject = Instantiate(roomTemplate.roomPrefab, roomTemplate.playerSpawnPos , Quaternion.identity);
        roomObject.transform.SetParent(this.transform,false); // 던전빌더 자식오브젝트로 던전 생성

        Room instantiatedRoom = GetComponentInChildren<Room>();
        instantiatedRoom.InitializedRoom(roomTemplate, roomObject);

        StaticEventHandler.CallRoomChanged(instantiatedRoom); // 방 변경 이벤트 호출
    }
}

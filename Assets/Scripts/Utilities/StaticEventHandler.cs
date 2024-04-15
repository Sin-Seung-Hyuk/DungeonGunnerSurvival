using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class StaticEventHandler
{
    // 던전 방 변경 이벤트 
    public static event Action<RoomChangedArgs> OnRoomChanged;
    public static void CallRoomChanged(Room room)
    {
        OnRoomChanged?.Invoke(new RoomChangedArgs() { room = room });
    }

    // 플레이어 스탯 변경 이벤트 (레벨업UI,장비,포션에서 접근)
    public static event Action<PlayerStatChangedArgs> OnPlayerStatChanged;
    public static void CallPlayerStatChanged(PlayerStatType statType, int value, bool isIncrease)
    {
        OnPlayerStatChanged?.Invoke(new PlayerStatChangedArgs() { 
            statType = statType, 
            value= value , 
            isIncrease = isIncrease 
        });
    }
}

public class RoomChangedArgs : EventArgs
{
    public Room room;
}

public class PlayerStatChangedArgs : EventArgs
{
    public PlayerStatType statType;
    public int value;
    public bool isIncrease; // 증가되는 스탯인지 감소인지
}
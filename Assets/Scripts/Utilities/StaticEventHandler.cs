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


    // 던전 타임아웃 이벤트
    public static event Action<RoomTimeoutArgs> OnRoomTimeout;
    public static void CallRoomTimeoutEvent(Room room)
    {
        OnRoomTimeout?.Invoke(new RoomTimeoutArgs() { room = room });
    }
}

public class RoomChangedArgs : EventArgs
{
    public Room room;
}

public class RoomTimeoutArgs : EventArgs
{
    public Room room;
}
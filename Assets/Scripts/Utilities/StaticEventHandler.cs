using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class StaticEventHandler
{
    // ���� �� ���� �̺�Ʈ 
    public static event Action<RoomChangedArgs> OnRoomChanged;
    public static void CallRoomChanged(Room room)
    {
        OnRoomChanged?.Invoke(new RoomChangedArgs() { room = room });
    }


    // ���� Ÿ�Ӿƿ� �̺�Ʈ
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
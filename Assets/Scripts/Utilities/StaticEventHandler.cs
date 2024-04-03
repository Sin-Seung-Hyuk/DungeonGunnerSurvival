using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class StaticEventHandler
{
    public static event Action<RoomChangedArgs> OnRoomChanged;
    public static void CallRoomChanged(Room room)
    {
        OnRoomChanged?.Invoke(new RoomChangedArgs() { room = room });
    }
}

public class RoomChangedArgs : EventArgs
{
    public Room room;
}
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


    // 무기 스탯변경 이벤트
    public static event Action OnWeaponStatChanged;
    public static void CallWeaponStatChangedEvent()
    {
        OnWeaponStatChanged?.Invoke();
    }


    // 장비템 클릭 이벤트
    public static event Action<EquipmentClickedArgs> OnEquipmentClicked;
    public static void CallEquipmentClicked(EquipmentType type, bool isClicked)
    {
        OnEquipmentClicked?.Invoke(new EquipmentClickedArgs() { type = type, isClicked = isClicked });
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

public class EquipmentClickedArgs : EventArgs
{
    public EquipmentType type;
    public bool isClicked;
}
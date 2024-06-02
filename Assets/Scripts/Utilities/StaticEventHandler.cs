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


    // ���� ���Ⱥ��� �̺�Ʈ
    public static event Action OnWeaponStatChanged;
    public static void CallWeaponStatChangedEvent()
    {
        OnWeaponStatChanged?.Invoke();
    }


    // ����� Ŭ�� �̺�Ʈ
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
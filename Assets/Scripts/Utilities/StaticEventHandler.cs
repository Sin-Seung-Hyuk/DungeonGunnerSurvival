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

    // �÷��̾� ���� ���� �̺�Ʈ (������UI,���,���ǿ��� ����)
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
    public bool isIncrease; // �����Ǵ� �������� ��������
}
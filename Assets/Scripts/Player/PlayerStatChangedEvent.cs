using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatChangedEvent : MonoBehaviour
{
    public event Action<PlayerStatChangedEvent, PlayerStatChangedEventArgs> OnPlayerStatChanged;

    public void CallPlayerStatChangedEvent(PlayerStatType statType, float value)
    {
        OnPlayerStatChanged?.Invoke(this, new PlayerStatChangedEventArgs()
        {
            statType = statType,
            changeValue = value
        });
    }
}

public class PlayerStatChangedEventArgs : EventArgs
{
    public PlayerStatType statType;
    public float changeValue;
}
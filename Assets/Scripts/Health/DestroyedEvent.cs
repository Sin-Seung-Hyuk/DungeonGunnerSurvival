using System;
using UnityEngine;

[DisallowMultipleComponent]
public class DestroyedEvent : MonoBehaviour
{
    public event Action<DestroyedEvent, DestroyedEventArgs> OnDestroyed;

    public void CallDestroyedEvent(bool isPlayer, int point)
    {
        OnDestroyed?.Invoke(this, new DestroyedEventArgs()
        {
            isPlayerDie = isPlayer,
            point = point
        });
    }
}

public class DestroyedEventArgs : EventArgs
{
    public bool isPlayerDie;
    public int point; // ÆÄ±« Æ÷ÀÎÆ®
}
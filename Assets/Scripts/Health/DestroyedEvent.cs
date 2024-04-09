using System;
using UnityEngine;

[DisallowMultipleComponent]
public class DestroyedEvent : MonoBehaviour
{
    public event Action<DestroyedEvent, DestroyedEventArgs> OnDestroyed;

    public void CallDestroyedEvent(bool isPooling, Vector3 point)
    {
        OnDestroyed?.Invoke(this, new DestroyedEventArgs()
        {
            isPooling = isPooling,
            point = point
        });
    }
}

public class DestroyedEventArgs : EventArgs
{
    public bool isPooling; // 오브젝트 풀에 반환해야하는지 (SetActive vs Destroy)
    public Vector3 point; // 파괴 위치
}
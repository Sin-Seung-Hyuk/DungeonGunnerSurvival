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
    public bool isPooling; // ������Ʈ Ǯ�� ��ȯ�ؾ��ϴ��� (SetActive vs Destroy)
    public Vector3 point; // �ı� ��ġ
}
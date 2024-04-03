using System;
using System.Collections.Generic;
using UnityEngine;

public class MovementEvent : MonoBehaviour
{
    public event Action<MovementEvent, MovementEventArgs> OnMovement;
    // �÷��̾� �̵��� ��Ʈ�ѷ� ���ο��� ���� ó��
    public event Action<MovementEvent> OnPlayerMovement; 

    public void CallMovement(Vector2 direction, float speed)
    {
        OnMovement?.Invoke(this, new MovementEventArgs()
        {
            moveDirection = direction,
            moveSpeed = speed
        });
    }

    public void CallPlayerMovement()
    {
        OnPlayerMovement?.Invoke(this);
    }
}

public class MovementEventArgs : EventArgs
{
    public Vector2 moveDirection;
    public float moveSpeed;
}

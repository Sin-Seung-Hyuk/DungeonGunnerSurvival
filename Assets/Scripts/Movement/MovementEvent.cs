using System;
using System.Collections.Generic;
using UnityEngine;

public class MovementEvent : MonoBehaviour
{
    public event Action<MovementEvent, MovementEventArgs> OnMovement;
    // 플레이어 이동은 컨트롤러 내부에서 따로 처리
    public event Action<MovementEvent> OnPlayerMovement; 

    public void CallMovement(Vector2 direction, float speed) // 방향,속도를 가지고 이벤트 호출
    {
        OnMovement?.Invoke(this, new MovementEventArgs()
        {
            moveDirection = direction,
            moveSpeed = speed
        });
    }

    public void CallPlayerMovement() // 플레이어의 이동 호출
    {
        OnPlayerMovement?.Invoke(this);
    }
}

public class MovementEventArgs : EventArgs
{
    public Vector2 moveDirection;
    public float moveSpeed;
}

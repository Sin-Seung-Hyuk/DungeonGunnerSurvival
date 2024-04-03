using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(IdleEvent))]
public class Idle : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private IdleEvent idleEvent;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        idleEvent = GetComponent<IdleEvent>();
    }
    private void OnEnable()
    {
        idleEvent.OnIdle += Idle_OnIdle;
    }
    private void OnDisable()
    {
        idleEvent.OnIdle -= Idle_OnIdle;
    }

    private void Idle_OnIdle(IdleEvent obj)
    {
        MoveRigidBody();
    }

    private void MoveRigidBody()
    {
        // Idle 이벤트 호출로 이동 멈추기
        rigidBody2D.velocity = Vector2.zero;
    }
}

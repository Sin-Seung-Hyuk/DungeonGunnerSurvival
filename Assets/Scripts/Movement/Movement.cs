using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MovementEvent))]
public class Movement : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private MovementEvent movementEvent;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        movementEvent = GetComponent<MovementEvent>();   
    }

    private void OnEnable()
    {
        movementEvent.OnMovement += Movement_OnMovement;
    }
    private void OnDisable()
    {
        movementEvent.OnMovement -= Movement_OnMovement;
    }

    private void Movement_OnMovement(MovementEvent movementEvent, MovementEventArgs args)
    {
        MoveRigidbody(args.moveDirection, args.moveSpeed);
    }

    private void MoveRigidbody(Vector2 moveDirection, float moveSpeed)
    {
        rigidBody2D.velocity = moveSpeed * moveDirection;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    private Player player;
    private PlayerStat playerStat;
    private Rigidbody2D rigid;

    private float moveSpeed;
    public Vector3 moveDirection { get; private set; }

    private void Awake()
    {
        player = GetComponent<Player>();
        rigid = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        playerStat = player.stat;
        moveSpeed = playerStat.MoveSpeed;
    }

    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        Move();
    }


    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveDirection = new Vector3(input.x, input.y, 0f);
    }
    
    private void Move()
    {
        rigid.velocity = moveDirection * moveSpeed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Door : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        int level = GameManager.Instance.GetCurrentDungeonLevel(); // 현재 레벨 구하기
        
        GameManager.Instance.CreateDungeonLevel(level);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(UniqueID))]
public class ItemPickUp : MonoBehaviour  // 아이템에 연결할 클래스
{
    [SerializeField] private InventoryItemData itemData; // 아이템이 가지고 있는 아이템데이터

    private SpriteRenderer spriteRenderer;

    private string id;


    public void InitializeItem(InventoryItemData data)
    {
        if (data == null) return; // 아무런 데이터가 들어오지 않았을 경우 (EnemySpawn 클래스 참고)

        itemData = data;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = itemData.ItemSprite;

        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var inventoryHolder = collision.transform.GetComponent<PlayerInventoryHolder>();

        if (!inventoryHolder) return; // 인벤토리 보유여부

        if (inventoryHolder.AddToInventory(itemData,1))
        {
            gameObject.SetActive(false);
        }
    }
}

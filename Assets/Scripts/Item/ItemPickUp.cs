using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(UniqueID))]
public class ItemPickUp : MonoBehaviour  // 아이템에 연결할 클래스
{
    public float pickUpRadius = 1f;
    public InventoryItemData itemData; // 아이템이 가지고 있는 아이템데이터

    private CircleCollider2D myCollider;
    private SpriteRenderer spriteRenderer;

    private string id;

    private void Awake()
    {
        id = GetComponent<UniqueID>().ID;

        myCollider = GetComponent<CircleCollider2D>();
        myCollider.isTrigger = true;
        myCollider.radius = pickUpRadius;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = itemData.ItemSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var inventoryHolder = collision.transform.GetComponent<PlayerInventoryHolder>();

        if (!inventoryHolder) return; // 인벤토리 보유여부

        if (inventoryHolder.AddToInventory(itemData,1))
        {
            Destroy(this.gameObject);
        }
    }
}


[System.Serializable] 
public struct ItemPickUpSaveData
{
    public InventoryItemData ItemData;
    public Vector3 position;
    public Quaternion rotation;
    
    public ItemPickUpSaveData(InventoryItemData data,Vector3 pos, Quaternion rot)
    {
        ItemData = data;
        position = pos;
        rotation = rot;
    }
}
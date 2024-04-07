using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(UniqueID))]
public class ItemPickUp : MonoBehaviour  // �����ۿ� ������ Ŭ����
{
    [SerializeField] private InventoryItemData itemData; // �������� ������ �ִ� �����۵�����

    private CircleCollider2D myCollider;
    private SpriteRenderer spriteRenderer;

    private string id;

    private void Awake()
    {
        id = GetComponent<UniqueID>().ID;

        myCollider = GetComponent<CircleCollider2D>();
        myCollider.isTrigger = true;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = itemData.ItemSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var inventoryHolder = collision.transform.GetComponent<PlayerInventoryHolder>();

        if (!inventoryHolder) return; // �κ��丮 ��������

        if (inventoryHolder.AddToInventory(itemData,1))
        {
            Destroy(this.gameObject);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(UniqueID))]
public class ItemPickUp : MonoBehaviour  // �����ۿ� ������ Ŭ����
{
    [SerializeField] private InventoryItemData itemData; // �������� ������ �ִ� �����۵�����

    private SpriteRenderer spriteRenderer;

    private string id;


    public void InitializeItem(InventoryItemData data)
    {
        if (data == null) return; // �ƹ��� �����Ͱ� ������ �ʾ��� ��� (EnemySpawn Ŭ���� ����)

        itemData = data;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = itemData.ItemSprite;

        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var inventoryHolder = collision.transform.GetComponent<PlayerInventoryHolder>();

        if (!inventoryHolder) return; // �κ��丮 ��������

        if (inventoryHolder.AddToInventory(itemData,1))
        {
            gameObject.SetActive(false);
        }
    }
}

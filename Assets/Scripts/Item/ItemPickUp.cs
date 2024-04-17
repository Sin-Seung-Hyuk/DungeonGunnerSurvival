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
    private ParticleSystem particle;
    private int gainExp;

    private string id;

    public void InitializeItem(InventoryItemData data)
    {
        if (data == null) return; // �ƹ��� �����Ͱ� ������ �ʾ��� ��� (EnemySpawn Ŭ���� ����)

        itemData = data;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = itemData.ItemSprite;

        particle = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particle.main; // ��ƼŬ �ý����� MainModule�� ���󺯰� ����
        main.startColor = itemData.gradeColor;

        gainExp = (int)itemData.itemGrade; // �ش� ��޿� �´� ����ġ ȹ�� (��޸��� ����ġ ����������)

        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player inventoryHolder = collision.transform.GetComponentInParent<Player>();

        if (!inventoryHolder) return; // �κ��丮 ��������

        if (inventoryHolder.playerInventory.AddToInventory(itemData,1))
        {
            gainExp += inventoryHolder.stat.expGain;
            inventoryHolder.playerExp.TakeExp(gainExp);
            gameObject.SetActive(false);
        }
    }
}

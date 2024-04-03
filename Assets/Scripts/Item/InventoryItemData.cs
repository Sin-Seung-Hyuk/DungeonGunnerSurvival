using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    public int ID = -1; // ������ ���� id
    public Sprite ItemSprite; // ������ ��������Ʈ (�ʵ�)
    public string DisplayName; // ȭ�鿡 ������ �̸�
    [TextArea(4, 2)] public string Description; // ������ ����
    public Sprite Icon; // ������ ������
    public int MaxStackSize; // ������ �����ѵ�
    public int GoldValue; // ������ ����

    public ItemGrade itemGrade; // ���
    public Color itemColor;
    public ItemType itemType; // Ÿ��
}

public enum ItemType
{
    Potion,
    JunkItem, // ����
    Equipment
}

public enum ItemGrade
{
    Normal,
    Rare,
    Unique
}
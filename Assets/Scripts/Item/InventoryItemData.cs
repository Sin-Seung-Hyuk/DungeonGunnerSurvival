using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    public int ID = -1; // ������ ���� id

    public string DisplayName; // ȭ�鿡 ������ �̸�
    [TextArea(4, 2)] public string Description; // ������ ����
    public Sprite ItemSprite; // ������ ��������Ʈ
    public int MaxStackSize; // ������ �����ѵ�
    public int GoldValue; // ������ ����

    public ItemGrade itemGrade; // ���
    public Color gradeColor; // ��޺� ����
    public ItemType itemType; // Ÿ��
    public EquipmentType equipmentType; // ��� Ÿ��

    public List<PlayerStatChangeList> playerStatChangeList; // ����� ����
}

public enum ItemType
{
    Potion,
    JunkItem, // ����
    Equipment
}

public enum EquipmentType
{
    Helmet,
    Necklaces,
    Ring,
    Glove,
    Belt,
    Boots,
    Armor,
    None
}

public enum ItemGrade
{
    Normal = 4,
    Rare = 8,
    Unique = 15,
    Legend = 30
}
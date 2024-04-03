using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    public int ID = -1; // 아이템 고유 id

    public string DisplayName; // 화면에 보여줄 이름
    [TextArea(4, 2)] public string Description; // 아이템 설명
    public Sprite ItemSprite; // 아이템 스프라이트
    public int MaxStackSize; // 아이템 보유한도
    public int GoldValue; // 아이템 가격

    public ItemGrade itemGrade; // 등급
    public Color gradeColor; // 등급별 색상
    public ItemType itemType; // 타입
}

public enum ItemType
{
    Potion,
    JunkItem, // 잡템
    Equipment
}

public enum ItemGrade
{
    Normal,
    Rare,
    Unique,
    Legend
}
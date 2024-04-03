using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Shop Item List")]
public class ShopItemList : ScriptableObject 
{
    // 상인이 가질 속성

    [SerializeField] private List<ShopInventoryItem> _items;
    [SerializeField] private int _maxAllowedGold; // 상인이 보유한 골드
    [SerializeField] private float _sellMarkUp;   // 판매 가격비율
    [SerializeField] private float _buyMarkUp;    // 구입 가격비율

    public List<ShopInventoryItem> Items => _items;
    public int MaxAllowedGold => _maxAllowedGold;
    public float SellMarkUp => _sellMarkUp;
    public float BuyMarkUp => _buyMarkUp;

}


[System.Serializable]
public struct ShopInventoryItem // 상인이 가질 아이템목록
{
    public InventoryItemData ItemData;
    public int Amount;
}
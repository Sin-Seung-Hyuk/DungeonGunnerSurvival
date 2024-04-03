using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Shop Item List")]
public class ShopItemList : ScriptableObject 
{
    // ������ ���� �Ӽ�

    [SerializeField] private List<ShopInventoryItem> _items;
    [SerializeField] private int _maxAllowedGold; // ������ ������ ���
    [SerializeField] private float _sellMarkUp;   // �Ǹ� ���ݺ���
    [SerializeField] private float _buyMarkUp;    // ���� ���ݺ���

    public List<ShopInventoryItem> Items => _items;
    public int MaxAllowedGold => _maxAllowedGold;
    public float SellMarkUp => _sellMarkUp;
    public float BuyMarkUp => _buyMarkUp;

}


[System.Serializable]
public struct ShopInventoryItem // ������ ���� �����۸��
{
    public InventoryItemData ItemData;
    public int Amount;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Shop Item List")]
public class ShopItemList : ScriptableObject 
{
    // ������ ���� �Ӽ�

    [SerializeField] private List<ShopInventoryItem> _items;
    [SerializeField] private float _sellMarkUp = 0.5f;   // �Ǹ� ���ݺ���
    [SerializeField] private float _buyMarkUp = 1.0f;    // ���� ���ݺ���

    public List<ShopInventoryItem> Items => _items;
    public float SellMarkUp => _sellMarkUp;
    public float BuyMarkUp => _buyMarkUp;

}


[System.Serializable]
public class ShopInventoryItem // ������ ���� �����۸��
{
    public InventoryItemData ItemData;
    public int Amount = 1;
}
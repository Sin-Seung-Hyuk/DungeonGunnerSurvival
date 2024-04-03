using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class ShopSystem
{
    [SerializeField] private List<ShopSlot> _shopInventory; // 아이템 슬롯
    [SerializeField] private int avaliableGold; // 상인 골드
    [SerializeField] private float _buyMarkUp;
    [SerializeField] private float _sellMarkUp;


    public List<ShopSlot> ShopInventory => _shopInventory;
    public int AvaliableGold => avaliableGold;
    public float BuyMarkUp => _buyMarkUp;
    public float SellMarkUp => _sellMarkUp;


    public ShopSystem(int size, int gold, float buyMarkUp, float sellMarkUp)
    {
        avaliableGold = gold;
        _buyMarkUp = buyMarkUp;
        _sellMarkUp = sellMarkUp;

        SetShopSize(size); // _shopInventory를 빈 슬롯으로 초기화
    }

    private void SetShopSize(int size)
    {
        _shopInventory = new List<ShopSlot>(size);

        for (int i =0; i<size; ++i)
        {
            _shopInventory.Add(new ShopSlot());
        }
    }

    public void AddToShop(InventoryItemData itemData, int amount)
    {
        if (ContainsItem(itemData, out ShopSlot shopSlot))
        {
            shopSlot.AddToStack(amount); // 가지고 있는 아이템이면 스택쌓기
            return;
        }

        ShopSlot freeSlot = GetFreeSlot();
        freeSlot.AssignItem(itemData, amount); // 빈슬롯에 아이템데이터,수량으로 초기화
    }

    private ShopSlot GetFreeSlot()
    {   // 상인 인벤토리에서 빈 슬롯 찾기
        var freeSlot = _shopInventory.FirstOrDefault(i => i.ItemData == null);

        if (freeSlot == null)
        {
            freeSlot = new ShopSlot();
            _shopInventory.Add(freeSlot);
        }

        return freeSlot;
    }

    public bool ContainsItem(InventoryItemData itemToAdd, out ShopSlot shopSlot)
    {
        // 상점인벤토리에서 아이템 찾기
        shopSlot = _shopInventory.Find(i => i.ItemData == itemToAdd);

        return shopSlot != null;
    }

    public void PurchaseItem(InventoryItemData data, int amount)
    {
        if (!ContainsItem(data, out ShopSlot slot)) return;

        slot.RemoveFromStack(amount); // 해당 아이템 스택감소
    }

    public void GainGold(int basketTotal)
    {
        avaliableGold += basketTotal; // 상인 골드획득
    }

    public void SellItem(InventoryItemData data, int amount, int price)
    {
        AddToShop(data, amount); // 유저가 판 아이템 상점에 추가
        ReduceGold(price);       // 상인 돈 감소
    }

    private void ReduceGold(int price)
    {
        avaliableGold -= price;
    }
}

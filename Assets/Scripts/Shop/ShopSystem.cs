using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class ShopSystem
{
    [SerializeField] private List<ShopSlot> _shopInventory; // ������ ����
    [SerializeField] private int avaliableGold; // ���� ���
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

        SetShopSize(size); // _shopInventory�� �� �������� �ʱ�ȭ
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
            shopSlot.AddToStack(amount); // ������ �ִ� �������̸� ���ýױ�
            return;
        }

        ShopSlot freeSlot = GetFreeSlot();
        freeSlot.AssignItem(itemData, amount); // �󽽷Կ� �����۵�����,�������� �ʱ�ȭ
    }

    private ShopSlot GetFreeSlot()
    {   // ���� �κ��丮���� �� ���� ã��
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
        // �����κ��丮���� ������ ã��
        shopSlot = _shopInventory.Find(i => i.ItemData == itemToAdd);

        return shopSlot != null;
    }

    public void PurchaseItem(InventoryItemData data, int amount)
    {
        if (!ContainsItem(data, out ShopSlot slot)) return;

        slot.RemoveFromStack(amount); // �ش� ������ ���ð���
    }

    public void GainGold(int basketTotal)
    {
        avaliableGold += basketTotal; // ���� ���ȹ��
    }

    public void SellItem(InventoryItemData data, int amount, int price)
    {
        AddToShop(data, amount); // ������ �� ������ ������ �߰�
        ReduceGold(price);       // ���� �� ����
    }

    private void ReduceGold(int price)
    {
        avaliableGold -= price;
    }
}

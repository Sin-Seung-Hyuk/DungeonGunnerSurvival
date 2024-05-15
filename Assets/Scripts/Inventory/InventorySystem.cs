using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[System.Serializable]
public class InventorySystem    // �κ��丮�� ���Ե��� �����ϴ� �κ��丮 �ý���
{
    [SerializeField] private List<InventorySlot> inventorySlots; // ���� ����Ʈ
    [SerializeField] private int gold; // ���� ��� (�κ��丮 �ý��� �������� ���)
    public List<InventorySlot> InventorySlots => inventorySlots;
    public int Gold => gold;
    public int InventorySize => inventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChanged; // �κ��丮 ���� ���� �̺�Ʈ


    public InventorySystem(InventorySlot_UI[] Slots)
    {
        inventorySlots = new List<InventorySlot>(Slots.Length);

        foreach (var slots in Slots)
        {
            slots.Init(new InventorySlot());
            inventorySlots.Add(slots.AssignedInventorySlot);
        }
    }
    public InventorySystem(int size) // �� 0�� ������
    {
        gold = 0;
        CreateInventory(size);
    }
    public InventorySystem(int size, int gold) // ���� ������ ������
    {
        this.gold = gold;
        CreateInventory(size);
    }
    private void CreateInventory(int size) // �κ��丮 �����Լ�
    {
        inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; ++i)
        {
            inventorySlots.Add(new InventorySlot()); // �� �������� �ʱ�ȭ
        }
    }


    // �κ��丮 �ý����� �����ϴ� ���� ����Ʈ�� ������ �߰��ϱ�
    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
    {
        // �κ��� �̹� ���� �������� ������
        if (ContainsItem(itemToAdd, out List<InventorySlot> invSlot))
        {
            foreach (var slot in invSlot) // ���� �������� ����ִ� ���� ��ȸ
            {
                if (slot.RoomLeftInStack(amountToAdd)) // ���� �߰� ���ɿ���
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot); // ����UI ������Ʈ �Լ��� ȣ��� (�׼ǿ� �����Ǿ)
                    return true;
                }
            }
        }

        // �κ��� ���� �������϶�
        if (HasFreeSlot(out InventorySlot freeSlot))
        {
            if (freeSlot.RoomLeftInStack(amountToAdd))
            {
                freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
                OnInventorySlotChanged?.Invoke(freeSlot);
                return true;
            }
        }

        return false;
    }

    public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> invSlot)
    {
        // Linq, ���ٽ� Ȱ��. ���Ը���Ʈ ��ȸ�ϸ鼭 �Ű����� �����Ϳ� ��ġ�ϴ� ���� ����Ʈ�� ��ȯ
        invSlot = inventorySlots.Where(i => i.ItemData == itemToAdd).ToList();
        //invSlot = (from slots in inventorySlots where slots.ItemData == itemToAdd select slots).ToList();

        // ���� �������� �Ѱ��� ������ �̹� ������ �ִ� �������̹Ƿ� true ��ȯ
        return invSlot == null ? false : true;
    }
    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        // ���ٽĴ�� ���Ը���Ʈ ��ȸ�ϸ� �����۵����Ͱ� null�� ������ ��ȯ 
        // ���ʷ� �߰��� ���� (First)�� ��ȯ, ������ null ��ȯ
        freeSlot = inventorySlots.FirstOrDefault(slot => slot.ItemData == null);

        return freeSlot == null ? false : true;
    }

    public bool CheckInventoryRemaining(Dictionary<InventoryItemData, int> shoppingCart)
    {
        InventorySystem cpySystem = new InventorySystem(this.InventorySize);

        for (int i = 0; i < InventorySize; ++i)
        {
            cpySystem.inventorySlots[i].AssignItem(this.inventorySlots[i].ItemData,
                this.inventorySlots[i].StackSize);
        }

        foreach (var pair in shoppingCart)
        {
            for (int i = 0; i < pair.Value; i++)
            {
                if (!cpySystem.AddToInventory(pair.Key, 1)) return false;
            }
        }

        return true;
    }

    public void GainGold(int price) // ��� ȹ��
    {
        gold += price;
    }
    public void SpendGold(int basketTotal) // ��� �Ҹ�
    {
        gold -= basketTotal;
    }

    public Dictionary<InventoryItemData, int> GetAllItem() // �κ��� ��� ������ ��ȯ
    {
        var distinctItems = new Dictionary<InventoryItemData, int>();

        foreach (var item in inventorySlots)
        {
            if (item.ItemData == null) continue;

            if (!distinctItems.ContainsKey(item.ItemData))
                distinctItems.Add(item.ItemData, item.StackSize);
            else distinctItems[item.ItemData] += item.StackSize;
        }

        return distinctItems;
    }

    public void RemoveItemsFromInventory(InventoryItemData data, int amount) // �κ����� ������ ����
    {
        if (ContainsItem(data, out List<InventorySlot> invSlot))
        {
            foreach (InventorySlot slot in invSlot)
            {
                int stackSize = slot.StackSize;

                if (stackSize > amount) slot.RemoveFromStack(amount); // ���ø� ����
                else
                {
                    slot.RemoveFromStack(stackSize); // RemoveFromStack ���ο��� ClearSlot() ȣ��
                    amount -= stackSize;

                    OnInventorySlotChanged?.Invoke(slot);
                }
            }
        }
    }
}

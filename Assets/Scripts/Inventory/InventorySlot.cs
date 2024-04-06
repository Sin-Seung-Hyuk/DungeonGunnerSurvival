using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot : ItemSlot
{
    public InventorySlot(InventoryItemData source, int amount) // ������
    {
        itemData = source;
        itemID = itemData.ID;
        stackSize = amount;
    }

    public InventorySlot(bool isEquip) // �⺻ ������
    {
        ClearSlot(isEquip);
    }


    public void UpdateInventorySlot(InventoryItemData data, int amount ) // ���� ������Ʈ
    {
        itemData = data;
        stackSize = amount;
        itemID = data.ID;
    }

    // �ش� ������ ������ ���� �� �ִ��� �˻� ===========================
    public bool RoomLeftInStack(int amountToAdd, out int amountRmaining)
    {   
        amountRmaining = itemData.MaxStackSize - stackSize;
        return RoomLeftInStack(amountToAdd);
    }

    public bool RoomLeftInStack(int amountToAdd)
    {
        if (ItemData == null || ItemData != null && stackSize + amountToAdd <= itemData.MaxStackSize)
            return true;
        return false;
    }


    // ������ ���� ������ ================================================
    public bool SplitStack(out InventorySlot splitStack)
    {
        if (stackSize <= 1)
        {
            splitStack = null;
            return false;
        }

        int halfStack = Mathf.RoundToInt(stackSize / 2);
        RemoveFromStack(halfStack);

        splitStack = new InventorySlot(itemData, halfStack);
        return true;
    }

}

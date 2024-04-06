using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot : ItemSlot
{
    public InventorySlot(InventoryItemData source, int amount) // 생성자
    {
        itemData = source;
        itemID = itemData.ID;
        stackSize = amount;
    }

    public InventorySlot(bool isEquip) // 기본 생성자
    {
        ClearSlot(isEquip);
    }


    public void UpdateInventorySlot(InventoryItemData data, int amount ) // 슬롯 업데이트
    {
        itemData = data;
        stackSize = amount;
        itemID = data.ID;
    }

    // 해당 아이템 스택을 쌓을 수 있는지 검사 ===========================
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


    // 아이템 스택 나누기 ================================================
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

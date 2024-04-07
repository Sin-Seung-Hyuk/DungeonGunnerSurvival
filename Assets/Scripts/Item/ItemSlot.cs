using System.Collections;
using System.Collections.Generic;
using UnityEngine;

                        // 직렬화,역직렬화를 위한 인터페이스
public class ItemSlot : ISerializationCallbackReceiver
{
    [SerializeField] protected InventoryItemData itemData; // 슬롯에 들어갈 아이템데이터
    [SerializeField] protected int stackSize; // 이 슬롯의 현재스택
    [SerializeField] protected int itemID = -1; // 이 슬롯에 들어간 아이템ID
    

    public InventoryItemData ItemData => itemData; 
    public int StackSize => stackSize;

    public void ClearSlot() // 슬롯 비우기
    {
        itemData = null;
        stackSize = -1;
        itemID = -1;
    }

    // 이 슬롯을 매개변수로 들어온 슬롯으로 초기화
    public void AssignItem(InventorySlot invSlot)
    {
        if (itemData == invSlot.ItemData) AddToStack(invSlot.stackSize); // 동일 아이템이면 스택쌓기
        else
        {
            itemData = invSlot.itemData;
            itemID = itemData.ID;
            stackSize = 0;
            AddToStack(invSlot.stackSize);
        }
    }
    // 이 슬롯을 매개변수로 들어온 아이템데이터,개수로 초기화
    public void AssignItem(InventoryItemData data, int amount)
    {
        if (itemData == data) AddToStack(stackSize); // 동일 아이템이면 스택쌓기
        else
        {
            itemData = data;
            itemID = data.ID;
            stackSize = 0;
            AddToStack(amount);
        }
    }


    // 아이템 개수만큼 스택에 쌓기 =======================================
    public void AddToStack(int amount)
    {
        stackSize += amount;
    }
    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
        if (stackSize <= 0) ClearSlot();
    }


    // 아이템 데이터베이스에서 아이템의 ID에 맞는 데이터 가져오기
    public void OnAfterDeserialize()
    {
        if (itemID == -1) return;

        var db = Resources.Load<Database>("Database");
        itemData = db.GetItem(itemID);
    }

    public void OnBeforeSerialize()
    {

    }
}

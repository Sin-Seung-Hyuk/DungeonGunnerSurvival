using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[System.Serializable]
public class InventorySystem    // 인벤토리의 슬롯들을 관리하는 인벤토리 시스템
{
    [SerializeField] private List<InventorySlot> inventorySlots; // 슬롯 리스트
    [SerializeField] private int gold; // 보유 골드 (인벤토리 시스템 소유자의 골드)
    public List<InventorySlot> InventorySlots => inventorySlots;
    public int Gold => gold;
    public int InventorySize => inventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChanged; // 인벤토리 슬롯 변경 이벤트


    public InventorySystem(InventorySlot_UI[] Slots)
    {
        inventorySlots = new List<InventorySlot>(Slots.Length);

        foreach (var slots in Slots)
        {
            slots.Init(new InventorySlot());
            inventorySlots.Add(slots.AssignedInventorySlot);
        }
    }
    public InventorySystem(int size) // 돈 0원 생성자
    {
        gold = 0;
        CreateInventory(size);
    }
    public InventorySystem(int size, int gold) // 돈을 지정한 생성자
    {
        this.gold = gold;
        CreateInventory(size);
    }
    private void CreateInventory(int size) // 인벤토리 생성함수
    {
        inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; ++i)
        {
            inventorySlots.Add(new InventorySlot()); // 빈 슬롯으로 초기화
        }
    }


    // 인벤토리 시스템이 관리하는 슬롯 리스트에 아이템 추가하기
    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
    {
        // 인벤에 이미 같은 아이템이 있을때
        if (ContainsItem(itemToAdd, out List<InventorySlot> invSlot))
        {
            foreach (var slot in invSlot) // 같은 아이템이 들어있는 슬롯 순회
            {
                if (slot.RoomLeftInStack(amountToAdd)) // 스택 추가 가능여부
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot); // 슬롯UI 업데이트 함수가 호출됨 (액션에 구독되어서)
                    return true;
                }
            }
        }

        // 인벤에 없는 아이템일때
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
        // Linq, 람다식 활용. 슬롯리스트 순회하면서 매개변수 데이터와 일치하는 원소 리스트로 반환
        invSlot = inventorySlots.Where(i => i.ItemData == itemToAdd).ToList();
        //invSlot = (from slots in inventorySlots where slots.ItemData == itemToAdd select slots).ToList();

        // 같은 아이템이 한개라도 있으면 이미 가지고 있는 아이템이므로 true 반환
        return invSlot == null ? false : true;
    }
    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        // 람다식대로 슬롯리스트 순회하며 아이템데이터가 null인 슬롯을 반환 
        // 최초로 발견한 슬롯 (First)를 반환, 없으면 null 반환
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

    public void GainGold(int price) // 골드 획득
    {
        gold += price;
    }
    public void SpendGold(int basketTotal) // 골드 소모
    {
        gold -= basketTotal;
    }

    public Dictionary<InventoryItemData, int> GetAllItem() // 인벤의 모든 아이템 반환
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

    public void RemoveItemsFromInventory(InventoryItemData data, int amount) // 인벤에서 아이템 감소
    {
        if (ContainsItem(data, out List<InventorySlot> invSlot))
        {
            foreach (InventorySlot slot in invSlot)
            {
                int stackSize = slot.StackSize;

                if (stackSize > amount) slot.RemoveFromStack(amount); // 스택만 감소
                else
                {
                    slot.RemoveFromStack(stackSize); // RemoveFromStack 내부에서 ClearSlot() 호출
                    amount -= stackSize;

                    OnInventorySlotChanged?.Invoke(slot);
                }
            }
        }
    }
}

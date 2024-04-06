using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// InventoryHolder를 상속받아 인벤토리 시스템 하나를 가지고 있는 상태
public class PlayerInventoryHolder : InventoryHolder
{
    // 동적인벤토리(플레이어)가 교체되어 할당된 경우
    public static UnityAction OnPlayerInventoryChanged;
    // 동적인벤토리(플레이어) 화면을 출력해야 하는 경우
    public static UnityAction<InventorySystem, int> OnPlayerInventoryDisplayRequested;


    private void Start()
    {
        SaveLoad.OnSaveGame += SaveFile;
    }

    private void SaveFile()
    {
        SaveGameManager.data.playerInventory = new InventorySaveData(primaryInventorySystem);
    }

    protected override void LoadInventory(SaveData data)
    {   // 세이브 데이터 확인하고 저장된 데이터 가져와 인벤토리 초기화

        // 세이브데이터의 플레이어 세이브데이터 확인하기
        if (data.playerInventory.invSystem != null)
        {
            this.primaryInventorySystem = data.playerInventory.invSystem;
            OnPlayerInventoryChanged?.Invoke();
        }
    }

    void Update()
    {
        // I 키로 인벤토리 활성화
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            OnPlayerInventoryDisplayRequested?.Invoke(primaryInventorySystem, offset);
        }

    }

    public bool AddToInventory(InventoryItemData data, int amount)
    {
        // 첫 인벤토리에 먼저 추가
        if (primaryInventorySystem.AddToInventory(data, amount))
            return true;

        return false;
    }

    public bool GetNumpadItem(int num)
    {
        if (primaryInventorySystem.InventorySlots[num].ItemData != null)
        {
            primaryInventorySystem.InventorySlots[num].RemoveFromStack(1);
            if (primaryInventorySystem.InventorySlots[num].StackSize < 1)
                primaryInventorySystem.InventorySlots[num].ClearSlot();
            OnPlayerInventoryChanged?.Invoke();

            return true;
        }

        return false;
    }
}

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

    private Player player;


    private void Start()
    {
        SaveLoad.OnSaveGame += SaveFile;

        player = GetComponent<Player>();
    }

    private void SaveFile()
    {
        SaveGameManager.data.playerInventory = new InventorySaveData(primaryInventorySystem);
    }

    protected override void LoadInventory(SaveData data)
    {   
        // 플레이어 인벤토리는 세이브 X
        // 게임이 시작되면 창고만 로드되고 인벤토리는 초기화
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

    public bool GetNumpadItem(int num) // 숫자패드(슬롯)에 등록된 포션 사용
    {
        // 해당 슬롯이 비어있지 않고 아이템 타입이 포션이여야 함
        if (primaryInventorySystem.InventorySlots[num].ItemData != null
            && primaryInventorySystem.InventorySlots[num].ItemData.itemType == ItemType.Potion)
        {
            // 플레이어 클래스에 접근해 해당 타입의 포션을 사용할 수 있는지 검사 (중복사용 X)
            if (player.CanUsePotion(primaryInventorySystem.InventorySlots[num].ItemData.playerStatChangeList[0].statType)) {
                // 플레이어 클래스에 접근하여 포션 사용함수 호출
                player.UsePotion(primaryInventorySystem.InventorySlots[num].ItemData);

                primaryInventorySystem.InventorySlots[num].RemoveFromStack(1); // 1개 감소
                if (primaryInventorySystem.InventorySlots[num].StackSize < 1)
                    primaryInventorySystem.InventorySlots[num].ClearSlot();
                OnPlayerInventoryChanged?.Invoke(); // 인벤토리 변경 이벤트 호출

                return true;
            }
        }

        return false;
    }
}

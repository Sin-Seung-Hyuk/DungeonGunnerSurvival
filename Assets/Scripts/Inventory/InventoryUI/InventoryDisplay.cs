using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;


// 인벤토리 디스플레이 추상클래스 : 인벤토리 UI에 등록하여 화면에 보여주는 부분을 관리
public abstract class InventoryDisplay : MonoBehaviour 
{
    [SerializeField] MouseItemData mouseInventoryItem;
    [SerializeField] protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary;

    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;


    protected virtual void Start()
    {
        
    }

    public abstract void AssignSlot(InventorySystem invToDisplay, int offset); // 인벤토리 시스템의 슬롯정보를 슬롯Ui에 복붙

    protected virtual void UpdateSlot(InventorySlot updateSlot) // 슬롯UI 한칸을 매개변수 슬롯과 동일하게 초기화
    {
        foreach (var slot in SlotDictionary)
        {
            if (slot.Value == updateSlot) // 슬롯UI의 슬롯과 변경된 슬롯이 같으면
            {
                slot.Key.UpdateUISlot(updateSlot); // 슬롯UI 업데이트 호출
            }
        }
    }

    // 인벤토리 슬롯 클릭 함수 ================================================================================================
    public void SlotClicked(InventorySlot_UI clickedSlotUI)
    {
        bool isAltPressed = Keyboard.current.leftAltKey.isPressed; // Alt키 입력감지

        // 클릭한 슬롯UI에 아이템데이터 있어야하고 && 마우스가 이미 클릭해서 가진 아이템정보가 null이여야함
        if (clickedSlotUI.AssignedInventorySlot.ItemData != null &&
            mouseInventoryItem.AssignedInventorySlot.ItemData == null)
        {
            if (isAltPressed && clickedSlotUI.AssignedInventorySlot.SplitStack(out InventorySlot splitStack))
            {
                mouseInventoryItem.UpdateMouseSlot(splitStack);
                clickedSlotUI.UpdateUISlot();
                return;
            }
            else
            {
                mouseInventoryItem.UpdateMouseSlot(clickedSlotUI.AssignedInventorySlot);
                clickedSlotUI.ClearSlot();

                // 장비아이템을 클릭했다면 장비창에서 어디에 장착해야하는지 나타나야함
                if (mouseInventoryItem.AssignedInventorySlot.ItemData.ItemType == ItemType.Equipment) {
                    
                }
                
                return;
            }
        }

        // 비어있는 슬롯에 마우스 데이터가 있는 상태에서 클릭한다면 (템옮기기)
        if (clickedSlotUI.AssignedInventorySlot.ItemData == null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            clickedSlotUI.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
            clickedSlotUI.UpdateUISlot();

            mouseInventoryItem.ClearSlot();
            return;
        }

        // 클릭한 슬롯에 아이템이 이미 있고 마우스에 아이템 들고있으면 (스왑or합치기)
        if (clickedSlotUI.AssignedInventorySlot.ItemData != null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {   // 클릭한 슬롯의 아이템과 마우스 아이템이 같은지 비교
            bool isSame = clickedSlotUI.AssignedInventorySlot.ItemData == mouseInventoryItem.AssignedInventorySlot.ItemData;

            // 서로 같은 아이템, 합칠 수 있으면 합치고 업데이트
            if (isSame && clickedSlotUI.AssignedInventorySlot.RoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize))
            {
                clickedSlotUI.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
                clickedSlotUI.UpdateUISlot();

                mouseInventoryItem.ClearSlot();
            }
            // 같은 아이템, 들고있는 수량이 커 합칠 수 없음
            else if (isSame &&
                !clickedSlotUI.AssignedInventorySlot.RoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize, out int leftInStack))
            {
                if (leftInStack < 1) SwapSlot(clickedSlotUI); // 스택이 꽉차있음
                else
                {   // 마우스에 들고있는 스택을 클릭한 슬롯에 넣고 남은 스택
                    int remainingMouse = mouseInventoryItem.AssignedInventorySlot.StackSize - leftInStack;

                    clickedSlotUI.AssignedInventorySlot.AddToStack(leftInStack);
                    clickedSlotUI.UpdateUISlot();

                    var newItem = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, remainingMouse);
                    mouseInventoryItem.ClearSlot();
                    mouseInventoryItem.UpdateMouseSlot(newItem);
                    return;
                }
            }

            else if (!isSame) // 서로 다른 아이템이면 아이템 스왑
            {
                SwapSlot(clickedSlotUI);
                return;
            }

        }
    }


    // 장비창 슬롯 클릭 함수 ================================================================================================
    public void SlotClicked(EquipmentSlot_UI clickedSlotUI, EquipmentType slotType)
    {
        // 0. 아이템을 장착/해제 할때에 호출되는 이벤트는 동일하게 하나의 이벤트로 장착/해제 여부에 따라 반영만 다르게 하면됨

        // 1. 슬롯이 비어있고 마우스 아이템이 '장비'타입인지 확인
        // 1-1. '장비'타입이면 해당 슬롯에 맞는 '부위'인지 확인
        // 1-2. 모두 통과되면 장착하고 이벤트 호출 (장비이벤트->스탯반영)
        if (clickedSlotUI.AssignedInventorySlot.ItemData == null &&
            mouseInventoryItem.AssignedInventorySlot.ItemData.itemType == ItemType.Equipment)
        {
            if (mouseInventoryItem.AssignedInventorySlot.ItemData.equipmentType == slotType)
            {
                clickedSlotUI.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
                clickedSlotUI.UpdateUISlot();

                clickedSlotUI.EquipItem(clickedSlotUI.AssignedInventorySlot.ItemData.playerStatChangeList);

                mouseInventoryItem.ClearSlot();
                return;
            }
        }

        // 2. 슬롯에 장비가 장착되어 있고 마우스가 비어있는지 확인
        // 2-1. 슬롯을 비우고 장비 이벤트 호출 (장비이벤트->스탯반영)
        // 2-2. 마우스에 해당 장비의 아이템으로 초기화
        if (clickedSlotUI.AssignedInventorySlot.ItemData != null &&
            mouseInventoryItem.AssignedInventorySlot.ItemData == null)
        {
            // 이벤트 호출
            clickedSlotUI.UnequipItem(clickedSlotUI.AssignedInventorySlot.ItemData.playerStatChangeList);

            mouseInventoryItem.UpdateMouseSlot(clickedSlotUI.AssignedInventorySlot);
            clickedSlotUI.ClearSlot();
            return;
        }

        // 3. 슬롯에 장비가 장착되어 있고 마우스에 아이템이 들려있는 경우
        // 3-1. 슬롯 장비와 마우스 장비의 타입을 비교
        if (clickedSlotUI.AssignedInventorySlot.ItemData != null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            if (mouseInventoryItem.AssignedInventorySlot.ItemData.equipmentType == slotType)
            {
                SwapSlot(clickedSlotUI);
                clickedSlotUI.UnequipItem(mouseInventoryItem.AssignedInventorySlot.ItemData.playerStatChangeList);
                clickedSlotUI.EquipItem(clickedSlotUI.AssignedInventorySlot.ItemData.playerStatChangeList);

                return;
            }
        }
    }


    // 클릭한 슬롯과 아이템 데이터 교체하기
    private void SwapSlot(InventorySlot_UI clickedSlot)
    {
        // 마우스가 들고있는 아이템 정보로 임의 슬롯 생성
        var newSlot = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, 
            mouseInventoryItem.AssignedInventorySlot.StackSize);
        mouseInventoryItem.ClearSlot();
        mouseInventoryItem.UpdateMouseSlot(clickedSlot.AssignedInventorySlot);

        // 클릭한 슬롯의 정보를 마우스 아이템으로 변경하고 UI 업데이트
        clickedSlot.ClearSlot();
        clickedSlot.AssignedInventorySlot.AssignItem(newSlot);
        clickedSlot.UpdateUISlot();
    }


    public abstract void HighlightEquipment();
}

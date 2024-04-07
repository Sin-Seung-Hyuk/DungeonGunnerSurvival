using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;


// �κ��丮 ���÷��� �߻�Ŭ���� : �κ��丮 UI�� ����Ͽ� ȭ�鿡 �����ִ� �κ��� ����
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

    public abstract void AssignSlot(InventorySystem invToDisplay, int offset); // �κ��丮 �ý����� ���������� ����Ui�� ����

    protected virtual void UpdateSlot(InventorySlot updateSlot) // ����UI ��ĭ�� �Ű����� ���԰� �����ϰ� �ʱ�ȭ
    {
        foreach (var slot in SlotDictionary)
        {
            if (slot.Value == updateSlot) // ����UI�� ���԰� ����� ������ ������
            {
                slot.Key.UpdateUISlot(updateSlot); // ����UI ������Ʈ ȣ��
            }
        }
    }

    // �κ��丮 ���� Ŭ�� �Լ� ================================================================================================
    public void SlotClicked(InventorySlot_UI clickedSlotUI)
    {
        bool isAltPressed = Keyboard.current.leftAltKey.isPressed; // AltŰ �Է°���

        // Ŭ���� ����UI�� �����۵����� �־���ϰ� && ���콺�� �̹� Ŭ���ؼ� ���� ������������ null�̿�����
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
                return;
            }
        }

        // ����ִ� ���Կ� ���콺 �����Ͱ� �ִ� ���¿��� Ŭ���Ѵٸ� (�ۿű��)
        if (clickedSlotUI.AssignedInventorySlot.ItemData == null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            clickedSlotUI.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
            clickedSlotUI.UpdateUISlot();

            mouseInventoryItem.ClearSlot();
            return;
        }

        // Ŭ���� ���Կ� �������� �̹� �ְ� ���콺�� ������ ��������� (����or��ġ��)
        if (clickedSlotUI.AssignedInventorySlot.ItemData != null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {   // Ŭ���� ������ �����۰� ���콺 �������� ������ ��
            bool isSame = clickedSlotUI.AssignedInventorySlot.ItemData == mouseInventoryItem.AssignedInventorySlot.ItemData;

            // ���� ���� ������, ��ĥ �� ������ ��ġ�� ������Ʈ
            if (isSame && clickedSlotUI.AssignedInventorySlot.RoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize))
            {
                clickedSlotUI.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
                clickedSlotUI.UpdateUISlot();

                mouseInventoryItem.ClearSlot();
            }
            // ���� ������, ����ִ� ������ Ŀ ��ĥ �� ����
            else if (isSame &&
                !clickedSlotUI.AssignedInventorySlot.RoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize, out int leftInStack))
            {
                if (leftInStack < 1) SwapSlot(clickedSlotUI); // ������ ��������
                else
                {   // ���콺�� ����ִ� ������ Ŭ���� ���Կ� �ְ� ���� ����
                    int remainingMouse = mouseInventoryItem.AssignedInventorySlot.StackSize - leftInStack;

                    clickedSlotUI.AssignedInventorySlot.AddToStack(leftInStack);
                    clickedSlotUI.UpdateUISlot();

                    var newItem = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, remainingMouse);
                    mouseInventoryItem.ClearSlot();
                    mouseInventoryItem.UpdateMouseSlot(newItem);
                    return;
                }
            }

            else if (!isSame) // ���� �ٸ� �������̸� ������ ����
            {
                SwapSlot(clickedSlotUI);
                return;
            }

        }
    }


    // ���â ���� Ŭ�� �Լ� ================================================================================================
    public void SlotClicked(EquipmentSlot_UI clickedSlotUI, EquipmentType slotType)
    {
        // 0. �������� ����/���� �Ҷ��� ȣ��Ǵ� �̺�Ʈ�� �����ϰ� �ϳ��� �̺�Ʈ�� ����/���� ���ο� ���� �ݿ��� �ٸ��� �ϸ��

        // 1. ������ ����ְ� ���콺 �������� '���'Ÿ������ Ȯ��
        // 1-1. '���'Ÿ���̸� �ش� ���Կ� �´� '����'���� Ȯ��
        // 1-2. ��� ����Ǹ� �����ϰ� �̺�Ʈ ȣ�� (����̺�Ʈ->���ȹݿ�)
        if (clickedSlotUI.AssignedInventorySlot.ItemData == null &&
            mouseInventoryItem.AssignedInventorySlot.ItemData.itemType == ItemType.Equipment)
        {
            if (mouseInventoryItem.AssignedInventorySlot.ItemData.equipmentType == slotType)
            {
                clickedSlotUI.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
                clickedSlotUI.UpdateUISlot();

                mouseInventoryItem.ClearSlot();
                return;
            }
        }

        // 2. ���Կ� ��� �����Ǿ� �ְ� ���콺�� ����ִ��� Ȯ��
        // 2-1. ������ ���� ��� �̺�Ʈ ȣ�� (����̺�Ʈ->���ȹݿ�)
        // 2-2. ���콺�� �ش� ����� ���������� �ʱ�ȭ
        if (clickedSlotUI.AssignedInventorySlot.ItemData != null &&
            mouseInventoryItem.AssignedInventorySlot.ItemData == null)
        {
            // �̺�Ʈ ȣ��
            mouseInventoryItem.UpdateMouseSlot(clickedSlotUI.AssignedInventorySlot);
            clickedSlotUI.ClearSlot();
            return;
        }

        // 3. ���Կ� ��� �����Ǿ� �ְ� ���콺�� �������� ����ִ� ���
        // 3-1. ���� ���� ���콺 ����� Ÿ���� ��
        if (clickedSlotUI.AssignedInventorySlot.ItemData != null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            if (mouseInventoryItem.AssignedInventorySlot.ItemData.equipmentType == slotType)

            {
                SwapSlot(clickedSlotUI);
                return;
            }
        }
    }


    // Ŭ���� ���԰� ������ ������ ��ü�ϱ�
    private void SwapSlot(InventorySlot_UI clickedSlot)
    {
        // ���콺�� ����ִ� ������ ������ ���� ���� ����
        var newSlot = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, 
            mouseInventoryItem.AssignedInventorySlot.StackSize);
        mouseInventoryItem.ClearSlot();
        mouseInventoryItem.UpdateMouseSlot(clickedSlot.AssignedInventorySlot);

        // Ŭ���� ������ ������ ���콺 ���������� �����ϰ� UI ������Ʈ
        clickedSlot.ClearSlot();
        clickedSlot.AssignedInventorySlot.AssignItem(newSlot);
        clickedSlot.UpdateUISlot();
    }

}

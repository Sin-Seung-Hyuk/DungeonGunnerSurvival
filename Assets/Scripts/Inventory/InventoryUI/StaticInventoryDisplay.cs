using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventorySlot_UI[] slots;


    private void OnEnable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged += RefreshStaticDisplay;
    }
    private void OnDisable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged -= RefreshStaticDisplay;
    }

    private void RefreshStaticDisplay()
    {
        if (inventoryHolder != null)
        {
            inventorySystem = inventoryHolder.PrimaryInventorySystem;
            // ��ӹ��� �߻�Ŭ���� ������ ������
            inventorySystem.OnInventorySlotChanged += UpdateSlot; // InventoryDisplay �߻�Ŭ������ ������ �Լ�
        }

        AssignSlot(inventorySystem, 0);
    }

    protected override void Start()
    {
        base.Start();
        RefreshStaticDisplay();
    }

    // �κ��丮 �ý����� �κ��丮 ���� ����Ʈ�� ����UI�� �ű�� �۾�
    public override void AssignSlot(InventorySystem invToDisplay, int offset)
    {
        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        // 0������ ������(������ ũ��)���� �κ��丮 �ʱ�ȭ
        for (int i = 0; i < 10; ++i)
        {
            // ����UI�� �κ��丮 �ý����� ���Ը���Ʈ�� �״�� �ű��
            slotDictionary.Add(slots[i], inventorySystem.InventorySlots[i]);
            slots[i].Init(inventorySystem.InventorySlots[i]);
            slots[i].itemNumPad.text = (i + 1).ToString();
        }
    }

}

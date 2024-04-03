using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicInventoryDisplay : InventoryDisplay
{
    // â��� ���� �������� ���� ĭ�� ä������ �κ��丮
    [SerializeField] protected InventorySlot_UI slotPrefab;


    protected override void Start()
    {
        base.Start();
    }
    private void OnDisable()
    {
        if (inventorySystem != null) inventorySystem.OnInventorySlotChanged -= UpdateSlot;
    }

    // �κ��丮 ���ΰ�ħ 
    public void RefreshInventory(InventorySystem invToDisplay,int offset)
    {
        ClearSlots();
        // Inventory Display �߻�Ŭ������ ����� �κ��ý���
        inventorySystem = invToDisplay;
        // �κ��丮�� ������ �߰��ɶ����� ���� ������Ʈ
        if (inventorySystem!=null)
            inventorySystem.OnInventorySlotChanged += UpdateSlot; // �߻�Ŭ������ ����, ����Ui ����
        AssignSlot(invToDisplay, offset);
    }

    // �κ��丮 �ý����� ���������� ���� ����UI ������Ʈ
    public override void AssignSlot(InventorySystem invToDisplay, int offset)
    {
        ClearSlots();

        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        if (invToDisplay == null) return;

        // i�� offset ���� : ~offset������ �������� ���� �ε����� ����
        for (int i=offset; i< invToDisplay.InventorySize; ++i)
        {
            var uiSlot = Instantiate(slotPrefab, transform);
            SlotDictionary.Add(uiSlot, invToDisplay.InventorySlots[i]);
            uiSlot.Init(invToDisplay.InventorySlots[i]);
            uiSlot.itemNumPad.text = (i+1).ToString();
            uiSlot.UpdateUISlot();
        }
    }

    private void ClearSlots()
    {
        foreach (var item in transform.Cast<Transform>()) {
            Destroy(item.gameObject);
        }

        // �θ�Ŭ������ ����� <����UI,����> ��ųʸ�
        if (slotDictionary != null) slotDictionary.Clear();
    }
}

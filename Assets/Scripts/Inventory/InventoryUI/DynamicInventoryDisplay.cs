using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicInventoryDisplay : InventoryDisplay
{
    // 창고와 같이 동적으로 슬롯 칸이 채워지는 인벤토리
    [SerializeField] protected InventorySlot_UI slotPrefab;


    protected override void Start()
    {
        base.Start();
    }
    private void OnDisable()
    {
        if (inventorySystem != null) inventorySystem.OnInventorySlotChanged -= UpdateSlot;
    }

    // 인벤토리 새로고침 
    public void RefreshInventory(InventorySystem invToDisplay,int offset)
    {
        ClearSlots();
        // Inventory Display 추상클래스에 선언된 인벤시스템
        inventorySystem = invToDisplay;
        // 인벤토리에 아이템 추가될때마다 슬롯 업데이트
        if (inventorySystem!=null)
            inventorySystem.OnInventorySlotChanged += UpdateSlot; // 추상클래스에 구현, 슬롯Ui 업뎃
        AssignSlot(invToDisplay, offset);
    }

    // 인벤토리 시스템의 슬롯정보를 토대로 슬롯UI 업데이트
    public override void AssignSlot(InventorySystem invToDisplay, int offset)
    {
        ClearSlots();

        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        if (invToDisplay == null) return;

        // i는 offset 부터 : ~offset까지는 퀵슬롯이 쓰는 인덱스기 때문
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

        // 부모클래스에 선언된 <슬롯UI,슬롯> 딕셔너리
        if (slotDictionary != null) slotDictionary.Clear();
    }
}

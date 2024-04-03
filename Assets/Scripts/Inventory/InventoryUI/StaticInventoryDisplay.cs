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
            // 상속받은 추상클래스 내에서 구현됨
            inventorySystem.OnInventorySlotChanged += UpdateSlot; // InventoryDisplay 추상클래스에 구현된 함수
        }

        AssignSlot(inventorySystem, 0);
    }

    protected override void Start()
    {
        base.Start();
        RefreshStaticDisplay();
    }

    // 인벤토리 시스템의 인벤토리 슬롯 리스트를 슬롯UI에 옮기는 작업
    public override void AssignSlot(InventorySystem invToDisplay, int offset)
    {
        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        // 0번부터 오프셋(퀵슬롯 크기)까지 인벤토리 초기화
        for (int i = 0; i < 10; ++i)
        {
            // 슬롯UI에 인벤토리 시스템의 슬롯리스트를 그대로 옮기기
            slotDictionary.Add(slots[i], inventorySystem.InventorySlots[i]);
            slots[i].Init(inventorySystem.InventorySlots[i]);
            slots[i].itemNumPad.text = (i + 1).ToString();
        }
    }

}

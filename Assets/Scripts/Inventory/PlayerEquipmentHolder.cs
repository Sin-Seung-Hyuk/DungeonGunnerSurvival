using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerEquipmentHolder : InventoryDisplay
{
    public static UnityAction OnPlayerEquipmentDisplayRequested;

    [SerializeField] private EquipmentSlot_UI[] slots;
    [SerializeField] private Image playerSprite;

    [SerializeField] private PlayerStatUI playerStatUI;
    [SerializeField] private WeaponStatUI weaponStatUI;

    private void Awake()
    {
        inventorySystem = new InventorySystem(slots);

        SaveLoad.OnLoadGame += LoadInventory;
    }

    protected override void Start()
    {
        base.Start();
        SaveLoad.OnSaveGame += SaveFile;

        playerSprite.sprite = GameManager.Instance.GetPlayer().playerDetails.playerSprite;

    }

    private void SaveFile()
    {
        SaveGameManager.data.playerEquipment = new EquipmentSaveData(inventorySystem);
    }

    private void LoadInventory(SaveData data)
    {   // 세이브 데이터 확인하고 저장된 데이터 가져와 인벤토리 초기화
        // 세이브데이터의 플레이어 세이브데이터 확인하기

        if (data.playerEquipment.invSystem != null)
        {
            this.inventorySystem = data.playerEquipment.invSystem;
            AssignSlot(inventorySystem,0);
        }
    }

    public void StatInfoChange()
    {
        if (playerStatUI.gameObject.activeSelf)
        {
            playerStatUI.gameObject.SetActive(false);
            weaponStatUI.gameObject.SetActive(true);
        }
        else
        {
            playerStatUI.gameObject.SetActive(true);
            weaponStatUI.gameObject.SetActive(false);
        }
    }

    public override void AssignSlot(InventorySystem invToDisplay, int offset)
    {
        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        if (invToDisplay == null) return;

        for (int i = 0; i < invToDisplay.InventorySize; ++i)
        {
            SlotDictionary.Add(slots[i], invToDisplay.InventorySlots[i]);
            slots[i].Init(invToDisplay.InventorySlots[i]);
            slots[i].UpdateUISlot();

            if (invToDisplay.InventorySlots[i].ItemData != null)
            {   // 게임 시작시 장착되어 있는 장비 적용
                slots[i].EquipItem(invToDisplay.InventorySlots[i].ItemData.playerStatChangeList);
            }
        }
    }
}

[System.Serializable]
public struct EquipmentSaveData
{
    public InventorySystem invSystem;

    public EquipmentSaveData(InventorySystem invSystem)
    {
        this.invSystem = invSystem;
    }
}
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
    {   // ���̺� ������ Ȯ���ϰ� ����� ������ ������ �κ��丮 �ʱ�ȭ
        // ���̺굥������ �÷��̾� ���̺굥���� Ȯ���ϱ�

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
            {   // ���� ���۽� �����Ǿ� �ִ� ��� ����
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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
    // �κ��丮 UI�� ��� �����ϴ� ��Ʈ�ѷ�
    // â�� ���� Ư�� ��Ȳ������ ������ �κ��丮�� ������ �ִ�.

    [FormerlySerializedAs("chestPanel")] // �ν������� �� ���� (�����丵�ص� ����)
    public DynamicInventoryDisplay inventoryPanel; // �����κ��丮 UI (â��)
    public DynamicInventoryDisplay playerPanel; // �����κ��丮 UI (�÷��̾�)
    public PlayerEquipmentHolder equipmentPanel; // �����κ��丮 UI (���)

    private void Awake()
    {
        inventoryPanel.gameObject.SetActive(false);
        playerPanel.gameObject.SetActive(false);
        equipmentPanel.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
        PlayerInventoryHolder.OnPlayerInventoryDisplayRequested += DisplayPlayerInventory;
        PlayerEquipmentHolder.OnPlayerEquipmentDisplayRequested += DisplayPlayerEquipment;
    }
    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
        PlayerInventoryHolder.OnPlayerInventoryDisplayRequested -= DisplayPlayerInventory;
        PlayerEquipmentHolder.OnPlayerEquipmentDisplayRequested -= DisplayPlayerEquipment;
    }

    void Update()
    {
        if (inventoryPanel.gameObject.activeInHierarchy &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
            inventoryPanel.gameObject.SetActive(false);

        if (playerPanel.gameObject.activeInHierarchy &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        { 
            playerPanel.gameObject.SetActive(false);
        }

        if (equipmentPanel.gameObject.activeInHierarchy &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            equipmentPanel.gameObject.SetActive(false);

            Time.timeScale = 1;
        }

        // TAB Ű�� ���â Ȱ��ȭ
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            PlayerEquipmentHolder.OnPlayerEquipmentDisplayRequested?.Invoke();

            Time.timeScale = 0;
        }
    } 


    // ���� �κ��丮�� ������ �� �Լ��� ȣ���
    private void DisplayInventory(InventorySystem invToDisplay, int offset)
    {
        inventoryPanel.gameObject.SetActive(true);
        inventoryPanel.RefreshInventory(invToDisplay, offset);
    }

    private void DisplayPlayerInventory(InventorySystem invToDisplay, int offset)
    {
        playerPanel.gameObject.SetActive(true);
        playerPanel.RefreshInventory(invToDisplay, offset);
    }

    private void DisplayPlayerEquipment()
    {
        equipmentPanel.gameObject.SetActive(true);
    }
}


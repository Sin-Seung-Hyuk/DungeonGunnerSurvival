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
    public PlayerEquipmentHolder equipmentPanel; // �κ��丮 UI (���)

    private bool isTogleEquipment = false;

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
        // TAB Ű�� ���â Ȱ��ȭ
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            DisplayPlayerEquipment();
            isTogleEquipment = !isTogleEquipment;
        }
    } 


    // ���� �κ��丮�� ������ �� �Լ��� ȣ���
    private void DisplayInventory(InventorySystem invToDisplay, int offset)
    {
        if (!inventoryPanel.gameObject.activeSelf)
        {
            inventoryPanel.gameObject.SetActive(true);
            inventoryPanel.RefreshInventory(invToDisplay, offset);
        }
        else inventoryPanel.gameObject.SetActive(false);
    }

    private void DisplayPlayerInventory(InventorySystem invToDisplay, int offset)
    {
        if (!playerPanel.gameObject.activeSelf)
        {
            playerPanel.gameObject.SetActive(true);
            playerPanel.RefreshInventory(invToDisplay, offset);
        }
        else playerPanel.gameObject.SetActive(false);
    }

    private void DisplayPlayerEquipment()
    {
        if (isTogleEquipment)
        {
            equipmentPanel.gameObject.SetActive(false);

            Time.timeScale = 1;
        } else if (!isTogleEquipment)
        {
            equipmentPanel.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
}


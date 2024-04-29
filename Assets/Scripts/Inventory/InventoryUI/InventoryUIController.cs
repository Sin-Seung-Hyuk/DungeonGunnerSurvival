using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
    // 인벤토리 UI를 묶어서 관리하는 컨트롤러
    // 창고 같이 특정 상황에서만 열리는 인벤토리를 가지고 있다.

    [FormerlySerializedAs("chestPanel")] // 인스펙터의 값 유지 (리팩토링해도 유지)
    public DynamicInventoryDisplay inventoryPanel; // 동적인벤토리 UI (창고)
    public DynamicInventoryDisplay playerPanel; // 동적인벤토리 UI (플레이어)
    public PlayerEquipmentHolder equipmentPanel; // 인벤토리 UI (장비)

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
        // TAB 키로 장비창 활성화
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            DisplayPlayerEquipment();
            isTogleEquipment = !isTogleEquipment;
        }

        // 계속 일시정지 활성화시키기
        if (isTogleEquipment)
        {
            Time.timeScale = 0;
        }
    } 


    // 동적 인벤토리가 열리면 이 함수가 호출됨
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
        }
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour // 최상위 UI 컨트롤러
{
    [SerializeField] private ShopKeeperDisplay shopKeeperDisplay; // 상점UI


    private void Awake()
    {
        shopKeeperDisplay.gameObject.SetActive(false); // 상점UI off
    }
    private void OnEnable()
    {
        ShopKeeper.OnShopWindowRequested += DisplayShopWindow;
    }   
    private void OnDisable()
    {
        ShopKeeper.OnShopWindowRequested -= DisplayShopWindow;
    }
    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            shopKeeperDisplay.gameObject.SetActive(false);
    }

    private void DisplayShopWindow(ShopSystem shopSystem, PlayerInventoryHolder playerInventory)
    {
        shopKeeperDisplay.gameObject.SetActive(true); // 상점UI on
        shopKeeperDisplay.DisplayShopWindow(shopSystem, playerInventory);
    }
}

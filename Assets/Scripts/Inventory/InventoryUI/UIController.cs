using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour // �ֻ��� UI ��Ʈ�ѷ�
{
    [SerializeField] private ShopKeeperDisplay shopKeeperDisplay; // ����UI


    private void Awake()
    {
        shopKeeperDisplay.gameObject.SetActive(false); // ����UI off
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
        shopKeeperDisplay.gameObject.SetActive(true); // ����UI on
        shopKeeperDisplay.DisplayShopWindow(shopSystem, playerInventory);
    }
}

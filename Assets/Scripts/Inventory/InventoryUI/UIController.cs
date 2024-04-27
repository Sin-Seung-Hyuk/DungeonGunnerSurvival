using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour // �ֻ��� UI ��Ʈ�ѷ�
{
    [SerializeField] private ShopKeeperDisplay shopKeeperDisplay; // ����UI

    [SerializeField] private TextMeshProUGUI TxtTimer;      // ���� Ÿ�̸�
    [SerializeField] private TextMeshProUGUI TxtRoomName;   // ���� �̸�

    [SerializeField] private GameObject gameOverUI;   // ���� ����,Ŭ����� ������ UI
    [SerializeField] private AddWeaponUI addWeaponUI;   // �÷��̾� ���� �߰� UI

    private float timer;


    private void Awake()
    {
        shopKeeperDisplay.gameObject.SetActive(false); // ����UI off
    }
    private void OnEnable()
    {
        ShopKeeper.OnShopWindowRequested += DisplayShopWindow;

        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;
    }  
    private void OnDisable()
    {
        ShopKeeper.OnShopWindowRequested -= DisplayShopWindow;

        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;
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


    // ================================ ���� Ÿ�̸� ���� ======================================
    #region TimerUI
    private void StaticEventHandler_OnRoomChanged(RoomChangedArgs args)
    {
        if (args.room.isEntrance)
            TxtTimer.text = "";

        else {
            timer = Settings.dungeonTimer;
            StartCoroutine(TimerRoutine());
        }

        TxtRoomName.text = args.room.roomTemplate.roomName;
    }

    private IEnumerator TimerRoutine()
    {
        while (timer >= 0f)
        {
            if (timer <= 10f)
            {
                TxtTimer.color = Settings.red;
                TxtTimer.fontSize = 15;
            }

            timer -= Time.deltaTime;
            TxtTimer.text = timer.ToString("###0");

            yield return null;
        }

        // ���� Ÿ�Ӿƿ� �̺�Ʈ ȣ�� (UI���� ���ӸŴ����� ���� �� ������ ������ �̺�Ʈ�� ȣ���ϰ� ����)
        StaticEventHandler.CallRoomTimeoutEvent(GameManager.Instance.GetCurrentRoom());
    }
    #endregion

    // ======================= ���ӿ��� UI ���� =======================================
    #region GameOverUI
    public void SetGameOverUI()
    {
        gameOverUI.gameObject.SetActive(true);
    }
    #endregion

    // ======================= �����߰� UI ���� =======================================
    #region AddWeaponUI
    public void SetAddWeaponUI(WeaponDetailsSO weaponDetail, bool isActive)
    {
        addWeaponUI.gameObject.SetActive(isActive);

        if (isActive)
            addWeaponUI.InitializeAddWeaponUI(weaponDetail);
    }
    #endregion
}
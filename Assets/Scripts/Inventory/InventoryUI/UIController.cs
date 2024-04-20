using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour // 최상위 UI 컨트롤러
{
    [SerializeField] private ShopKeeperDisplay shopKeeperDisplay; // 상점UI

    [SerializeField] private TextMeshProUGUI TxtTimer;      // 던전 타이머
    [SerializeField] private TextMeshProUGUI TxtRoomName;   // 던전 이름

    private float timer = 300;


    private void Awake()
    {
        shopKeeperDisplay.gameObject.SetActive(false); // 상점UI off
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
        shopKeeperDisplay.gameObject.SetActive(true); // 상점UI on
        shopKeeperDisplay.DisplayShopWindow(shopSystem, playerInventory);
    }

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
                TxtTimer.color = new Color32(154, 37, 37, 255);
                TxtTimer.fontSize = 15;
            }

            timer -= Time.deltaTime;
            TxtTimer.text = timer.ToString("###0");

            yield return null;
        }

        // 던전 타임아웃 이벤트 호출 (UI에서 게임매니저의 현재 방 정보를 가져와 이벤트만 호출하고 종료)
        StaticEventHandler.CallRoomTimeoutEvent(GameManager.Instance.GetCurrentRoom());
    }
}
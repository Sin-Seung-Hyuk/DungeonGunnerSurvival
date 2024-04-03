using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System;

public class MouseItemData : MonoBehaviour  // 마우스로 슬롯UI 클릭하여 아이템 가져오기
{
    // 인스펙터에서 스프라이트,텍스트 설정
    public Image ItemSprite;
    public TextMeshProUGUI ItemCount;

    // 이 마우스아이템 데이터가 가질 슬롯 (슬롯이 가진 정보를 마우스데이터에 넣기)
    public InventorySlot AssignedInventorySlot;


    private void Awake()
    {
        ItemSprite.color = Color.clear;
        ItemCount.text = "";
    }

    public void UpdateMouseSlot(InventorySlot invSlot) // 이 슬롯으로 마우스아이템데이터 업데이트
    {
        // 클릭한 슬롯의 슬롯데이터 가져오기
        AssignedInventorySlot.AssignItem(invSlot);
        ItemSprite.sprite = invSlot.ItemData.Icon;
        ItemCount.text = invSlot.StackSize.ToString();
        ItemSprite.color = Color.white;
    }

    private void Update()
    {
        if (AssignedInventorySlot.ItemData != null)
        {   // InputSystem의 Mouse 클래스로 위치 가져오기
            transform.position = Mouse.current.position.ReadValue();

            // InputSystem의 Mouse 클래스에서 왼쪽마우스 클릭했는지 검사
            if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
            {
                ClearSlot(); // 마우스데이터 삭제 (아이템 삭제)
            }
        }
    }

    public void ClearSlot()
    {
        AssignedInventorySlot.ClearSlot();
        ItemCount.text = "";
        ItemSprite.sprite = null;
        ItemSprite.color = Color.clear;
    }

    public static bool IsPointerOverUIObject()
    {   // 현재 마우스 포인터 위치에 레이를 쏴서 부딪힌 것들을 rsults 리스트에 반환
        // 리스트 크기를 통해 포인터 위에 다른 UI객체가 있는지 확인할 수 있음

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
}

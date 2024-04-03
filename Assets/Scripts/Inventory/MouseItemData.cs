using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System;

public class MouseItemData : MonoBehaviour  // ���콺�� ����UI Ŭ���Ͽ� ������ ��������
{
    // �ν����Ϳ��� ��������Ʈ,�ؽ�Ʈ ����
    public Image ItemSprite;
    public TextMeshProUGUI ItemCount;

    // �� ���콺������ �����Ͱ� ���� ���� (������ ���� ������ ���콺�����Ϳ� �ֱ�)
    public InventorySlot AssignedInventorySlot;


    private void Awake()
    {
        ItemSprite.color = Color.clear;
        ItemCount.text = "";
    }

    public void UpdateMouseSlot(InventorySlot invSlot) // �� �������� ���콺�����۵����� ������Ʈ
    {
        // Ŭ���� ������ ���Ե����� ��������
        AssignedInventorySlot.AssignItem(invSlot);
        ItemSprite.sprite = invSlot.ItemData.Icon;
        ItemCount.text = invSlot.StackSize.ToString();
        ItemSprite.color = Color.white;
    }

    private void Update()
    {
        if (AssignedInventorySlot.ItemData != null)
        {   // InputSystem�� Mouse Ŭ������ ��ġ ��������
            transform.position = Mouse.current.position.ReadValue();

            // InputSystem�� Mouse Ŭ�������� ���ʸ��콺 Ŭ���ߴ��� �˻�
            if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
            {
                ClearSlot(); // ���콺������ ���� (������ ����)
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
    {   // ���� ���콺 ������ ��ġ�� ���̸� ���� �ε��� �͵��� rsults ����Ʈ�� ��ȯ
        // ����Ʈ ũ�⸦ ���� ������ ���� �ٸ� UI��ü�� �ִ��� Ȯ���� �� ����

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot_UI : InventorySlot_UI // ���� ����UI ���
{
    [SerializeField] private EquipmentType slotType; // �� ��񽽷��� ���Ÿ��
    [SerializeField] private Button BtnSlot;


    public override void OnUIISlotClick() // ����UI Ŭ���Լ� �������̵�
    {
        // �θ������Ʈ�� Ŭ���̺�Ʈ�� �� ������ �Ѱ���

        ParentDisplay?.SlotClicked(this, slotType);
    }

    // ��� ����
    public void EquipItem(List<PlayerStatChangeList> playerStatChangeList)
    {
        foreach (var statChangeList in playerStatChangeList)
        {
            GameManager.Instance.GetPlayer().playerStatChangedEvent
                .CallPlayerStatChangedEvent(statChangeList.statType, statChangeList.changeValue);
        }

        // ������ �������� ��޻������� ��񽽷� ���� ����
        SetBtnColor(AssignedInventorySlot.ItemData.gradeColor); 
    }

    // ��� ����
    public void UnequipItem(List<PlayerStatChangeList> playerStatChangeList)
    {
        foreach (var statChangeList in playerStatChangeList)
        {
            GameManager.Instance.GetPlayer().playerStatChangedEvent
                .CallPlayerStatChangedEvent(statChangeList.statType, -statChangeList.changeValue);
        }
        SetBtnColor(Color.white);
    }

    private void SetBtnColor(Color color)
    {
        ColorBlock btnColor = BtnSlot.colors;
        btnColor.normalColor = color;
        btnColor.highlightedColor = color;
        btnColor.selectedColor = color;
        btnColor.pressedColor = color;
        BtnSlot.colors = btnColor;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot_UI : InventorySlot_UI // 기존 슬롯UI 상속
{
    [SerializeField] private EquipmentType slotType; // 이 장비슬롯의 장비타입
    [SerializeField] private Button BtnSlot;


    public override void OnUIISlotClick() // 슬롯UI 클릭함수 오버라이딩
    {
        // 부모오브젝트에 클릭이벤트로 이 슬롯을 넘겨줌

        ParentDisplay?.SlotClicked(this, slotType);
    }

    // 장비 착용
    public void EquipItem(List<PlayerStatChangeList> playerStatChangeList)
    {
        foreach (var statChangeList in playerStatChangeList)
        {
            GameManager.Instance.GetPlayer().playerStatChangedEvent
                .CallPlayerStatChangedEvent(statChangeList.statType, statChangeList.changeValue);
        }

        // 장착한 아이템의 등급색상으로 장비슬롯 색상 설정
        SetBtnColor(AssignedInventorySlot.ItemData.gradeColor); 
    }

    // 장비 해제
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

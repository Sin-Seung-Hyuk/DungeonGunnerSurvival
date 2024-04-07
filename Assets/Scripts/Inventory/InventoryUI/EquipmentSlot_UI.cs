using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot_UI : InventorySlot_UI // 기존 슬롯UI 상속
{
    [SerializeField] private EquipmentType slotType; // 이 장비슬롯의 장비타입

    public override void OnUIISlotClick() // 슬롯UI 클릭함수 오버라이딩
    {
        // 부모오브젝트에 클릭이벤트로 이 슬롯을 넘겨줌

        ParentDisplay?.SlotClicked(this, slotType);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot_UI : InventorySlot_UI // ���� ����UI ���
{
    [SerializeField] private EquipmentType slotType; // �� ��񽽷��� ���Ÿ��

    public override void OnUIISlotClick() // ����UI Ŭ���Լ� �������̵�
    {
        // �θ������Ʈ�� Ŭ���̺�Ʈ�� �� ������ �Ѱ���

        ParentDisplay?.SlotClicked(this, slotType);
    }
}

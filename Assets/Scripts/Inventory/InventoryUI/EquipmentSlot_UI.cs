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

    // ��� ����
    public void EquipItem(List<PlayerStatChangeList> playerStatChangeList)
    {
        foreach (var statChangeList in playerStatChangeList)
        {
            GameManager.Instance.GetPlayer().playerStatChangedEvent
                .CallPlayerStatChangedEvent(statChangeList.statType, statChangeList.changeValue);
        }
    }

    // ��� ����
    public void UnequipItem(List<PlayerStatChangeList> playerStatChangeList)
    {
        foreach (var statChangeList in playerStatChangeList)
        {
            GameManager.Instance.GetPlayer().playerStatChangedEvent
                .CallPlayerStatChangedEvent(statChangeList.statType, -statChangeList.changeValue);
        }
    }
}

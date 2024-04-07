using System.Collections;
using System.Collections.Generic;
using UnityEngine;

                        // ����ȭ,������ȭ�� ���� �������̽�
public class ItemSlot : ISerializationCallbackReceiver
{
    [SerializeField] protected InventoryItemData itemData; // ���Կ� �� �����۵�����
    [SerializeField] protected int stackSize; // �� ������ ���罺��
    [SerializeField] protected int itemID = -1; // �� ���Կ� �� ������ID
    

    public InventoryItemData ItemData => itemData; 
    public int StackSize => stackSize;

    public void ClearSlot() // ���� ����
    {
        itemData = null;
        stackSize = -1;
        itemID = -1;
    }

    // �� ������ �Ű������� ���� �������� �ʱ�ȭ
    public void AssignItem(InventorySlot invSlot)
    {
        if (itemData == invSlot.ItemData) AddToStack(invSlot.stackSize); // ���� �������̸� ���ýױ�
        else
        {
            itemData = invSlot.itemData;
            itemID = itemData.ID;
            stackSize = 0;
            AddToStack(invSlot.stackSize);
        }
    }
    // �� ������ �Ű������� ���� �����۵�����,������ �ʱ�ȭ
    public void AssignItem(InventoryItemData data, int amount)
    {
        if (itemData == data) AddToStack(stackSize); // ���� �������̸� ���ýױ�
        else
        {
            itemData = data;
            itemID = data.ID;
            stackSize = 0;
            AddToStack(amount);
        }
    }


    // ������ ������ŭ ���ÿ� �ױ� =======================================
    public void AddToStack(int amount)
    {
        stackSize += amount;
    }
    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
        if (stackSize <= 0) ClearSlot();
    }


    // ������ �����ͺ��̽����� �������� ID�� �´� ������ ��������
    public void OnAfterDeserialize()
    {
        if (itemID == -1) return;

        var db = Resources.Load<Database>("Database");
        itemData = db.GetItem(itemID);
    }

    public void OnBeforeSerialize()
    {

    }
}

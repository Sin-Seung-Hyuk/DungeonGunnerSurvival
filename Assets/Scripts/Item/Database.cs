using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Inventory System/Item Database")]
public class Database : ScriptableObject
{
    [SerializeField] private List<InventoryItemData> itemDatabase;

    [ContextMenu("Set ID")] // �ν����Ϳ��� ������ ��� ������ �Լ�
    public void SetItemID()
    {
        itemDatabase = new List<InventoryItemData>();

        // InventoryItemData ��ü ��� �о�� ID ������� �����Ͽ� ����Ʈ�� ��ȯ
        var foundItems = Resources.LoadAll<InventoryItemData>("ItemData").
            OrderBy(i => i.ID).ToList();

        // ID�� -1�� �ƴϰ� �������� �� ���� �̳��� ����ִ� ���
        var hasIDInRange = foundItems.Where(i => i.ID != -1 && i.ID < foundItems.Count)
            .OrderBy(i => i.ID).ToList();

        // ID�� -1�� �ƴ����� �������� �������� ID�� �� ū ���
        var hasIDNotInRange = foundItems.Where(i => i.ID != -1 && i.ID >= foundItems.Count)
            .OrderBy(i => i.ID).ToList();

        // ID�� �������� ���� ���
        var noID = foundItems.Where(i => i.ID <= -1).OrderBy(i => i.ID).ToList();

        var idx = 0;
        for (int i = 0; i < foundItems.Count; ++i)
        {
            InventoryItemData itemToAdd;
            itemToAdd = hasIDInRange.Find(data => data.ID == i); // �ش� ID�� ������ ã��

            if (itemToAdd != null)
                itemDatabase.Add(itemToAdd);
            else if (idx < noID.Count) // ID�� ���� �����ۿ� ID ����
            {
                noID[idx].ID = i;
                itemToAdd = noID[idx];
                idx++;
                itemDatabase.Add(itemToAdd);
            }
        }

        // ID�� ������ �������� ū ��� �������� �Ѳ����� �����ͺ��̽��� �߰�
        foreach (var item in hasIDNotInRange)
        {
            itemDatabase.Add(item);
        }
    }

    public InventoryItemData GetItem(int id) // �ش� id�� �����۵����� ��ȯ
    {
        return itemDatabase.Find(i => i.ID == id);
    }
}

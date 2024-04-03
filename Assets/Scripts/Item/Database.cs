using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Inventory System/Item Database")]
public class Database : ScriptableObject
{
    [SerializeField] private List<InventoryItemData> itemDatabase;

    [ContextMenu("Set ID")]
    public void SetItemID()
    {
        itemDatabase = new List<InventoryItemData>();

        // InventoryItemData 객체 모두 읽어와 ID 순서대로 정렬하여 리스트로 반환
        var foundItems = Resources.LoadAll<InventoryItemData>("ItemData").
            OrderBy(i=> i.ID).ToList();

        var hasIDInRange = foundItems.Where(i => i.ID != -1 && i.ID < foundItems.Count)
            .OrderBy(i => i.ID).ToList();

        var hasIDNotInRange = foundItems.Where(i => i.ID != -1 && i.ID >= foundItems.Count)
            .OrderBy(i => i.ID).ToList();

        var noID = foundItems.Where(i => i.ID <= -1).OrderBy(i => i.ID).ToList();

        var idx = 0;
        for (int i = 0; i < foundItems.Count; ++i)
        {
            InventoryItemData itemToAdd;
            itemToAdd = hasIDInRange.Find(data => data.ID == i);

            if (itemToAdd != null)
                itemDatabase.Add(itemToAdd);
            else if (idx < noID.Count)
            {
                noID[idx].ID = i;
                itemToAdd = noID[idx];
                idx++;
                itemDatabase.Add(itemToAdd);
            }
        }

        foreach (var item in hasIDNotInRange)
        {
            itemDatabase.Add(item);
        }
    }

    public InventoryItemData GetItem(int id)
    {
        return itemDatabase.Find(i => i.ID == id);
    }
}

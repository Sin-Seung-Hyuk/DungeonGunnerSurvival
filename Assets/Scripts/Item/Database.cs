using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Inventory System/Item Database")]
public class Database : ScriptableObject
{
    [SerializeField] private List<InventoryItemData> itemDatabase;

    [ContextMenu("Set ID")] // 인스펙터에서 빠르게 사용 가능한 함수
    public void SetItemID()
    {
        itemDatabase = new List<InventoryItemData>();

        // InventoryItemData 객체 모두 읽어와 ID 순서대로 정렬하여 리스트로 반환
        var foundItems = Resources.LoadAll<InventoryItemData>("ItemData").
            OrderBy(i => i.ID).ToList();

        // ID가 -1이 아니고 아이템의 총 개수 이내에 들어있는 경우
        var hasIDInRange = foundItems.Where(i => i.ID != -1 && i.ID < foundItems.Count)
            .OrderBy(i => i.ID).ToList();

        // ID가 -1이 아니지만 아이템의 개수보다 ID가 더 큰 경우
        var hasIDNotInRange = foundItems.Where(i => i.ID != -1 && i.ID >= foundItems.Count)
            .OrderBy(i => i.ID).ToList();

        // ID가 배정받지 못한 경우
        var noID = foundItems.Where(i => i.ID <= -1).OrderBy(i => i.ID).ToList();

        var idx = 0;
        for (int i = 0; i < foundItems.Count; ++i)
        {
            InventoryItemData itemToAdd;
            itemToAdd = hasIDInRange.Find(data => data.ID == i); // 해당 ID의 아이템 찾기

            if (itemToAdd != null)
                itemDatabase.Add(itemToAdd);
            else if (idx < noID.Count) // ID가 없는 아이템에 ID 배정
            {
                noID[idx].ID = i;
                itemToAdd = noID[idx];
                idx++;
                itemDatabase.Add(itemToAdd);
            }
        }

        // ID가 아이템 개수보다 큰 경우 마지막에 한꺼번에 데이터베이스에 추가
        foreach (var item in hasIDNotInRange)
        {
            itemDatabase.Add(item);
        }
    }

    public InventoryItemData GetItem(int id) // 해당 id의 아이템데이터 반환
    {
        return itemDatabase.Find(i => i.ID == id);
    }
}

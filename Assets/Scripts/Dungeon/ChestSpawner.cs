using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    public void SpawnChest()
    {
        Vector3Int cellPos = new Vector3Int(2,1,0);
        Instantiate(GameResources.Instance.chestPrefab, cellPos, Quaternion.identity);
    }
}

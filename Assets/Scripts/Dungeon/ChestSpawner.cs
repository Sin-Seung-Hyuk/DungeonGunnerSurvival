using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    [SerializeField] private List<WeaponDetailsSO> weaponList; // 무기보상 리스트

    public WeaponDetailsSO weaponDetail { get; private set; }

    public void SpawnChest()
    {
        Player player = GameManager.Instance.GetPlayer();

        while (true)
        {
            weaponDetail = weaponList[Random.Range(0, weaponList.Count)];
            bool hasWeapon = false; // 갖고있는 무기인지 검사

            foreach (var playerWeapon in player.weaponList) {
                if (weaponDetail == playerWeapon.weaponDetail) {
                    hasWeapon = true;
                    break; // 이미 가진무기라면 반복문 종료
                }
            }

            if (!hasWeapon) {
                break; // 가진 무기가 아니라면 반복문 종료
            }
        }

        Vector3Int cellPos = new Vector3Int(2,1,0);
        Instantiate(GameResources.Instance.chestPrefab, cellPos, Quaternion.identity);
    }
}

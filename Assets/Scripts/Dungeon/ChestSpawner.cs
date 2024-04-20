using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    [SerializeField] private List<WeaponDetailsSO> weaponList; // ���⺸�� ����Ʈ

    public WeaponDetailsSO weaponDetail { get; private set; }

    public void SpawnChest()
    {
        Player player = GameManager.Instance.GetPlayer();

        while (true)
        {
            weaponDetail = weaponList[Random.Range(0, weaponList.Count)];
            bool hasWeapon = false; // �����ִ� �������� �˻�

            foreach (var playerWeapon in player.weaponList) {
                if (weaponDetail == playerWeapon.weaponDetail) {
                    hasWeapon = true;
                    break; // �̹� ���������� �ݺ��� ����
                }
            }

            if (!hasWeapon) {
                break; // ���� ���Ⱑ �ƴ϶�� �ݺ��� ����
            }
        }

        Vector3Int cellPos = new Vector3Int(2,1,0);
        Instantiate(GameResources.Instance.chestPrefab, cellPos, Quaternion.identity);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveData  // ������ �����͸� ����ִ� ��ü SaveData
{
    public SerializableDictionary<string, InventorySaveData> chestDictionary; // ���� ������

    public InventorySaveData playerInventory; // �÷��̾� �κ��丮 ������ (�ϳ��� �����ϹǷ� ��ųʸ� X)
    public EquipmentSaveData playerEquipment; // �÷��̾� ��� ������ (�ϳ��� �����ϹǷ� ��ųʸ� X)

    public SaveData()
    {

        chestDictionary = new SerializableDictionary<string, InventorySaveData>();

        playerInventory = new InventorySaveData();
        playerEquipment = new EquipmentSaveData();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


                             // �κ��丮 Ȧ��, ��ȣ�ۿ� �������̽�
public class ChestInventory : InventoryHolder, IInteractable
{
    // �κ��丮 Ȧ���� ��ӹ޾����Ƿ� ���� �κ��丮 �ý��� ����
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }


    private void Start()
    {
        SaveLoad.OnSaveGame += SaveFile;
        LoadInventory(SaveGameManager.data);
    }

    private void OnDestroy() // â��� �ı��Ǹ鼭 ���� �� �������� (���̵�)
    {   
        SaveGameManager.SaveData();
        SaveLoad.OnSaveGame -= SaveFile;
    }

    private void SaveFile()
    {
        // ���� ������ ���� �����ϱ�
        var chestData = new InventorySaveData(primaryInventorySystem, transform.position, transform.rotation);

        if (!SaveGameManager.data.chestDictionary.ContainsKey(GetComponent<UniqueID>().ID))
            SaveGameManager.data.chestDictionary.Add(GetComponent<UniqueID>().ID, chestData);
        else
            SaveGameManager.data.chestDictionary[GetComponent<UniqueID>().ID] = chestData;
    }
    protected override void LoadInventory(SaveData data)
    {   // ���̺� ������ Ȯ���ϰ� ����� ������ ������ �κ��丮 �ʱ�ȭ

        // ���̺굥������ ���ڵ�ųʸ��� ID �� ������ TryGetValue�� Ȯ���ϱ�
        if (data.chestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out InventorySaveData chestData))
        {
            this.primaryInventorySystem = chestData.invSystem;
            this.transform.position = chestData.position;
            this.transform.rotation = chestData.rotation;
        }
    }

    // �� �κ��丮�� �ý����� �������� �����κ��丮 ��� �Լ� ȣ��
    public void Interact(Interactor interator, out bool interactSuccessful)
    {
        OnDynamicInventoryDisplayRequested?.Invoke(primaryInventorySystem,0);
        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.chestOpen);
        interactSuccessful = true;
        interator.SetInteracting();
    }

    public void EndInteraction()
    {

    }


}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


                             // 인벤토리 홀더, 상호작용 인터페이스
public class ChestInventory : InventoryHolder, IInteractable
{
    // 인벤토리 홀더를 상속받았으므로 고유 인벤토리 시스템 보유
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }


    private void Start()
    {
        SaveLoad.OnSaveGame += SaveFile;
        LoadInventory(SaveGameManager.data);
    }

    private void OnDestroy() // 창고는 파괴되면서 저장 후 구독해지 (맵이동)
    {   
        SaveGameManager.SaveData();
        SaveLoad.OnSaveGame -= SaveFile;
    }

    private void SaveFile()
    {
        // 현재 상자의 정보 저장하기
        var chestData = new InventorySaveData(primaryInventorySystem, transform.position, transform.rotation);

        if (!SaveGameManager.data.chestDictionary.ContainsKey(GetComponent<UniqueID>().ID))
            SaveGameManager.data.chestDictionary.Add(GetComponent<UniqueID>().ID, chestData);
        else
            SaveGameManager.data.chestDictionary[GetComponent<UniqueID>().ID] = chestData;
    }
    protected override void LoadInventory(SaveData data)
    {   // 세이브 데이터 확인하고 저장된 데이터 가져와 인벤토리 초기화

        // 세이브데이터의 상자딕셔너리의 ID 값 가져와 TryGetValue로 확인하기
        if (data.chestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out InventorySaveData chestData))
        {
            this.primaryInventorySystem = chestData.invSystem;
            this.transform.position = chestData.position;
            this.transform.rotation = chestData.rotation;
        }
    }

    // 이 인벤토리의 시스템을 바탕으로 동적인벤토리 출력 함수 호출
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


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class InventoryHolder : MonoBehaviour // 추상클래스
{
    [SerializeField] private int inventorySize; // 인벤토리 크기
    [SerializeField] protected InventorySystem primaryInventorySystem;
    [SerializeField] protected int offset = 10; // 퀵슬롯을 위한 오프셋 (매우중요)
    [SerializeField] protected int _gold;

    public int Offset => offset;

    public InventorySystem PrimaryInventorySystem => primaryInventorySystem;

    // 동적인벤토리(창고) 화면을 출력해야 하는 경우
    public static UnityAction<InventorySystem, int> OnDynamicInventoryDisplayRequested;

    protected virtual void Awake()
    {
        SaveLoad.OnLoadGame += LoadInventory; // 게임로드 이벤트에 구독

        primaryInventorySystem = new InventorySystem(inventorySize);
    }
    private void OnDisable()
    {
        SaveLoad.OnLoadGame -= LoadInventory;
    }

    protected abstract void LoadInventory(SaveData saveData); // 구독할 함수는 창고/플레이어 각자따로 구현
}


[System.Serializable]
public struct InventorySaveData  // 저장할 데이터 구조체
{
    public InventorySystem invSystem;
    public Vector3 position;
    public Quaternion rotation;

    public InventorySaveData(InventorySystem invSystem, Vector3 pos, Quaternion rot)
    {
        this.invSystem = invSystem;
        position = pos;
        rotation = rot;
    }

    // 플레이어는 인벤토리의 위치,각도가 필요없음
    public InventorySaveData(InventorySystem invSystem)
    {
        this.invSystem = invSystem;
        position = Vector3.zero;
        rotation = Quaternion.identity;
    }
}
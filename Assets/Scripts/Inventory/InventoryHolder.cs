using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class InventoryHolder : MonoBehaviour // �߻�Ŭ����
{
    [SerializeField] private int inventorySize; // �κ��丮 ũ��
    [SerializeField] protected InventorySystem primaryInventorySystem;
    [SerializeField] protected int offset = 10; // �������� ���� ������ (�ſ��߿�)
    [SerializeField] protected int _gold;

    public int Offset => offset;

    public InventorySystem PrimaryInventorySystem => primaryInventorySystem;

    // �����κ��丮(â��) ȭ���� ����ؾ� �ϴ� ���
    public static UnityAction<InventorySystem, int> OnDynamicInventoryDisplayRequested;

    protected virtual void Awake()
    {
        SaveLoad.OnLoadGame += LoadInventory; // ���ӷε� �̺�Ʈ�� ����

        primaryInventorySystem = new InventorySystem(inventorySize);
    }
    private void OnDisable()
    {
        SaveLoad.OnLoadGame -= LoadInventory;
    }

    protected abstract void LoadInventory(SaveData saveData); // ������ �Լ��� â��/�÷��̾� ���ڵ��� ����
}


[System.Serializable]
public struct InventorySaveData  // ������ ������ ����ü
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

    // �÷��̾�� �κ��丮�� ��ġ,������ �ʿ����
    public InventorySaveData(InventorySystem invSystem)
    {
        this.invSystem = invSystem;
        position = Vector3.zero;
        rotation = Quaternion.identity;
    }
}
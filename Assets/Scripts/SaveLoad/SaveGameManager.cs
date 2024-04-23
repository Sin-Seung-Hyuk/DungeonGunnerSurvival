using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameManager : MonoBehaviour // ���̺� �Ŵ���
{
    public static SaveData data; // ������ �����ϴ� ���̺굥���� ��ü

    private void OnEnable()
    {
        data = new SaveData(); // ���� ���̺굥���� ����

        SaveLoad.OnLoadGame += LoadData;
    }

    private void Start()
    {
        TryLoadData(); // �����ϸ� ���̺� �ε�
    }

    private void OnDisable() // ���� ������ �ڵ�ȣ��
    {
        SaveData();
    }

    public void DeleteData()
    {
        SaveLoad.DeleteSaveData();
    }

    public static void SaveData()
    {
        SaveData saveData = data;

        SaveLoad.Save(saveData);
    }

    public static void TryLoadData()
    {
        SaveLoad.Load();
    }

    private void LoadData(SaveData _data)
    {
        data = _data;
    }
}

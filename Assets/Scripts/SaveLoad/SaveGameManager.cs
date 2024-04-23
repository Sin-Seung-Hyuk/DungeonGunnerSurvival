using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameManager : MonoBehaviour // 세이브 매니저
{
    public static SaveData data; // 전역에 존재하는 세이브데이터 객체

    private void OnEnable()
    {
        data = new SaveData(); // 정적 세이브데이터 생성

        SaveLoad.OnLoadGame += LoadData;
    }

    private void Start()
    {
        TryLoadData(); // 시작하면 세이브 로드
    }

    private void OnDisable() // 게임 꺼질때 자동호출
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

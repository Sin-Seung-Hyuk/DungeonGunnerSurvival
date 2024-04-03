using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public static class SaveLoad // 저장,로드 기능이 구현되어있는 스태틱 클래스
{
    public static UnityAction OnSaveGame; // 데이터 저장
    public static UnityAction<SaveData> OnLoadGame; // SaveData에서 ID 비교하여 데이터 로드

    private static string directory = "/DungeonGunnerSaveData/"; // 저장 위치
    private static string fileName = "SaveGame.sav"; // 저장 이름


    public static bool Save(SaveData data)
    {
        OnSaveGame?.Invoke(); // 저장 이벤트 호출

        string dir = Application.persistentDataPath + directory;

        if (!Directory.Exists(dir)) // 해당 경로가 없으면 폴더생성
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(dir + fileName, json); // SaveData를 JSon 문자열 파일로 저장

        return true;
    }

    public static SaveData Load()
    {   // 저장해놓은 위치
        string fullPath = Application.persistentDataPath + directory + fileName;

        SaveData data = new SaveData();

        if (File.Exists(fullPath))  // 해당 경로로 이동
        {
            string json = File.ReadAllText(fullPath);
            data = JsonUtility.FromJson<SaveData>(json); // JSon 파일을 읽어서 SaveData 타입으로 반환
        }
        else Debug.Log("세이브 파일 없음");

        OnLoadGame?.Invoke(data); // 로드게임 이벤트 호출

        return data; // data(세이브)가 존재하면 트루
    }

    public static void DeleteSaveData()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;

        if (File.Exists(fullPath)) File.Delete(fullPath); // 해당 경로의 파일 삭제
    }
}

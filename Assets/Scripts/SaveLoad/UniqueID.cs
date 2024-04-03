using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[ExecuteInEditMode]
public class UniqueID : MonoBehaviour
{
    [ReadOnly] // 이 오브젝트의 고유한 ID 받아오기
    [SerializeField] private string id = Guid.NewGuid().ToString();

    [SerializeField]  // 저장,로드를 위한 아이템데이터베이스 딕셔너리 생성
    private static SerializableDictionary<string, GameObject> idDataBase =
        new SerializableDictionary<string, GameObject>();

    public string ID => id;

    private void OnValidate() // 이 객체 생성되면 데이터베이스에 ID 추가
    {
        if (idDataBase.ContainsKey(id)) Generate();
        else idDataBase.Add(id, this.gameObject);
    }

    private void OnDestroy() // 이 객체 파괴되면 데이터베이스에 ID 제거
    {
        if (idDataBase.ContainsKey(id))
            idDataBase.Remove(id);
    }

    private void Generate()
    {
        id = Guid.NewGuid().ToString();
        idDataBase.Add(id, this.gameObject);
    }
}

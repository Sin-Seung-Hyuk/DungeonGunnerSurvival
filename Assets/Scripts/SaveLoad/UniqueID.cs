using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[ExecuteInEditMode]
public class UniqueID : MonoBehaviour
{
    [ReadOnly] // �� ������Ʈ�� ������ ID �޾ƿ���
    [SerializeField] private string id = Guid.NewGuid().ToString();

    [SerializeField]  // ����,�ε带 ���� �����۵����ͺ��̽� ��ųʸ� ����
    private static SerializableDictionary<string, GameObject> idDataBase =
        new SerializableDictionary<string, GameObject>();

    public string ID => id;

    private void OnValidate() // �� ��ü �����Ǹ� �����ͺ��̽��� ID �߰�
    {
        if (idDataBase.ContainsKey(id)) Generate();
        else idDataBase.Add(id, this.gameObject);
    }

    private void OnDestroy() // �� ��ü �ı��Ǹ� �����ͺ��̽��� ID ����
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

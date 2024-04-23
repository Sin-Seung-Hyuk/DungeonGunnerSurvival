using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    // ���� Pool�� ����Ʈ�� ����
    [SerializeField] private Pool[] poolArray = null;
    private Dictionary<int, Queue<Component>> poolDictionary = new Dictionary<int, Queue<Component>>();
    private Transform objPoolTransform;

    [System.Serializable]
    public struct Pool
    {
        public int poolSize; // Ǯ�� ũ��
        public GameObject prefab; // ������ ������
        public string componentType; // ������Ʈ Ÿ��
    }

    private void Start()
    {
        objPoolTransform = this.gameObject.transform;

        for (int i=0; i< poolArray.Length; ++i)
        {   // ���� ���۽� �̸� Ǯ ����
            CreatePool(poolArray[i].prefab, poolArray[i].poolSize, poolArray[i].componentType);
        }
    }

    private void CreatePool(GameObject prefab, int poolSize, string componentType)
    {
        int poolKey = prefab.GetInstanceID(); // �������� ���� ID

        GameObject poolContainer = new GameObject(prefab.name);
        poolContainer.transform.SetParent(objPoolTransform);

        if (!poolDictionary.TryGetValue(poolKey, out _))
        {
            poolDictionary.Add(poolKey, new Queue<Component>()); // Ǯ ���

            for (int i = 0; i < poolSize; ++i)
            {
                GameObject newObj = Instantiate(prefab, poolContainer.transform); // as GameObject;

                newObj.SetActive(false);

                poolDictionary[poolKey].Enqueue(newObj.GetComponent(Type.GetType(componentType)));
            }
        }
    }


    public Component Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();

        if (poolDictionary.TryGetValue(poolKey, out _))
        {
            // �ش� Ǯ�� ť���� ���� ��������
            Component componentToReuse = poolDictionary[poolKey].Dequeue();
            poolDictionary[poolKey].Enqueue(componentToReuse);

            if (componentToReuse.gameObject.activeSelf == true)
            {
                GameObject newObj = Instantiate(prefab, componentToReuse.gameObject.transform.parent);
                newObj.transform.position = position;
                newObj.transform.rotation = rotation;
                componentToReuse = newObj.GetComponent(componentToReuse.GetType());
            }

            // ������ ������Ʈ�� �Ӽ� �ʱ�ȭ
            ResetObject(position, rotation, componentToReuse, prefab);

            return componentToReuse;
        }
        else return null;
    }

    private void ResetObject(Vector3 position, Quaternion rotation, Component componentToReuse, GameObject prefab)
    {
        componentToReuse.transform.position = position;
        componentToReuse.transform.rotation = rotation;
        componentToReuse.gameObject.transform.localScale = prefab.transform.localScale;
    }
}



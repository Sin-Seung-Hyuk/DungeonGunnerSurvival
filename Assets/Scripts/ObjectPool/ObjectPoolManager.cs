using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    // 여러 Pool을 리스트로 관리
    [SerializeField] private Pool[] poolArray = null;
    private Dictionary<int, Queue<Component>> poolDictionary = new Dictionary<int, Queue<Component>>();
    private Transform objPoolTransform;

    [System.Serializable]
    public struct Pool
    {
        public int poolSize; // 풀의 크기
        public GameObject prefab; // 생성할 프리팹
        public string componentType; // 컴포넌트 타입
    }

    private void Start()
    {
        objPoolTransform = this.gameObject.transform;

        for (int i=0; i< poolArray.Length; ++i)
        {   // 게임 시작시 미리 풀 생성
            CreatePool(poolArray[i].prefab, poolArray[i].poolSize, poolArray[i].componentType);
        }
    }

    private void CreatePool(GameObject prefab, int poolSize, string componentType)
    {
        int poolKey = prefab.GetInstanceID(); // 프리팹의 고유 ID

        GameObject poolContainer = new GameObject(prefab.name);
        poolContainer.transform.SetParent(objPoolTransform);

        if (!poolDictionary.TryGetValue(poolKey,out _))
        {
            poolDictionary.Add(poolKey, new Queue<Component>()); // 풀 등록

            for (int i=0;i<poolSize; ++i)
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
            // 해당 풀의 큐에서 새거 꺼내오기
            Component componentToReuse = GetComponentFromPool(poolKey);

            // 생성될 오브젝트의 속성 초기화
            ResetObject(position, rotation, componentToReuse, prefab);

            return componentToReuse;
        }
        else return null;
    }

    private Component GetComponentFromPool(int poolKey)
    {
        // 컴포넌트 큐에 집어넣고 새로 꺼내오기
        Component componentToReuse = poolDictionary[poolKey].Dequeue();
        poolDictionary[poolKey].Enqueue(componentToReuse);

        if (componentToReuse.gameObject.activeSelf == true)
        {
            componentToReuse.gameObject.SetActive(false); 
        }

        return componentToReuse;
    }

    private void ResetObject(Vector3 position, Quaternion rotation, Component componentToReuse, GameObject prefab)
    {
        componentToReuse.transform.position = position;
        componentToReuse.transform.rotation = rotation;
        componentToReuse.gameObject.transform.localScale = prefab.transform.localScale;
    }
}

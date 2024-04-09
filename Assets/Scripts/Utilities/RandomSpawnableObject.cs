using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnableObject<T> {
    private struct chanceBoundaries // 랜덤으로 생성할 오브젝트의 확률범위 설정
    {
        public T spawnableObject;
        public int lowBoundaryValue;
        public int highBoundaryValue;
    }
    public int ratioValueTotal { get; private set; } // 확률 범위의 최대값
    private List<chanceBoundaries> chanceBoundariesList = new List<chanceBoundaries>();


    // 랜덤 스폰 클래스 생성자 (랜덤으로 스폰할 리스트 받아오기)
    public RandomSpawnableObject(List<SpawnableObjectRatio<T>> _spawnableObjectList)
    {
        int upper = -1;

        foreach (SpawnableObjectRatio<T> spawnable in _spawnableObjectList)
        {
            int low = upper + 1; // 이 오브젝트의 확률 최소값
            upper = low + spawnable.ratio - 1; // 이 오브젝트의 확률 최대값
            ratioValueTotal += spawnable.ratio; // 확률의 최대범위

            chanceBoundariesList.Add(new chanceBoundaries()
            {
                spawnableObject = spawnable.dungeonObject,
                lowBoundaryValue = low,
                highBoundaryValue = upper
            });
        }
    }

    public T GetItem(int chance) // 매개변수로 들어온 확률에 맞는 오브젝트 반환
    {
        foreach (chanceBoundaries spawnChance in chanceBoundariesList)
        {       
            if (chance >= spawnChance.lowBoundaryValue && chance <= spawnChance.highBoundaryValue)
            {
                // 매개변수로 들어온 수가 해당 오브젝트의 확률 범위에 들어왔다면 반환
                return spawnChance.spawnableObject;
            }
        }

        return default(T); // 아무런 아이템도 반환되지 않음. null이 없을수도 있으므로 default(T)
    }
}

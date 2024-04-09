using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnableObject<T> {
    private struct chanceBoundaries // �������� ������ ������Ʈ�� Ȯ������ ����
    {
        public T spawnableObject;
        public int lowBoundaryValue;
        public int highBoundaryValue;
    }
    public int ratioValueTotal { get; private set; } // Ȯ�� ������ �ִ밪
    private List<chanceBoundaries> chanceBoundariesList = new List<chanceBoundaries>();


    // ���� ���� Ŭ���� ������ (�������� ������ ����Ʈ �޾ƿ���)
    public RandomSpawnableObject(List<SpawnableObjectRatio<T>> _spawnableObjectList)
    {
        int upper = -1;

        foreach (SpawnableObjectRatio<T> spawnable in _spawnableObjectList)
        {
            int low = upper + 1; // �� ������Ʈ�� Ȯ�� �ּҰ�
            upper = low + spawnable.ratio - 1; // �� ������Ʈ�� Ȯ�� �ִ밪
            ratioValueTotal += spawnable.ratio; // Ȯ���� �ִ����

            chanceBoundariesList.Add(new chanceBoundaries()
            {
                spawnableObject = spawnable.dungeonObject,
                lowBoundaryValue = low,
                highBoundaryValue = upper
            });
        }
    }

    public T GetItem(int chance) // �Ű������� ���� Ȯ���� �´� ������Ʈ ��ȯ
    {
        foreach (chanceBoundaries spawnChance in chanceBoundariesList)
        {       
            if (chance >= spawnChance.lowBoundaryValue && chance <= spawnChance.highBoundaryValue)
            {
                // �Ű������� ���� ���� �ش� ������Ʈ�� Ȯ�� ������ ���Դٸ� ��ȯ
                return spawnChance.spawnableObject;
            }
        }

        return default(T); // �ƹ��� �����۵� ��ȯ���� ����. null�� �������� �����Ƿ� default(T)
    }
}

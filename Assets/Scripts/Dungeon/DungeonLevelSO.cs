using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonLevel_", menuName = "Scriptable Objects/Dungeon/Dungeon Level")]
public class DungeonLevelSO : ScriptableObject
{
    public string levelName; // 던전 이름
    public int level; // 레벨 몇인지

    // 레벨별 생성될 방 템플릿
    public List<RoomTemplateSO> roomTemplateList; 
}


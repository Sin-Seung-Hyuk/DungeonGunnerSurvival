using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player_", menuName = "Scriptable Objects/Player/Player")]
public class PlayerDetailsSO : ScriptableObject
{
    public GameObject playerPrefab;

    public string characterName; // 플레이어가 아닌 캐릭터 이름
    public RuntimeAnimatorController runtimeAnimatorController;
    public Sprite minimapIcon;
    public Sprite playerSprite;

    [Space(10)]
    [Header("Character Stat")]
    [Range(80, 150)] public int maxHp;
    [Range(10, 100)] public int baseDamage;
    [Range(0, 100)] public int criticChance;
    [Range(100, 200)] public int criticDamage;
    [Range(0, 100)] public int reloadSpeed;
    [Range(0, 100)] public int fireRateSpeed;
    [Range(5f, 20f)] public float moveSpeed;
    [Range(0.8f, 5f)] public float circleRange; // 자석 범위
    [Range(0, 100)] public int dodgeChance;
    [Range(0, 100)] public int expGain;

}
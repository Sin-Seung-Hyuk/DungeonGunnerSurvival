using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStat
{
    public int baseDamage    {get;set;}
    public int reloadSpeed   {get;set;}
    public int fireRateSpeed {get;set;}
    public float moveSpeed   {get;set;}
    public float circleRange {get;set;}
    public int dodgeChance   {get;set;}
    public int expGain       { get; set; }
}

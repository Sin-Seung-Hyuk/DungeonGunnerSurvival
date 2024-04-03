using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrentPlayer_", menuName = "Scriptable Objects/Player/CurrentPlayer")]
public class CurrentPlayerSO : ScriptableObject
{
    public PlayerDetailsSO currentPlayerDetailsSO;
    public string playerName;
}

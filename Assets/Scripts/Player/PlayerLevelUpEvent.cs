using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelUpEvent : MonoBehaviour
{
    public event Action<PlayerLevelUpEvent> OnPlayerLevelUp;

    public void CallPlayerLevelUpEvent()
    {
        OnPlayerLevelUp?.Invoke(this);
    }
}

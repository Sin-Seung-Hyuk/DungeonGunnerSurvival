using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpBarUI : MonoBehaviour
{
    [SerializeField] private Slider expBar;
    [SerializeField] private TextMeshProUGUI TxtLevel;

    public void SetExpBar(float ratio)
    {
        expBar.value = ratio;
    }

    public void SetLevel(int level)
    {
        TxtLevel.text = level.ToString();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    

public class ShoppingCartItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemText;

    // 쇼핑카트에 담긴 아이템 리스트 텍스트
    internal void SetItemText(string newString)
    {
        itemText.text = newString;
    }
}

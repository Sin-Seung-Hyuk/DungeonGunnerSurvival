using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    

public class ShoppingCartItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemText;

    // ����īƮ�� ��� ������ ����Ʈ �ؽ�Ʈ
    internal void SetItemText(string newString)
    {
        itemText.text = newString;
    }
}

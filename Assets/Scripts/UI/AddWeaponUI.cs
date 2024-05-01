using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddWeaponUI : MonoBehaviour
{
    [SerializeField] private Image weaponImage; // 추가될 무기의 이미지
    [SerializeField] private TextMeshProUGUI TxtWeaponName; // 무기이름


    public void InitializeAddWeaponUI(WeaponDetailsSO weaponDetail)
    {
        weaponImage.sprite = weaponDetail.weaponSprite;
        TxtWeaponName.text = weaponDetail.weaponName;
    }
}

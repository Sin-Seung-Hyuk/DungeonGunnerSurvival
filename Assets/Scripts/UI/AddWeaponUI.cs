using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddWeaponUI : MonoBehaviour
{
    [SerializeField] private Image weaponImage;
    [SerializeField] private TextMeshProUGUI TxtWeaponName;


    public void InitializeAddWeaponUI(WeaponDetailsSO weaponDetail)
    {
        weaponImage.sprite = weaponDetail.weaponSprite;
        TxtWeaponName.text = weaponDetail.weaponName;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponStatUI : MonoBehaviour
{
    [SerializeField] private List<WeaponStatUIComponents> StatUIComponentList;
    [SerializeField] private Player player;


    private void Start()
    {
        StaticEventHandler.OnWeaponStatChanged += StaticEventHandler_OnWeaponStatChanged;
    }

    private void OnEnable()
    {
        SetWeaponStatText();
    }

    private void StaticEventHandler_OnWeaponStatChanged()
    {
        SetWeaponStatText();
    }

    private void SetWeaponStatText()
    {
        for (int i =0; i< player.weaponList.Count; ++i)
        {
            Weapon weapon = player.weaponList[i];

            if (weapon != null)
            {
                StatUIComponentList[i].weaponSprite.color = new Color(1f, 1f, 1f, 1f);
                StatUIComponentList[i].weaponSprite.sprite = weapon.weaponSprite;
                StatUIComponentList[i].TxtLevel.text = weapon.weaponLevel.ToString();
                StatUIComponentList[i].TxtDamage.text = weapon.weaponBaseDamage.ToString();
                StatUIComponentList[i].TxtCriticChance.text = weapon.weaponCriticChance.ToString();
                StatUIComponentList[i].TxtCriticDamage.text = weapon.weaponCriticDamage.ToString();
                StatUIComponentList[i].TxtFireRate.text = weapon.weaponFireRate.ToString();
                StatUIComponentList[i].TxtReload.text = weapon.weaponReloadTime.ToString();
            }
        }
    }
}

[System.Serializable]
public class WeaponStatUIComponents
{
    public Image weaponSprite;
    public TextMeshProUGUI TxtLevel;
    public TextMeshProUGUI TxtDamage;
    public TextMeshProUGUI TxtCriticChance;
    public TextMeshProUGUI TxtCriticDamage;
    public TextMeshProUGUI TxtFireRate;
    public TextMeshProUGUI TxtReload;
}
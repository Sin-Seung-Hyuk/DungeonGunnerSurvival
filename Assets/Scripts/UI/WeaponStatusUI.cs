using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStatusUI : MonoBehaviour
{
    [Header("Object Reference")]
    [SerializeField] private List<Image> weaponImage;
    [SerializeField] private List<TextMeshProUGUI> ammoRemainingText;
                             
    [SerializeField] private List<Transform> reloadBar;
    [SerializeField] private List<Image> barImage;

    [SerializeField] private Player player;
    private Coroutine reloadWeaponCoroutine;


    private void Start()
    {
        player.activeWeaponEvent.OnActiveWeapon += OnActiveWeapon;
        player.weaponFiredEvent.OnWeaponFired += OnWeaponFired;
        player.reloadWeaponEvent.OnReloadWeapon += OnReloadWeapon;
    }
    private void OnDisable()
    {
        player.activeWeaponEvent.OnActiveWeapon -= OnActiveWeapon;
        player.weaponFiredEvent.OnWeaponFired -= OnWeaponFired;
        player.reloadWeaponEvent.OnReloadWeapon -= OnReloadWeapon;
    }


    // �� �̺�Ʈ�� ������ �Լ� ȣ�� ========================================
    #region Subscribe
    private void OnActiveWeapon(ActiveWeaponEvent arg1, ActiveWeaponEventArgs args)
    {
        InitializeWeaponUI(args.weapon,args.weaponIndex);
    }

    private void OnReloadWeapon(ReloadWeaponEvent arg1, ReloadWeaponEventArgs arg2)
    {
        UpdateWeaponReloadBar(arg2.weapon, arg2.weaponIndex);
    }

    private void OnWeaponFired(WeaponFiredEvent arg1, WeaponFiredEventArgs arg2)
    {
        WeaponFired(arg2.weapon, arg2.weaponIndex);
    }
    #endregion


    // �� ��Ȳ�� ������ �Լ� ȣ�� =========================================
    #region Functions
    private void InitializeWeaponUI(Weapon weapon, int weaponIndex)
    {
        weaponImage[weaponIndex].gameObject.SetActive(true);
        weaponImage[weaponIndex].sprite = weapon.weaponSprite; // ��������Ʈ ����
        UpdateAmmoText(weapon, weaponIndex); // ���� ź�� 
    }

    private void UpdateWeaponReloadBar(Weapon weapon, int weaponIndex)
    {
        StopReloadWeaponCoroutine();

        reloadWeaponCoroutine = StartCoroutine(UpdateWeaponReloadBarRoutine(weapon, weaponIndex));
    }

    private void WeaponFired(Weapon weapon, int weaponIndex)
    {
        UpdateAmmoText(weapon, weaponIndex); // ���� ź�� 
    }
    #endregion


    // UI ������Ʈ �Լ� ���� =======================================================
    #region UI Upadate Functions
    private void UpdateAmmoText(Weapon weapon, int weaponIndex)
    {
        ammoRemainingText[weaponIndex].text = weapon.weaponAmmoRemaining.ToString() + " / " + weapon.weaponAmmoCapacity.ToString();
    }

    private IEnumerator UpdateWeaponReloadBarRoutine(Weapon weapon, int weaponIndex)
    {
        barImage[weaponIndex].color = Color.red;

        while (weapon.isWeaponReloading)
        {
            float barFill = weapon.weaponReloadTimer / weapon.weaponReloadTime;

            reloadBar[weaponIndex].transform.localScale = new Vector3(barFill, 1f, 1f);

            yield return null;
        }

        barImage[weaponIndex].color = Color.green;
        reloadBar[weaponIndex].transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void StopReloadWeaponCoroutine()
    {
        if (reloadWeaponCoroutine != null) StopCoroutine(reloadWeaponCoroutine);
    }
    #endregion
}

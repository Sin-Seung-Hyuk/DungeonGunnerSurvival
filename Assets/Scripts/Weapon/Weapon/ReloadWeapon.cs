using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ReloadWeaponEvent))]
[RequireComponent(typeof(WeaponReloadedEvent))]
public class ReloadWeapon : MonoBehaviour
{
    ReloadWeaponEvent reloadWeaponEvent;
    WeaponReloadedEvent weaponReloadedEvent;
    Coroutine reloadWeaponCoroutine;

    private void Awake()
    {
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        weaponReloadedEvent = GetComponent<WeaponReloadedEvent>();
    }
    private void OnEnable()
    {
        reloadWeaponEvent.OnReloadWeapon += ReloadWeaponEvent_OnReloadWeapon;
    }
    private void OnDisable()
    {
        reloadWeaponEvent.OnReloadWeapon -= ReloadWeaponEvent_OnReloadWeapon;
    }

    private void ReloadWeaponEvent_OnReloadWeapon(ReloadWeaponEvent reloadWeaponEvent, ReloadWeaponEventArgs reloadWeaponArgs)
    {
        StartReloadingWeapon(reloadWeaponArgs);
    }

    private void StartReloadingWeapon(ReloadWeaponEventArgs reloadWeaponArgs)
    {
        if (reloadWeaponCoroutine != null)
        {
            StopCoroutine(reloadWeaponCoroutine);
        }

        reloadWeaponCoroutine = StartCoroutine(ReloadWeaponRoutine(reloadWeaponArgs.weapon));
    }

    private IEnumerator ReloadWeaponRoutine(Weapon weapon)
    {
        //if (weapon.weaponDetails.weaponReloadSound != null)
        //{  // ������ ���� 
        //    SoundEffectManager.Instance.PlaySoundEffect(
        //        weapon.weaponDetails.weaponReloadSound);
        //}

        weapon.isWeaponReloading = true; // ������ ������ ����

        while (weapon.weaponReloadTimer < weapon.weaponReloadTime) // Ÿ�̸� < �ҿ�ð�
        {
            weapon.weaponReloadTimer += Time.deltaTime; // ������ ������ �ð��� �����Ҷ����� �ð����ϱ�
            yield return null;
        }
        
        weapon.weaponAmmoRemaining = weapon.weaponAmmoCapacity;

        weapon.weaponReloadTimer = 0f; // ������ �Ϸ�
        weapon.isWeaponReloading = false;
        //weaponReloadedEvent.CallWeaponReloadedEvent(weapon);
    }
}

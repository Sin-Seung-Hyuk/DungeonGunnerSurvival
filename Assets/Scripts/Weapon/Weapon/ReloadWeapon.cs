using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ReloadWeaponEvent))]
public class ReloadWeapon : MonoBehaviour
{
    ReloadWeaponEvent reloadWeaponEvent;
    Coroutine reloadWeaponCoroutine;

    private void Awake()
    {
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
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
        // ====================== ���⸦ ���̵�� �Ҹ��� ���������� ������ ����� ���� ========================
        //if (reloadWeaponArgs.weapon.weaponDetail.weaponReloadingSoundEffect != null)
        //{  // ������ ���� 
        //    SoundEffectManager.Instance.PlaySoundEffect(
        //        reloadWeaponArgs.weapon.weaponDetail.weaponReloadingSoundEffect);
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
    }
}

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
        // ====================== 무기를 많이들면 소리가 어지러워서 재장전 사운드는 빼기 ========================
        //if (reloadWeaponArgs.weapon.weaponDetail.weaponReloadingSoundEffect != null)
        //{  // 재장전 사운드 
        //    SoundEffectManager.Instance.PlaySoundEffect(
        //        reloadWeaponArgs.weapon.weaponDetail.weaponReloadingSoundEffect);
        //}

        weapon.isWeaponReloading = true; // 재장전 중으로 변경

        while (weapon.weaponReloadTimer < weapon.weaponReloadTime) // 타이머 < 소요시간
        {
            weapon.weaponReloadTimer += Time.deltaTime; // 정해진 재장전 시간에 도달할때까지 시간더하기
            yield return null;
        }
        
        weapon.weaponAmmoRemaining = weapon.weaponAmmoCapacity;

        weapon.weaponReloadTimer = 0f; // 재장전 완료
        weapon.isWeaponReloading = false;
    }
}

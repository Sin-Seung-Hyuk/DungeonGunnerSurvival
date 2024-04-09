using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : Singleton<SoundEffectManager> 
{
    public int soundsVolume = 8;

    private void Start()
    {
        if (PlayerPrefs.HasKey("soundsVolume"))
            soundsVolume = PlayerPrefs.GetInt("soundsVolume"); // ����Ǿ��ִ� ���� ��������

        SetSoundsVolume(soundsVolume);
    }
    private void OnDisable()
    {
        // ���õ� ���� �����ϰ� ����
        PlayerPrefs.SetInt("soundsVolume", soundsVolume);
    }

    public void PlaySoundEffect(SoundEffectSO soundEffect)
    {
        // ������Ʈ Ǯ�� ��ϵ� ���� ���ӿ�����Ʈ Ȱ��ȭ�Ͽ� �Ҹ� ���
        SoundEffect sound = (SoundEffect)ObjectPoolManager.Instance.
            Release(soundEffect.soundPrefab, Vector3.zero, Quaternion.identity);

        sound.SetSound(soundEffect); // ����� Ŭ��,���� ����
        sound.gameObject.SetActive(true);
        StartCoroutine(DisableSound(sound, soundEffect.soundEffectClip.length));

    }

    public void IncreaseVolume()
    {
        int maxVolume = 20;

        if (soundsVolume >= maxVolume) return;

        ++soundsVolume;
        SetSoundsVolume(soundsVolume);
    }
    public void DecreaseVolume()
    {
        if (soundsVolume == 0) return;

        --soundsVolume;
        SetSoundsVolume(soundsVolume);
    }

    private IEnumerator DisableSound(SoundEffect sound, float soundDuration)
    {   // ���� ���̸�ŭ �ð��� ������ ���� ������Ʈ ��Ȱ��ȭ
        yield return new WaitForSeconds(soundDuration);
        sound.gameObject.SetActive(false);
    }

    private void SetSoundsVolume(int soundsVolume)
    {
        float muteDecibels = -80f;

        if (soundsVolume == 0)
        {
            GameResources.Instance.soundsMasterMixerGroup.audioMixer.SetFloat("soundsVolume", muteDecibels);
        }
        else
        {   // ���� ������ ���ú��� ��ȯ
            GameResources.Instance.soundsMasterMixerGroup.audioMixer.SetFloat(
                "soundsVolume", Utilities.LinearToDecibels(soundsVolume));
        }
    }
}

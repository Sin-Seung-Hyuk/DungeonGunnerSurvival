using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI soundsText;

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    private IEnumerator InitializeUI()
    {
        // 한 프레임 그냥 넘기기. 사운드 세팅 기다리기
        yield return null;

        musicText.text = MusicManager.Instance.musicVolume.ToString();
        soundsText.text = SoundEffectManager.Instance.soundsVolume.ToString();
    }

    private void OnEnable()
    {
        StartCoroutine(InitializeUI());
    }
    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    private void Update() {
        Time.timeScale = 0f; // 활성화 되어있는 동안 일시정지
    }

    public void IncreaseMusicVolume()
    {
        MusicManager.Instance.IncreaseVolume();
        musicText.SetText(MusicManager.Instance.musicVolume.ToString());
    }
    public void DecreaseMusicVolume()
    {
        MusicManager.Instance.DecreaseVolume();
        musicText.SetText(MusicManager.Instance.musicVolume.ToString());
    }
    public void IncreaseSoundsVolume()
    {
        SoundEffectManager.Instance.IncreaseVolume();
        soundsText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
    }
    public void DecreaseSoundsVolume()
    {
        SoundEffectManager.Instance.DecreaseVolume();
        soundsText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
    }

    public void BtnMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    public void BtnResume()
    {
        gameObject.SetActive(false);
    }
}

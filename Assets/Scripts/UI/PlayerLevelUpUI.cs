
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class PlayerLevelUpUI : MonoBehaviour
{
    // UI 컴포넌트가 들어갈 리스트
    [SerializeField] private List<LevelUpUIComponents> levelUpUIComponents = new List<LevelUpUIComponents>(3);
    private Player player;


    private void OnEnable()
    {
        Time.timeScale = 0; // 이 UI가 활성화 될때마다 일시정지

        // 사운드 추가
    }

    public void InitializeLevelUpUI(PlayerLevelUpList list, int idx) // 몇번째 UI 컴포넌트들 설정인지
    {
        // 1. 플레이어 스탯인지 무기스탯인지 확인
        // 2. 플레이어 스탯 : 현재 플레이어 스프라이트, 플레이어의 스탯변경 함수 실행
        // 3. 무기스탯 : 랜덤으로 무기 선택, 해당 무기 클래스 가져와 스탯변경,스프라이트 설정

        player = GameManager.Instance.GetPlayer(); // 플레이어 가져오기

        // UI 텍스트 설정
        levelUpUIComponents[idx].TxtChoice.color = list.txtColor;
        levelUpUIComponents[idx].TxtChangeValue.color = list.txtColor;
        levelUpUIComponents[idx].TxtChoice.text = list.choiceText;
        levelUpUIComponents[idx].TxtChangeValue.text = list.changeValueText;

        // 기존에 버튼에 추가된 기능들 지우고 다시 부여하기
        levelUpUIComponents[idx].BtnChoice.onClick.RemoveAllListeners();

        if (list.isPlayerStat) // 플레이어 스탯인지, 무기스탯인지 확인
            SetPlayerLevelUpChoice(list, idx);
        else SetWeaponLevelUpChoice(list, idx);
    }

    private void SetPlayerLevelUpChoice(PlayerLevelUpList list, int idx)
    {
        // 플레이어 스프라이트로 설정
        levelUpUIComponents[idx].choiceImage.sprite = player.playerDetails.playerSprite;
        levelUpUIComponents[idx].BtnChoice.onClick.AddListener(() => BtnPlayerStatUp(list)); // 인수가 있는 함수등록시 람다식활용
    }
    private void SetWeaponLevelUpChoice(PlayerLevelUpList list, int idx)
    {
        // 현재 플레이어 무기 리스트에서 랜덤으로 무기 선택하여 가져오기
        Weapon randomWeapon = player.weaponList[Random.Range(0, player.weaponList.Count)];
        levelUpUIComponents[idx].choiceImage.sprite = randomWeapon.weaponSprite;
        levelUpUIComponents[idx].BtnChoice.onClick.AddListener(() => BtnWeaponStatUp(randomWeapon, list));
    }


    private void BtnWeaponStatUp(Weapon weapon, PlayerLevelUpList list)
    {
        // 랜덤으로 선택된 무기의 스탯 변경 (해당 무기의 스탯만 변경됨)
        weapon.ChangeWeaponStat(list.statType, list.changeValue, true);
        Time.timeScale = 1; // 게임 재개

        gameObject.SetActive(false);
    }

    private void BtnPlayerStatUp(PlayerLevelUpList list)
    {
        // 플레이어의 스탯 변경
        player.playerStatChangedEvent.CallPlayerStatChangedEvent(list.statType, list.changeValue);
        Time.timeScale = 1;

        gameObject.SetActive(false);
    }
}

[System.Serializable]
public class LevelUpUIComponents // 보여줄 UI 컴포넌트들
{
    public Button BtnChoice;
    public Image choiceImage;
    public TextMeshProUGUI TxtChoice;
    public TextMeshProUGUI TxtChangeValue;
}
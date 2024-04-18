
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class PlayerLevelUpUI : MonoBehaviour
{
    // UI ������Ʈ�� �� ����Ʈ
    [SerializeField] private List<LevelUpUIComponents> levelUpUIComponents = new List<LevelUpUIComponents>(3);
    private Player player;


    private void OnEnable()
    {
        Time.timeScale = 0; // �� UI�� Ȱ��ȭ �ɶ����� �Ͻ�����

        // ���� �߰�
    }

    public void InitializeLevelUpUI(PlayerLevelUpList list, int idx) // ���° UI ������Ʈ�� ��������
    {
        // 1. �÷��̾� �������� ���⽺������ Ȯ��
        // 2. �÷��̾� ���� : ���� �÷��̾� ��������Ʈ, �÷��̾��� ���Ⱥ��� �Լ� ����
        // 3. ���⽺�� : �������� ���� ����, �ش� ���� Ŭ���� ������ ���Ⱥ���,��������Ʈ ����

        player = GameManager.Instance.GetPlayer(); // �÷��̾� ��������

        // UI �ؽ�Ʈ ����
        levelUpUIComponents[idx].TxtChoice.color = list.txtColor;
        levelUpUIComponents[idx].TxtChangeValue.color = list.txtColor;
        levelUpUIComponents[idx].TxtChoice.text = list.choiceText;
        levelUpUIComponents[idx].TxtChangeValue.text = list.changeValueText;

        // ������ ��ư�� �߰��� ��ɵ� ����� �ٽ� �ο��ϱ�
        levelUpUIComponents[idx].BtnChoice.onClick.RemoveAllListeners();

        if (list.isPlayerStat) // �÷��̾� ��������, ���⽺������ Ȯ��
            SetPlayerLevelUpChoice(list, idx);
        else SetWeaponLevelUpChoice(list, idx);
    }

    private void SetPlayerLevelUpChoice(PlayerLevelUpList list, int idx)
    {
        // �÷��̾� ��������Ʈ�� ����
        levelUpUIComponents[idx].choiceImage.sprite = player.playerDetails.playerSprite;
        levelUpUIComponents[idx].BtnChoice.onClick.AddListener(() => BtnPlayerStatUp(list)); // �μ��� �ִ� �Լ���Ͻ� ���ٽ�Ȱ��
    }
    private void SetWeaponLevelUpChoice(PlayerLevelUpList list, int idx)
    {
        // ���� �÷��̾� ���� ����Ʈ���� �������� ���� �����Ͽ� ��������
        Weapon randomWeapon = player.weaponList[Random.Range(0, player.weaponList.Count)];
        levelUpUIComponents[idx].choiceImage.sprite = randomWeapon.weaponSprite;
        levelUpUIComponents[idx].BtnChoice.onClick.AddListener(() => BtnWeaponStatUp(randomWeapon, list));
    }


    private void BtnWeaponStatUp(Weapon weapon, PlayerLevelUpList list)
    {
        // �������� ���õ� ������ ���� ���� (�ش� ������ ���ȸ� �����)
        weapon.ChangeWeaponStat(list.statType, list.changeValue, true);
        Time.timeScale = 1; // ���� �簳

        gameObject.SetActive(false);
    }

    private void BtnPlayerStatUp(PlayerLevelUpList list)
    {
        // �÷��̾��� ���� ����
        player.playerStatChangedEvent.CallPlayerStatChangedEvent(list.statType, list.changeValue);
        Time.timeScale = 1;

        gameObject.SetActive(false);
    }
}

[System.Serializable]
public class LevelUpUIComponents // ������ UI ������Ʈ��
{
    public Button BtnChoice;
    public Image choiceImage;
    public TextMeshProUGUI TxtChoice;
    public TextMeshProUGUI TxtChangeValue;
}
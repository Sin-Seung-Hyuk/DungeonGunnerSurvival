using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CharacterSelectorUI : MonoBehaviour
{
    [SerializeField] private Transform characterSelector;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI characterStrength;
    [SerializeField] private TextMeshProUGUI characterWeakness;
    [SerializeField] private Image StartingWeaponImage;

    private List<PlayerDetailsSO> playerDetailsList;
    private GameObject playerSelectionPrefab;
    private CurrentPlayerSO currentPlayer;
    private List<GameObject> playerCharacterGameObjList = new List<GameObject>();
    private Coroutine coroutine;
    private int selectedPlayerIdx = 0; // ���õ� ĳ���� �ּ�
    private float offset = 4f; // ĳ���� ����ȭ�鿡�� ĳ���� ���� ����

    private void Awake()
    {
        playerSelectionPrefab = GameResources.Instance.playerSelectionPrefab;
        playerDetailsList = GameResources.Instance.playerDetailsList;
        currentPlayer = GameResources.Instance.currentPlayer;
    }

    private void Start()
    {
        for (int i = 0; i < playerDetailsList.Count; i++)
        {
            // characterSelector�� ���� ������ �� �ִ� ĳ���� ������ �����ϱ�
            GameObject playerSelectionObj = Instantiate(playerSelectionPrefab, characterSelector);
            playerCharacterGameObjList.Add(playerSelectionObj);

            // localPosition ���� �θ������Ʈ(characterSelector)�� �������� �� ��ǥ�� ��ġ ���� (ĳ���͸��� 4f ����)
            playerSelectionObj.transform.localPosition = new Vector3((offset * i), 0f, 0f);
            PopulatePlayerDetails(playerSelectionObj.GetComponent<PlayerSelectionUI>(), playerDetailsList[i]);
        }

        UpdateCharacterDescription();

        currentPlayer.currentPlayerDetailsSO = playerDetailsList[selectedPlayerIdx];
    }

    private void PopulatePlayerDetails(PlayerSelectionUI playerSelection, PlayerDetailsSO playerDetailsSO)
    {   // PlayerDetailsSO �� �ִ� ��������Ʈ,�ִϸ����� �����͸� ������ ����ŷ�� �Ȱ��� ����ٿ��ֱ�
        playerSelection.playerSpriteRenderer.sprite = playerDetailsSO.playerSprite;
        playerSelection.animator.runtimeAnimatorController = playerDetailsSO.runtimeAnimatorController;
    }

    public void NextCharacter()
    {
        selectedPlayerIdx = (selectedPlayerIdx + 1) % playerDetailsList.Count;

        currentPlayer.currentPlayerDetailsSO = playerDetailsList[selectedPlayerIdx];

        MoveToSelectedCharacter(selectedPlayerIdx);
        UpdateCharacterDescription();
    }
    public void PrevCharacter()
    {
        if (selectedPlayerIdx == 0) selectedPlayerIdx = playerDetailsList.Count;

        selectedPlayerIdx--;

        currentPlayer.currentPlayerDetailsSO = playerDetailsList[selectedPlayerIdx];

        MoveToSelectedCharacter(selectedPlayerIdx);
        UpdateCharacterDescription();
    }

    private void MoveToSelectedCharacter(int idx)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(MoveToSelectedCharacterRoutine(idx));
    }

    private IEnumerator MoveToSelectedCharacterRoutine(int idx)
    {
        // ���õ� ĳ������ localPosition (�θ���� ��ġ) x��
        float currentLocalXPosition = characterSelector.localPosition.x;
        float targetLocalXPosition = idx * offset * characterSelector.localScale.x * -1f;

        while (Mathf.Abs(currentLocalXPosition - targetLocalXPosition) > 0.01f)
        {
            // Lerp�� ���� ���� x�������� �Ű������� ���� �ּ��� x��ġ���� Time.deltaTime * 10f �� ���ļ� �̵�
            currentLocalXPosition = Mathf.Lerp(currentLocalXPosition, targetLocalXPosition, Time.deltaTime * 10f);

            characterSelector.localPosition = new Vector3(currentLocalXPosition, characterSelector.localPosition.y, 0f);
            yield return null;
        }

        characterSelector.localPosition = new Vector3(targetLocalXPosition, characterSelector.localPosition.y, 0f);
    }


    public void UpdateCharacterDescription()
    {
        characterName.text = playerDetailsList[selectedPlayerIdx].characterName;
        characterStrength.text = playerDetailsList[selectedPlayerIdx].characterStrength;
        characterWeakness.text = playerDetailsList[selectedPlayerIdx].characterWeakness;
        StartingWeaponImage.sprite = playerDetailsList[selectedPlayerIdx].playerStartingWeapon.weaponSprite;
    }
}

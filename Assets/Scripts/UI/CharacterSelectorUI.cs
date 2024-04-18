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
    private int selectedPlayerIdx = 0; // 선택된 캐릭터 주소
    private float offset = 4f; // 캐릭터 선택화면에서 캐릭터 간의 간격

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
            // characterSelector에 내가 선택할 수 있는 캐릭터 프리팹 생성하기
            GameObject playerSelectionObj = Instantiate(playerSelectionPrefab, characterSelector);
            playerCharacterGameObjList.Add(playerSelectionObj);

            // localPosition 으로 부모오브젝트(characterSelector)를 기준으로 한 좌표로 위치 조절 (캐릭터마다 4f 간격)
            playerSelectionObj.transform.localPosition = new Vector3((offset * i), 0f, 0f);
            PopulatePlayerDetails(playerSelectionObj.GetComponent<PlayerSelectionUI>(), playerDetailsList[i]);
        }

        UpdateCharacterDescription();

        currentPlayer.currentPlayerDetailsSO = playerDetailsList[selectedPlayerIdx];
    }

    private void PopulatePlayerDetails(PlayerSelectionUI playerSelection, PlayerDetailsSO playerDetailsSO)
    {   // PlayerDetailsSO 에 있는 스프라이트,애니메이터 데이터를 가져와 마네킹에 똑같이 복사붙여넣기
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
        // 선택된 캐릭터의 localPosition (부모기준 위치) x값
        float currentLocalXPosition = characterSelector.localPosition.x;
        float targetLocalXPosition = idx * offset * characterSelector.localScale.x * -1f;

        while (Mathf.Abs(currentLocalXPosition - targetLocalXPosition) > 0.01f)
        {
            // Lerp를 통해 현재 x포지션을 매개변수로 들어온 주소의 x위치까지 Time.deltaTime * 10f 에 걸쳐서 이동
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

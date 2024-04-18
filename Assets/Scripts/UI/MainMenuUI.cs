using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject BtnPlay;
    //[SerializeField] private GameObject BtnHighScores;
    [SerializeField] private GameObject BtnReturnToMainMenu;
    [SerializeField] private GameObject BtnGameRules;

    private bool isHighScoreScene = false;
    private bool isGameRulesScene = false;

    void Start()
    {
        //MusicManager.Instance.PlayMusic(GameResources.Instance.mainMusic, 0f, 2f);

        SceneManager.LoadScene("CharacterScene", LoadSceneMode.Additive);

        BtnReturnToMainMenu.SetActive(false);
    }


    public void LoadHighScore()
    {
        BtnPlay.SetActive(false);
        //BtnHighScores.SetActive(false);
        BtnGameRules.SetActive(false);
        isHighScoreScene = true;

        SceneManager.UnloadSceneAsync("CharacterScene");

        BtnReturnToMainMenu.SetActive(true);

        SceneManager.LoadScene("HighScoreScene", LoadSceneMode.Additive);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void LoadCharacterSelector()
    {
        BtnReturnToMainMenu.SetActive(false);

        if (isHighScoreScene)
        {
            SceneManager.UnloadSceneAsync("HighScoreScene");
            isHighScoreScene = false;
        }
        else if (isGameRulesScene)
        {
            SceneManager.UnloadSceneAsync("GameRulesScene");
            isGameRulesScene = false;
        }

        BtnPlay.SetActive(true);
        //BtnHighScores.SetActive(true);
        BtnGameRules.SetActive(true);

        SceneManager.LoadScene("CharacterScene", LoadSceneMode.Additive);
    }

    public void LoadGameRulesScene()
    {
        BtnPlay.SetActive(false);
        //BtnHighScores.SetActive(false);
        BtnGameRules.SetActive(false);
        isGameRulesScene = true;

        SceneManager.UnloadSceneAsync("CharacterScene");

        BtnReturnToMainMenu.SetActive(true);

        SceneManager.LoadScene("GameRulesScene", LoadSceneMode.Additive);
    }
}

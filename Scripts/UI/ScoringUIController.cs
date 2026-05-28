using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringUIController : MonoBehaviour
{
    [Header("===== BACKGROUND =====")]

    [SerializeField] Image background;

    [SerializeField] Sprite[] backgroundImages;

    [Header("===== SCORING SCREEN =====")]

    [SerializeField] Canvas scoringScreenCanvas;

    [SerializeField] Text playerScoreText;

    [SerializeField] Button buttonMainMenu;

    [SerializeField] Transform highScoreLeaderboardContainer;

    [Header("===== SCORING SCREEN =====")]

    [SerializeField] Canvas newHighScoreScreenCanvas;

    [SerializeField] Button buttonCancel;

    [SerializeField] Button buttonSubmin;

    [SerializeField] InputField playerNameInputField;


    void Start()
    {
        Cursor.visible = true; //顯示滑鼠
        Cursor.lockState = CursorLockMode.None; //顯示滑鼠位置


        ShowRandomBackground();

        if (ScoreManager.Instance.HasNewHighScore)
        {
            ShowNewHighScoreScreen();
        }

        else
        {
            ShowScoringScreen();
        }

        ButtonPressedBehavior.buttonFunctionTable.Add(buttonMainMenu.gameObject.name, OnButtonMainMenuClicked);

        ButtonPressedBehavior.buttonFunctionTable.Add(buttonSubmin.gameObject.name, OnButtonSubmitClicked);

        ButtonPressedBehavior.buttonFunctionTable.Add(buttonCancel.gameObject.name, HideNewHighScoreScreen);

        GameManager.GameState = GameState.Scoring;
    }

    void OnDisable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    void ShowRandomBackground()
    {
        background.sprite = backgroundImages[Random.Range(0, backgroundImages.Length)];
    }

    void ShowScoringScreen()
    {
        scoringScreenCanvas.enabled = true;

        playerScoreText.text = ScoreManager.Instance.Scroe.ToString(); //顯示玩家分數
        UIInput01.Instance.SelectUI(buttonMainMenu); //選中主菜單
        
        UpdateHighScoreLeaderboard(); // 高分榜
    }

    void ShowNewHighScoreScreen()
    {
        newHighScoreScreenCanvas.enabled = true;

        UIInput01.Instance.SelectUI(buttonCancel);
    }


    void HideNewHighScoreScreen()
    {
        newHighScoreScreenCanvas.enabled = false;
        ScoreManager.Instance.SavePlayerScoreData();
        ShowRandomBackground();
        ShowScoringScreen();
    }


    void UpdateHighScoreLeaderboard()
    {
        var playerScoreList = ScoreManager.Instance.LoadPlayerScoreData().list;

        for (int i = 0; i < highScoreLeaderboardContainer.childCount; i++)
        {
            var child = highScoreLeaderboardContainer.GetChild(i);

            child.Find("Rank").GetComponent<Text>().text = (i + 1).ToString();
            child.Find("Score").GetComponent<Text>().text = playerScoreList[i].score.ToString();
            child.Find("Name").GetComponent<Text>().text = playerScoreList[i].playerName;
        }
    }

    void OnButtonMainMenuClicked()
    {
        scoringScreenCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }

    void OnButtonSubmitClicked()
    {
        if (!string.IsNullOrEmpty(playerNameInputField.text))
        {
            ScoreManager.Instance.SetPlayerName(playerNameInputField.text);
        }

        HideNewHighScoreScreen();
    }

}

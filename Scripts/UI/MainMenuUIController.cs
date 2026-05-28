using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [Header("===== CANVAS =====")]

    [SerializeField] Canvas mainMenuCanvas;

    [Header("===== BUTTONS =====")]

    [SerializeField] Button buttonSelect;

    [SerializeField] Button buttonContiune;

    [SerializeField] Button buttonOptions;

    [SerializeField] Button buttonOptions02;

    [SerializeField] Button buttonQuit;

    void OnEnable()
    {
        // buttonStart.onClick.AddListener(OnStartGameButtonClick);

        ButtonPressedBehavior.buttonFunctionTable.Add(buttonSelect.gameObject.name, OnSelectClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonContiune.gameObject.name, OnContiuneClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonOptions.gameObject.name, OnOptionsButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonOptions02.gameObject.name, OnOptionsButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonQuit.gameObject.name, OnQuitButtonClick);

    }

    void OnDisable()
    {
        // buttonStart.onClick.RemoveAllListeners();

        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    void Start()
    {
        Time.timeScale = 1f;
        GameManager.GameState = GameState.Playing;
        UIInput01.Instance.SelectUI(buttonSelect);
    }


    void OnOptionsButtonClick()
    {
        UIInput01.Instance.SelectUI(buttonOptions);
    }

    void OnQuitButtonClick()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    void OnSelectClick()
    {
        mainMenuCanvas.enabled = false;
        SceneLoader.Instance.LoadSelectCharacterUIScene();
    }

    void OnContiuneClick()
    {
        mainMenuCanvas.enabled = false;
        SceneLoader.Instance.LoadGameplay3Scene();
    }
}

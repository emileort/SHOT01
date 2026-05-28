using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    [Header("----PLAYER INPUT----")]

    [SerializeField] PlayerInput playerInput;

    [Header("----AUDIO DATA----")]

    [SerializeField] AudioData pauseSFX;

    [SerializeField] AudioData unpauseSFX;

    [Header("----CANVAS----")]

    [SerializeField] Canvas hUDCanvas;

    [SerializeField] Canvas menusCanvas;

    [Header("----BUTTON----")]

    [SerializeField] Button resumeButton;

    [SerializeField] Button optionsButton;

    [SerializeField] Button mainMenuButton;

    int buttonPressedParameterID = Animator.StringToHash("Pressed");

    void OnEnable()
    {
        playerInput.onPause += Pause;

        playerInput.onUnpause += Unpause;

        // 點擊事件訂閱
        // resumeButton.onClick.AddListener(OnResumeButtonClick);
        // optionsButton.onClick.AddListener(OnOptionsButtonClick);
        // mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);

        // 交給動畫器處理
        ButtonPressedBehavior.buttonFunctionTable.Add(resumeButton.gameObject.name, OnResumeButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(optionsButton.gameObject.name, OnOptionsButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(mainMenuButton.gameObject.name, OnMainMenuButtonClick);
    }

    void OnDisable()
    {
        playerInput.onPause -= Pause;

        playerInput.onUnpause -= Unpause;


        ButtonPressedBehavior.buttonFunctionTable.Clear();
        // 點擊事件退訂
        // resumeButton.onClick.RemoveListener(OnResumeButtonClick);
        // resumeButton.onClick.RemoveAllListeners();
        // optionsButton.onClick.RemoveAllListeners();
        // mainMenuButton.onClick.RemoveAllListeners();
    }

    void Pause()
    {
        
        hUDCanvas.enabled = false;
        menusCanvas.enabled = true;
        GameManager.GameState = GameState.Paused;
        TimeController.Instance.Pause();
        playerInput.EnablePauseMenuInput();
        playerInput.SwitchToDynamicUpdateMode();

        UIInput01.Instance.SelectUI(resumeButton);

        AudioManager.Instance.PlaySFX(pauseSFX);
    }

    void Unpause()
    {
        // 選中按鈕
        resumeButton.Select();
        // 調用按下動畫進行播放
        resumeButton.animator.SetTrigger(buttonPressedParameterID);

        AudioManager.Instance.PlaySFX(unpauseSFX);

        // OnResumeButtonClick();
    }

    void OnResumeButtonClick()
    {
        
        hUDCanvas.enabled = true;
        menusCanvas.enabled = false;
        GameManager.GameState = GameState.Playing;
        TimeController.Instance.Unpause();
        playerInput.EnableGameplayInput();
        playerInput.SwitchToFixedUpdateMode();
    }

    void OnOptionsButtonClick()
    {
        //加載遊戲設定畫面
        UIInput01.Instance.SelectUI(optionsButton);
        playerInput.EnablePauseMenuInput();
    }

    void OnMainMenuButtonClick()
    {
        menusCanvas.enabled = false;
        //加載標題畫面
        SceneLoader.Instance.LoadMainMenuScene();
    }


}

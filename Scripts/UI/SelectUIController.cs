using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectUIController : MonoBehaviour
{
    [Header("===== CANVAS =====")]

    [SerializeField] Canvas selectCanvas;

    [Header("===== BUTTONS =====")]

    [SerializeField] Button buttonStart1;

    [SerializeField] Button buttonStart2;

    [SerializeField] Button buttonOptions;

    [SerializeField] Button buttonBackMainMenu;

    public GameObject thatOneGuy2;

    Animator animator;


    void OnEnable()
    {
        // buttonStart.onClick.AddListener(OnStartGameButtonClick);

        ButtonPressedBehavior.buttonFunctionTable.Add(buttonStart1.gameObject.name, OnStartGameButton1Click);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonStart2.gameObject.name, OnStartGameButton2Click);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonOptions.gameObject.name, OnOptionsButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonBackMainMenu.gameObject.name, OnButtonMainMenuClicked);
        
    }

    void OnDisable()
    {
        // buttonStart.onClick.RemoveAllListeners();

        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        Time.timeScale = 1f;
        GameManager.GameState = GameState.Playing;
        UIInput01.Instance.SelectUI(buttonStart1);
    }

    void OnStartGameButton1Click()
    {
        selectCanvas.enabled = false;

        Animator animator = thatOneGuy2.GetComponent<Animator>();

        SceneLoader.Instance.LoadGameplay1Scene();
    }

    void OnStartGameButton2Click()
    {
        selectCanvas.enabled = false;
        SceneLoader.Instance.LoadGameplay2Scene();
    }

    void OnOptionsButtonClick()
    {
        UIInput01.Instance.SelectUI(buttonOptions);
    }

    void OnButtonMainMenuClicked()
    {
        selectCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }



}


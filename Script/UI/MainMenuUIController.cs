using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMO
{
    public class MainMenuUIController : MonoBehaviour
    {
        [Header("===== CANVAS =====")]

        [SerializeField] Canvas mainMenuCanvas;

        [Header("===== BUTTONS =====")]

        [SerializeField] Button buttonStart;

        [SerializeField] Button buttonContiune;

        [SerializeField] Button buttonOptions;

        [SerializeField] Button buttonOptions02;

        [SerializeField] Button buttonQuit;

        void OnEnable()
        {
            buttonStart.onClick.AddListener(OnStartGameButtonClick);
            buttonContiune.onClick.AddListener(OnContiuneClick);
            buttonOptions.onClick.AddListener(OnOptionsButtonClick);
            buttonOptions02.onClick.AddListener(OnOptionsButtonClick);
            buttonQuit.onClick.AddListener(OnQuitButtonClick);

        }


        void OnOptionsButtonClick()
        {

        }

        void OnQuitButtonClick()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        void OnStartGameButtonClick()
        {
            SceneLoader.Instance.LoadGameplay1Scene();
        }

        void OnContiuneClick()
        {
            SceneLoader.Instance.LoadGameplay2Scene();
        }
    }

}

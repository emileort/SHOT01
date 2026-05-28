using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogStstem : MonoBehaviour
{
    public GameObject DialogUI;
    public Text DialogText;
    [TextArea(1, 3)] public string[] DialogTextList;
    public int currentIndex;
    public float charsPerSecond = 2f;
    private float timer;

    void Start()
    {
        timer = 0;
        charsPerSecond = Mathf.Max(2f, charsPerSecond);

        GameManager.GameState = GameState.Paused;
        TimeController.Instance.Pause();
    }

    private void Update()
    {
        ContinueDialog();
    }

    public void CloseDialog()
    {
        DialogUI.SetActive(false);
    }

    public void ContinueDialog()
    {
        currentIndex++;

        if (currentIndex < DialogTextList.Length)
        {
            DialogText.text = DialogTextList[currentIndex];
            timer += Time.deltaTime;
            if (timer >= charsPerSecond)
            {
                timer = 0;
                currentIndex++;
            }
        }
        else if(currentIndex > DialogTextList.Length)
        {
            CloseDialog();
            GameManager.GameState = GameState.Playing;
            TimeController.Instance.Unpause();
        }
    }

    private void OnEnable()
    {
        
        currentIndex = 0;
        DialogText.text = DialogTextList[currentIndex];
    }
}

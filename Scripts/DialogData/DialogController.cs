using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    public DialogData dialogPlayer2;
    public DialogData dialogPlayer3;

    private Stack<string> dialogPlayer2Stack = new Stack<string>();
    private Stack<string> dialogPlayer3Stack = new Stack<string>();

    public bool isTalking;

    private void Awake()
    {
        FillDialogStack();
    }

    public void FillDialogStack()
    {
        for(int i= dialogPlayer2.dialogList.Count - 1; i >= 0; i--)
        {
            dialogPlayer2Stack.Push(dialogPlayer2.dialogList[i]);
        }
        for (int i = dialogPlayer3.dialogList.Count - 1; i >= 0; i--)
        {
            dialogPlayer3Stack.Push(dialogPlayer3.dialogList[i]);
        }
    }

    public void ShowDialogPlayer2()
    {
        if (!isTalking)
            StartCoroutine(DialogRoutine(dialogPlayer2Stack));
    }

    public void ShowDialogPlayer3()
    {
        if (!isTalking)
            StartCoroutine(DialogRoutine(dialogPlayer3Stack));
    }

    private IEnumerator DialogRoutine(Stack<string> data)
    {
        isTalking = true;

        if(data.TryPop(out string result))
        {
            EventHandler.CallShowDialogEvent(result);
            yield return null;
            GameManager.GameState = GameState.Paused;
        }
        else
        {
            EventHandler.CallShowDialogEvent(string.Empty);
            FillDialogStack();
            GameManager.GameState = GameState.Playing;
        }

        isTalking = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogController))]

public class CharacterDialog : Interactive
{
    DialogController dialogController;
    private void Awake()
    {
        dialogController = GetComponent<DialogController>();
    }

    protected override void OnClickedAction()
    {
        dialogController.ShowDialogPlayer3();
    }
    public override void EmptyClicked()
    {
        if (isDone)
        {
            dialogController.ShowDialogPlayer3();
        }
        else
        {
            dialogController.ShowDialogPlayer2();
        }
    }
}

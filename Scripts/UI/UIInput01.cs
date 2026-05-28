using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIInput01 : Singleton<UIInput01>
{
    [SerializeField] PlayerInput playerInput;

    InputSystemUIInputModule UIInputModule;

    protected override void Awake()
    {
        base.Awake();

        UIInputModule = GetComponent<InputSystemUIInputModule>();

        UIInputModule.enabled = false;
    }

    public void SelectUI(Selectable UIObject)
    {
        UIObject.Select();

        UIObject.OnSelect(null);

        UIInputModule.enabled = true;
    }

    public void DisableALLUIInput()
    {
        playerInput.DisableAllInputs();

        UIInputModule.enabled = false;
    }
}

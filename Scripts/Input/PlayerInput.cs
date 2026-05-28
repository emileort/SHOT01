using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName="Player Input")]

public class PlayerInput :
    ScriptableObject,
    InputActions.IGamePlayerActions,
    InputActions.IPauseMenuActions,
    InputActions.IGameOverScreenActions
{
    public event UnityAction<Vector2> onMove = delegate { };

    public event UnityAction onStopMove = delegate { };

    public event UnityAction onFire = delegate { };

    public event UnityAction onStopFire = delegate { };

    public event UnityAction onDodge = delegate { };

    public event UnityAction onOverdrive = delegate { };

    public event UnityAction onPause = delegate { };

    public event UnityAction onUnpause = delegate { };

    public event UnityAction onLaunchMissile = delegate { };

    public event UnityAction onConfirmGameOver = delegate { };

    public event UnityAction onShift = delegate { };

    public event UnityAction onShiftStop = delegate { };

    InputActions inputActions;

    void OnEnable()
    {
        inputActions = new InputActions();
        inputActions.GamePlayer.SetCallbacks(this);
        inputActions.PauseMenu.SetCallbacks(this);
        inputActions.GameOverScreen.SetCallbacks(this);
    }

    void OnDisable()
    {
        DisableAllInputs();
    }


    // 切換動作表
    void SwitchActionMap(InputActionMap actionMap,bool isUIIput)
    {
        inputActions.Disable();
        actionMap.Enable();

        if (isUIIput)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //暫停時將原本的時間固定更新改成動態更新
    public void SwitchToDynamicUpdateMode()
    {
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    }

    public void SwitchToFixedUpdateMode()
    {
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
    }

    //過場或劇情時，暫停動作表運行
    public void DisableAllInputs()
    {
        inputActions.Disable();

        //inputActions.GamePlayer.Disable();
        //inputActions.PauseMenu.Disable();
    }
    //啟用動作表時，滑鼠可見為隱藏，動作為鎖定
    public void EnableGameplayInput()
    {
        SwitchActionMap(inputActions.GamePlayer, false);
        
        //inputActions.GamePlayer.Enable();

        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void EnablePauseMenuInput()
    {
        SwitchActionMap(inputActions.PauseMenu, true);
    }

    public void EnableGameOverScreenInput()
    {
        SwitchActionMap(inputActions.GameOverScreen, false);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onMove.Invoke(context.ReadValue<Vector2>());
        }

        if(context.phase== InputActionPhase.Canceled)
        {
            onStopMove.Invoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onFire.Invoke();
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            onStopFire.Invoke();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onDodge.Invoke();
        }
    }

    public void OnOverdrive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onOverdrive.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onPause.Invoke();
        }
    }

    public void OnUnpause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onUnpause.Invoke();
        }
    }

    public void OnLaunchMissile(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onLaunchMissile.Invoke();
        }
    }

    public void OnConfirmGameOver(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onConfirmGameOver.Invoke();
        }
    }

    public void OnShift(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onShift.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            onShiftStop.Invoke();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool a_Input;
        public bool b_Input;
        public bool x_Input;
        public bool y_Input;
        public bool rb_Input;
        public bool rt_Input;
        public bool lb_Input;
        public bool lt_Input;
        public bool critical_Attack_Input;
        public bool jump_Input;
        public bool inventory_Input;
        public bool lockOnInput;
        public bool right_Staick_Right_Input;
        public bool right_Staick_Left_Input;

        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;
        public bool d_Pad_Up;

        public bool rollFlag;
        public bool twoHandFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public bool lockOnFlag;
        public bool inventoryFlag;
        public bool isInteracting;
        public float rollInputTimer;

        public Transform criticalAttackRayCastStartPoint;

        PlayerControls inputActions;
        PlayerCombatManager playerCombatManager;
        PlayerInventory playerInventory;
        PlayerManager playerManager;
        PlayerStatsManager playerStatsManager;
        PlayerEffectsManager playerEffectsManager;
        BlockingCollider blockingCollider;
        WeaponSlotManager weaponSlotManager;
        UIManager uiManager;
        CameraHandler cameraHandler;
        PlayerAnimatorManager playerAnimatorManager;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
            blockingCollider = GetComponentInChildren<BlockingCollider>();
            uiManager = FindObjectOfType<UIManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
        }

        public void OnEnable()
        {
            // 若操作為空
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

                inputActions.PlayerActions.RB.performed += i => rb_Input = true;
                inputActions.PlayerActions.RT.performed += i => rt_Input = true;
                inputActions.PlayerActions.LB.performed += i => lb_Input = true;
                inputActions.PlayerActions.LB.canceled += i => lb_Input = false;
                inputActions.PlayerActions.LT.performed += i => lt_Input = true;
                inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
                inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
                inputActions.PlayerActions.A.performed += i => a_Input = true;
                inputActions.PlayerActions.X.performed += i => x_Input = true;
                inputActions.PlayerActions.Roll.performed += i => b_Input = true;
                inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
                inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
                inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Staick_Right_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Staick_Left_Input = true;
                inputActions.PlayerActions.Y.performed += i => y_Input = true;
                inputActions.PlayerActions.CriticalAttack.performed += i =>  critical_Attack_Input = true;
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            if (playerStatsManager.isDead)
                return;

            HandleMoveInput(delta);
            HandleRollInput(delta);
            HandleLBInput();
            HandleCombatInput(delta);
            HandleQuickSlotsInput();
            HandleInventoryInput();
            HandleLockOnInput();
            HandleTwoHandInput();
            HandleCriticalAttackInput();
            HandleUseConsumableInput();
        }

        private void HandleMoveInput(float delta)
        {
            if (playerManager.isAiming)
            {
                // 水平方向為X軸
                horizontal = movementInput.x;
                // 垂直方向為Y軸
                vertical = movementInput.y;
                moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical) / 2);
                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
            else
            {
                // 水平方向為X軸
                horizontal = movementInput.x;
                // 垂直方向為Y軸
                vertical = movementInput.y;
                moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
        }

        private void HandleRollInput(float delta)
        {
            // b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;
            // b_Input = inputActions.PlayerActions.Roll.triggered;
            // b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
            // b_Input = inputActions.PlayerActions.Roll.IsPressed();

            if (b_Input)
            {
                rollInputTimer += delta;
                if (playerStatsManager.currentStamina <= 0)
                {
                    b_Input = false;
                    sprintFlag = false;
                }

                if (moveAmount > 0.5 && playerStatsManager.currentStamina > 0)
                {
                    sprintFlag = true;
                }
            }

            else
            {
                sprintFlag = false;

                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }


        private void HandleCombatInput(float delta)
        {
            if (rb_Input)
            {
                playerCombatManager.HandleRBAction();
            }

            if (rt_Input)
            {
                playerCombatManager.HandleHeavyAttack(playerInventory.rightWeapon);
            }


            if (lt_Input)
            {
                //假如正在兩手狀態控制武器
                if (twoHandFlag)
                {
                    
                }
                else
                {
                    playerCombatManager.HandleLtAction();
                }
                //其他控制輕攻擊假如是物理武器
                //假如是盾牌控制武器
            }

        }

        private void HandleLBInput()
        {
            if(playerManager.isInAir||
                playerManager.isSprinting||
                playerManager.isFiringSpell)
            {
                lb_Input = false;
                return;
            }

            if (lb_Input)
            {
                playerCombatManager.HandleLBAction();
            }
            else if(lb_Input==false)
            {
                playerManager.isBlocking = false;

                if (blockingCollider.blockingCollider.enabled)
                {
                    blockingCollider.DisableBlockingCollider();
                }
                if (playerManager.isAiming)
                {
                    playerAnimatorManager.animator.SetBool("isAiming", false);
                }
            }
        }

        // 快速換武器
        private void HandleQuickSlotsInput()
        {
            
            if (d_Pad_Right)
            {
                playerInventory.ChangeRightWeapon();
            }
            if (d_Pad_Left)
            {
                playerInventory.ChangeLeftWeapon();
            }
        }


        private void HandleInventoryInput()
        {
            if (inventory_Input)
            {
                inventoryFlag = !inventoryFlag;

                if (inventoryFlag)
                {
                    uiManager.OpenSelectWindow();
                    uiManager.UpdateUI();
                    uiManager.hudWindow.SetActive(false);
                }

                else
                {
                    uiManager.CloseSelectWindow();
                    uiManager.CloseAllInventoryWindow();
                    uiManager.hudWindow.SetActive(true);
                }
            }
        }

        private void HandleLockOnInput()
        {
            if (lockOnInput && lockOnFlag == false)
            {
                lockOnInput = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOnInput && lockOnFlag)
            {
                lockOnInput = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTargets();
                // 清除鎖定目標
            }

            if (lockOnFlag && right_Staick_Left_Input)
            {
                right_Staick_Left_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }

            if (lockOnFlag && right_Staick_Right_Input)
            {
                right_Staick_Right_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }

            cameraHandler.SetCameraHeight();
        }

        private void HandleTwoHandInput()
        {
            if (y_Input)
            {
                y_Input = false;

                twoHandFlag = !twoHandFlag;

                if (twoHandFlag)
                {
                    //啟用雙手

                    playerManager.isTwoHand = true;
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                    weaponSlotManager.LoadTwoHandIKTargets(true);
                }
                else
                {
                    //暫停雙手

                    playerManager.isTwoHand = false;
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
                    weaponSlotManager.LoadTwoHandIKTargets(false);
                }
            }
        }

        private void HandleCriticalAttackInput()
        {
            if (critical_Attack_Input)
            {
                critical_Attack_Input = false;
                playerCombatManager.AttemptBackStabOrRiposte();
            }
        }

        private void HandleUseConsumableInput()
        {
            if (x_Input)
            {
                x_Input = false;
                // 使用當前元素瓶
                playerInventory.currentConsumable.AttemptToConsume(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
            }

        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class PlayerManager : CharacterManager
    {
        InputHandler inputHandler;
        Animator animator;
        CameraHandler cameraHandler;
        PlayerAnimatorManager playerAnimatorManager;
        PlayerEffectsManager playerEffectsManager;
        PlayerStatsManager playerStatsManager;
        PlayerCn playerCn;
        
        InteractableUI interactableUI;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;



        protected override void Awake()
        {
            base.Awake();
            cameraHandler = FindObjectOfType<CameraHandler>();
            backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            inputHandler = GetComponent<InputHandler>();
            animator = GetComponent<Animator>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerCn = GetComponent<PlayerCn>();
            interactableUI = FindObjectOfType<InteractableUI>();

        }


        void Update()
        {
            float delta = Time.deltaTime;
            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("canDoCombo");
            isUsingRightHand = animator.GetBool("isUsingRightHand");
            isUsingLeftHand = animator.GetBool("isUsingLeftHand");
            animator.SetBool("isTwoHand",isTwoHand);
            isInvulnerable = animator.GetBool("isInvulnerable");
            isFiringSpell = animator.GetBool("isFiringSpell");
            isAiming = animator.GetBool("isAiming");

            animator.SetBool("isBlocking", isBlocking);
            animator.SetBool("isInAir", isInAir);
            animator.SetBool("isDead", playerStatsManager.isDead);

            inputHandler.TickInput(delta);
            playerAnimatorManager.canRotate = animator.GetBool("canRotate");
            playerCn.HandleJumping();
            playerCn.HandRollingAndSprinting(delta);
            playerStatsManager.RegenerateStamina();

            CheckForInteractsbleObject();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            float delta = Time.fixedDeltaTime;

            playerCn.HandleMovement(delta);
            playerCn.HandleFalling(delta, playerCn.moveDirection);
            playerCn.HandleRotation(delta);
            playerEffectsManager.HandleAllBuildUpEffects();
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            inputHandler.lt_Input = false;
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.a_Input = false;
            inputHandler.jump_Input = false;
            inputHandler.inventory_Input = false;
            
            isSprinting = inputHandler.b_Input;

            float delta = Time.deltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }

            if (isInAir)
            {
                playerCn.inAirTimer = playerCn.inAirTimer + Time.deltaTime;
            }
        }

        #region 玩家物品
        public void CheckForInteractsbleObject()
        {
            RaycastHit hit;

            if(Physics.SphereCast(transform.position,0.3f,transform.forward,out hit, 1f))
            {
                if (hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if (interactableObject != null)
                    {
                        string interactableText = interactableObject.interactbleText;
                        interactableUI.interactableText.text = interactableText; //設定物體文本
                        interactableUIGameObject.SetActive(true); //設定撿到為真

                        if (inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            else
            {
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if (itemInteractableGameObject !=null && inputHandler.a_Input)
                {
                    itemInteractableGameObject.SetActive(false);
                }
            }
        }

        public void OpenChestInteraction(Transform playerStandHereWhenOpeningChest)
        {
            playerCn.rigidbody.velocity = Vector3.zero; // 停止角色動作
            transform.position = playerStandHereWhenOpeningChest.transform.position;
            playerAnimatorManager.PlayTargetAnimation("Open Chest", true);

        }

        public void PassThroghFogWallInteraction(Transform fogWallEnterance)
        {
            playerCn.rigidbody.velocity = Vector3.zero; // 停止角色動作

            Vector3 rotationDirection = fogWallEnterance.transform.forward;
            Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = turnRotation;

            playerAnimatorManager.PlayTargetAnimation("PassThrough Fog", true);
        }

        #endregion
    }
}


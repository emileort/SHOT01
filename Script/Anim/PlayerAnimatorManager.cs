using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class PlayerAnimatorManager : AnimatorManager
    {
        
        InputHandler inputHandler;
        PlayerCn playerLocomotion;

        int vertical;
        int horizontal;

        void Start()
        {
            animator = this.GetComponent<Animator>();
        }


        protected override void Awake()
        {
            base.Awake();

            inputHandler = GetComponentInParent<InputHandler>();
            animator = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponentInParent<PlayerCn>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        // private void UpdateAnumStatus()
        // {
        //     anim.SetFloat("animTime", Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));
        // 
        //     anim.ResetTrigger("atk");
        // }

        // private void Atk()
        // {
        //     if (inputHander.rb_Input)
        //     {
        //         anim.SetTrigger("atk");
        //     }
        // }

        public void UpdateAnimatorValues(float verticalMovement,float horizontalMovement,bool isSprinting)
        {
            #region Vertical
            float v = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }

            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }

            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v= -0.5f;
            }

            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }

            else
            {
                v = 0;
            }
            #endregion

            #region Horizontal

            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }

            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }

            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }

            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }

            else
            {
                h = 0;
            }
            #endregion

            if (isSprinting && verticalMovement > 0)
            {
                v = 2;
                h = horizontalMovement;
            }

            animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void DisableCollision()
        {
            playerLocomotion.characterCollider.enabled = false;
            playerLocomotion.characterCollisionBlockerCollider.enabled = false;
        }

        public void EnableCollision()
        {
            playerLocomotion.characterCollider.enabled = true;
            playerLocomotion.characterCollisionBlockerCollider.enabled = true;
        }

        private void OnAnimatorMove()
        {
            if (characterManager.isInteracting == false)
                return;

            float delta = Time.deltaTime;
            playerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotion.rigidbody.velocity = velocity;

        }

    }
}


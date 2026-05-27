using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace EMO
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator animator;
        protected CharacterManager characterManager;
        protected CharacterStatsManager characterStatsManager;
        public bool canRotate;

        protected RigBuilder rigBuilder;
        public TwoBoneIKConstraint leftHandConstraint;
        public TwoBoneIKConstraint rightHandConstraint;

        bool handIKWeightReset = false;


        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            rigBuilder = GetComponent<RigBuilder>();
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false,bool mirrorAnim=false)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("canRotate", canRotate);
            animator.SetBool("isInteracting", isInteracting);
            animator.SetBool("isMirrored", mirrorAnim);
            animator.CrossFade(targetAnim, 0.2f);
        }

        public void PlayerTargetAnimationWithRootRotation(string targetAnim,bool isInteracting)
        {
            animator.applyRootMotion= isInteracting;
            animator.SetBool("isRotatingWithRootMotion", true);
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnim, 0.2f);
        }


        public virtual void CanRotate()
        {
            animator.SetBool("canRotate", true);
        }

        public virtual void StopRotation()
        {
            animator.SetBool("canRotate", false);
        }

        public virtual void EnableCombo()
        {
            animator.SetBool("canDoCombo", true);
        }

        public virtual void DisableCombo()
        {
            animator.SetBool("canDoCombo", false);
        }

        public virtual void EnableIsInvulnerable()
        {
            animator.SetBool("isInvulnerable", true);
        }

        public virtual void DisableIsInvulnerable()
        {
            animator.SetBool("isInvulnerable", false);
        }


        public virtual void EnableIsParrying()
        {
            characterManager.isParrying = true;
        }

        public virtual void DisableIsParrying()
        {
            characterManager.isParrying = false;
        }

        public virtual void EnableCanBeRiposted()
        {
            characterManager.canBeRiposted = true;
        }

        public virtual void DisableCanBeRiposted()
        {
            characterManager.canBeRiposted = false;
        }

        public virtual void TakeCriticalDamageAnimationEvent()
        {
            characterStatsManager.TakeDamageNoAnimtion(characterManager.pendingCriticalDamage ,0);
            characterManager.pendingCriticalDamage = 0;
        }

        public virtual void SetHandIKForWeapon(RightHandTarget rightHandTarget,LeftHandTarget leftHandTarget,bool isTwoHandingWeapon)
        {
            //檢查假如我們雙持武器
            if (isTwoHandingWeapon)
            {
                //假如我們有，同意手的IK，假如需要
                if (rightHandTarget != null)
                {
                    rightHandConstraint.data.target = rightHandTarget.transform;
                    rightHandConstraint.data.targetPositionWeight = 1;
                    rightHandConstraint.data.targetRotationWeight = 1;
                }
                if (leftHandTarget != null)
                {
                    leftHandConstraint.data.target = leftHandTarget.transform;
                    leftHandConstraint.data.targetPositionWeight = 1;
                    leftHandConstraint.data.targetRotationWeight = 1;
                }

            }
            else
            {
                rightHandConstraint.data.target = null;
                leftHandConstraint.data.target = null;
            }

            rigBuilder.Build();
        }

        public virtual void CheckHandIKWeight(RightHandTarget rightHandIK,LeftHandTarget leftHandIK,bool isTwoHandinWeapon)
        {
            if (characterManager.isInteracting)
                return;
            if (handIKWeightReset)
            {
                handIKWeightReset = false;

                if (rightHandConstraint.data.target != null)
                {
                    rightHandConstraint.data.target = rightHandIK.transform;
                    rightHandConstraint.data.targetPositionWeight = 1;
                    rightHandConstraint.data.targetRotationWeight = 1;
                }
                if (leftHandConstraint.data.target != null)
                {
                    leftHandConstraint.data.target = leftHandIK.transform;
                    leftHandConstraint.data.targetPositionWeight = 1;
                    leftHandConstraint.data.targetRotationWeight = 1;
                }
            }
        }

        public virtual void EraseHandIKForWeapon()
        {
            handIKWeightReset = true;

            //限制全部雙手IK的權重為0
            if (rightHandConstraint.data.target != null)
            {
                rightHandConstraint.data.targetPositionWeight = 0;
                rightHandConstraint.data.targetRotationWeight = 0;
            }
            if (leftHandConstraint.data.target != null) 
            {
                leftHandConstraint.data.targetPositionWeight = 1;
                leftHandConstraint.data.targetRotationWeight = 1;
            }
            
        }

    }
}


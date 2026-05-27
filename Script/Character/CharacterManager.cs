using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class CharacterManager : MonoBehaviour
    {
        AnimatorManager animatorManager;
        CharacterWeaponSlotManager characterWeaponSlotManager;
        
        [Header("鎖定位置")]
        public Transform lockOnTransform;

        [Header("戰鬥載體")]
        public CriticalDamageCollider backStabCollider;
        public CriticalDamageCollider riposteCollider;

        [Header("掛空")]
        public bool isInteracting;

        [Header("狀態旗幟")]
        public bool canBeRiposted;
        public bool canBeParried;
        public bool canDoCombo;
        public bool isParrying;
        public bool isBlocking;
        public bool isInvulnerable;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
        public bool isTwoHand;
        public bool isAiming;

        [Header("移動旗幟")]
        public bool isRotatingWithRootMotion;
        public bool canRotate;
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;


        [Header("法術")]
        public bool isFiringSpell;

        // 傷害將在動畫事件上呈現
        // 用背刺動畫
        public int pendingCriticalDamage;

        protected virtual void Awake()
        {
            animatorManager = GetComponent<AnimatorManager>();
            characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
        }

        protected virtual void FixedUpdate()
        {
            animatorManager.CheckHandIKWeight(characterWeaponSlotManager.rightHandIKTarget, characterWeaponSlotManager.leftHandIKTarget, isTwoHand);
        }
    }
}


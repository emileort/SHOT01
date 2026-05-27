using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        protected CharacterManager characterManager;
        protected CharacterInventoryManager characterInventoryManager;
        protected CharacterEffectsManager characterEffectsManager;
        protected CharacterStatsManager characterStatsManager;
        protected AnimatorManager animatorManager;

        
        [Header("空手武器")]
        public WeaponItem unarmedWeapon;

        [Header("武器替換")]
        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;
        public WeaponHolderSlot backSlot;

        [Header("傷害碰撞體")]
        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        [Header("攻擊武器")]
        public WeaponItem attackingWeapon;

        [Header("手部IK目標")]
        public RightHandTarget rightHandIKTarget;
        public LeftHandTarget leftHandIKTarget;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
            characterInventoryManager = GetComponent<CharacterInventoryManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            animatorManager = GetComponent<AnimatorManager>();
            LoadWeaponHolderSlots();
        }

        public void Start()
        {
            
        }

        protected virtual void LoadWeaponHolderSlots()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.isBackSlot)
                {
                    backSlot = weaponSlot;
                }
            }
        }
        
        public virtual void LoadBothWeaponsOnslots()
        {
            LoadWeaponOnSlot(characterInventoryManager.rightWeapon, false);
            LoadWeaponOnSlot(characterInventoryManager.leftWeapon, true);
        }

        public virtual void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (weaponItem != null)
            {
                if (isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();

                }
                else
                {
                    if (characterManager.isTwoHand)
                    {
                        //移動目前左手武器後面或不可見它
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        //雙持時，左手待機動畫
                        animatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();

                    }

                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    LoadTwoHandIKTargets(characterManager.isTwoHand);
                    animatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }

            }

            else
            {
                weaponItem = unarmedWeapon;

                if (isLeft)
                {
                    characterInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    animatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    characterInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    animatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }

        }


        protected virtual void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            // leftHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();

            // 調取各種傷害
            leftHandDamageCollider.physicalDamage = characterInventoryManager.leftWeapon.physicalDamage;
            leftHandDamageCollider.fireDamage = characterInventoryManager.leftWeapon.fireDamage;

            // 調用隊伍ID，以免TK
            leftHandDamageCollider.characterManager = characterManager;
            leftHandDamageCollider.teamIDNumber = characterStatsManager.teamIDNumber;

            // 調用玩家武器特效與左手武器破勢數值
            leftHandDamageCollider.poiseBreak = characterInventoryManager.leftWeapon.poiseBreak;
            characterEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }

        protected virtual void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            // rightHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();

            // 調取各種傷害
            rightHandDamageCollider.physicalDamage = characterInventoryManager.rightWeapon.physicalDamage;
            rightHandDamageCollider.fireDamage = characterInventoryManager.rightWeapon.fireDamage;
            // 調用隊伍ID，以免TK
            rightHandDamageCollider.characterManager = characterManager;
            rightHandDamageCollider.teamIDNumber = characterStatsManager.teamIDNumber;

            // 調用玩家武器特效與右手武器破勢數值
            rightHandDamageCollider.poiseBreak = characterInventoryManager.rightWeapon.poiseBreak;
            characterEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }

        public virtual void LoadTwoHandIKTargets(bool isTwoHandingWeapon)
        {
            leftHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<LeftHandTarget>();
            rightHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<RightHandTarget>();

            animatorManager.SetHandIKForWeapon(rightHandIKTarget, leftHandIKTarget, isTwoHandingWeapon);
        }

        public virtual void OpenDamageCollider()
        {
            if (characterManager.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
            else if (characterManager.isUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
        }

        public virtual void CloseDamageCollider()
        {
            if (rightHandDamageCollider != null)
            {
                rightHandDamageCollider.DisaleDamageCollider();
            }
            if (leftHandDamageCollider != null)
            {
                leftHandDamageCollider.DisaleDamageCollider();
            }
        }

        public virtual void GrantWeaponAttackingPoiseBonus()
        {
            characterStatsManager.totalPoiseDefence = characterStatsManager.totalPoiseDefence + characterStatsManager.offensivePoiseBouns;
        }

        public virtual void ResetWeaponAttackingPoiseBonus()
        {
            characterStatsManager.totalPoiseDefence = characterStatsManager.armorPoiseBonus;
        }


    }
}

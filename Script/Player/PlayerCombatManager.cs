using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class PlayerCombatManager : MonoBehaviour
    {
        CameraHandler cameraHandler;
        PlayerAnimatorManager playerAnimatorHandler;
        PlayerEquipmentManager playerEquipmentManager;
        PlayerManager playerManager;
        PlayerStatsManager playerStats;
        PlayerInventory playerInventory;
        PlayerEffectsManager playerEffectsManager;
        InputHandler inputHander;
        WeaponSlotManager weaponSlotManager;

        [Header("攻擊動畫")]
        string oh_light_attack_1 = "OH_Light_Attack_01";
        string oh_light_attack_2 = "OH_Light_Attack_02";
        string oh_heavy_attack_1 = "OH_Heavy_Attack_01";
        string th_light_attack_1 = "TH_Light_Attack_01";
        string th_light_attack_2 = "TH_Light_Attack_02";
        string th_heavy_attack_1 = "TH_Heavy_Attack_01";

        string weapon_art = "Parry";

        public string lastAttack;

        LayerMask backStabLayer = 1 << 13;
        LayerMask riposteLayer = 1 << 14;

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            playerAnimatorHandler = GetComponent<PlayerAnimatorManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStatsManager>();
            playerInventory = GetComponent<PlayerInventory>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
            inputHander = GetComponent<InputHandler>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            //確認我們有多少耐力，假如沒有足夠的，就返回
            if (playerStats.currentStamina <= 0)
                return;

            if (inputHander.comboFlag)
            {
                playerAnimatorHandler.animator.SetBool("canDoCombo", false);

                if (lastAttack == oh_light_attack_1)
                {
                    playerAnimatorHandler.PlayTargetAnimation(oh_light_attack_2, true);
                }
                else if (lastAttack == th_light_attack_1)
                {
                    playerAnimatorHandler.PlayTargetAnimation(th_light_attack_2, true);
                }
            }
            
        }

        public void HandleLightAttack(WeaponItem weapon)
        {

            //確認我們有多少耐力，假如沒有足夠的，就返回
            if (playerStats.currentStamina <= 0)
                return;

            weaponSlotManager.attackingWeapon = weapon;

            if (inputHander.twoHandFlag)
            {

                playerAnimatorHandler.animator.SetBool("isTwoHand", true);
                playerAnimatorHandler.PlayTargetAnimation(th_light_attack_1, true);
                lastAttack = th_light_attack_1;
            }
            else
            {
                playerAnimatorHandler.PlayTargetAnimation(oh_light_attack_1, true);
                lastAttack = oh_light_attack_1;
            }
        }

        public void HandleLightAttack02(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;
            playerAnimatorHandler.PlayTargetAnimation(oh_light_attack_2, true);
            lastAttack = oh_light_attack_1;
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            //確認我們有多少耐力，假如沒有足夠的，就返回
            if (playerStats.currentStamina <= 0)
                return;

            weaponSlotManager.attackingWeapon = weapon;

            if (inputHander.twoHandFlag)
            {

                playerAnimatorHandler.animator.SetBool("isTwoHand", true);
                playerAnimatorHandler.PlayTargetAnimation(th_heavy_attack_1, true);
                lastAttack = th_heavy_attack_1;
            }
            else
            {
                playerAnimatorHandler.PlayTargetAnimation(oh_heavy_attack_1, true);
                lastAttack = oh_light_attack_1;
            }
        }

        #region 輸入動作
        public void HandleRBAction()
        {
            playerAnimatorHandler.EraseHandIKForWeapon();

            if (playerInventory.rightWeapon.weaponType==WeaponType.StraightSword
                || playerInventory.rightWeapon.weaponType == WeaponType.Unarmed)
            {
                PerformRBMeleeAction();
            }
            else if (playerInventory.rightWeapon.weaponType == WeaponType.SpellCaster ||
                playerInventory.rightWeapon.weaponType == WeaponType.FaithCaster
                || playerInventory.rightWeapon.weaponType == WeaponType.PyromancyCaster)
            {
                PerformMagicAction(playerInventory.rightWeapon,true);
            }

        }

        public void HandleLBAction()
        {
            if (playerManager.isTwoHand)
            {
                if (playerInventory.rightWeapon.weaponType == WeaponType.Bow)
                {
                    PerformLBAimingAction();
                }
            }
            else
            {
                if (playerInventory.leftWeapon.weaponType == WeaponType.Shield ||
                    playerInventory.leftWeapon.weaponType == WeaponType.StraightSword) 
                {

                    PerformLBBlockingAction();

                }
                else if(playerInventory.leftWeapon.weaponType == WeaponType.FaithCaster ||
                    playerInventory.leftWeapon.weaponType == WeaponType.PyromancyCaster)
                {
                    PerformMagicAction(playerInventory.leftWeapon, true);
                    playerAnimatorHandler.animator.SetBool("isUsingLeftHand", true);

                }
            }
        }


        public void HandleLtAction()
        {
            if (playerInventory.leftWeapon.weaponType==WeaponType.Shield
                || playerInventory.leftWeapon.weaponType == WeaponType.Unarmed)
            {
                PerformLTWeaponArt(inputHander.twoHandFlag);
            }
            else if (playerInventory.leftWeapon.weaponType == WeaponType.StraightSword)
            {
                PerformMagicAction(playerInventory.leftWeapon, true);
            }
        }

        #endregion

        #region 攻擊動作
        private void PerformRBMeleeAction()
        {
            // 判定是否能進行連擊
            if (playerManager.canDoCombo)
            {
                inputHander.comboFlag = true;
                HandleWeaponCombo(playerInventory.rightWeapon);
                inputHander.comboFlag = false;
            }
            // 假如否，則返回空
            else
            {

                if (playerManager.isInteracting)
                    return;

                if (playerManager.canDoCombo)
                    return;

                playerAnimatorHandler.animator.SetBool("isUsingRightHand", true);
                HandleLightAttack(playerInventory.rightWeapon);
            }

            //播放特效
            playerEffectsManager.PlayWeaponFX(false);
        }

        private void PerformLBAimingAction()
        {
            playerAnimatorHandler.animator.SetBool("isAiming",true);
        }

        private void PerformMagicAction(WeaponItem weapon, bool isLeftHanded)
        {
            if (playerManager.isInteracting) return;
            
            if (weapon.weaponType == WeaponType.FaithCaster)
            {
                if (playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
                {
                    //確認FP
                    if (playerStats.currentFocusPoints >= playerInventory.currentSpell.focusPointCost)
                    {
                        //嘗試施放法術
                        playerInventory.currentSpell.AttemptToCastSpell(playerAnimatorHandler, playerStats, weaponSlotManager,isLeftHanded);
                    }
                    else
                    {
                        playerAnimatorHandler.PlayTargetAnimation("Shrug", true);
                    }
                }
            }
            else if (weapon.weaponType == WeaponType.PyromancyCaster)
            {
                if (playerInventory.currentSpell != null && playerInventory.currentSpell.isPyroSpell)
                {
                    //確認FP
                    if (playerStats.currentFocusPoints >= playerInventory.currentSpell.focusPointCost)
                    {
                        //嘗試施放法術
                        playerInventory.currentSpell.AttemptToCastSpell(playerAnimatorHandler, playerStats, weaponSlotManager,isLeftHanded);
                    }
                    else
                    {
                        playerAnimatorHandler.PlayTargetAnimation("Shrug", true);
                    }
                }
            }
        }

        private void PerformLTWeaponArt(bool isTwoHanding)
        {
            if (playerManager.isInteracting)
                return;

            if (isTwoHanding)
            {
                //假如我們雙手狀態

            }
            else
            {
                //假如我們一般狀態
                playerAnimatorHandler.PlayTargetAnimation(weapon_art, true);
                

            }

        }

        private void SuccessfullyCastSpell()
        {
            playerInventory.currentSpell.SuccessfullyCastSpell(playerAnimatorHandler, playerStats, cameraHandler, weaponSlotManager,playerManager.isUsingLeftHand);
            playerAnimatorHandler.animator.SetBool("isFiringSpell", true);

        }

        #endregion

        #region 防禦動作
        
        private void PerformLBBlockingAction()
        {
            if (playerManager.isInteracting)
                return;

            if (playerManager.isBlocking)
                return;

            playerAnimatorHandler.PlayTargetAnimation("Block Start", false, true);

            playerEquipmentManager.OpenBlockingCollider();

            playerManager.isBlocking = true;
        }

        #endregion

        public void AttemptBackStabOrRiposte()
        {

            //確認我們有多少耐力，假如沒有足夠的，就返回
            if (playerStats.currentStamina <= 0)
                return;

            RaycastHit hit;

            if(Physics.Raycast(inputHander.criticalAttackRayCastStartPoint.position,
                transform.TransformDirection(Vector3.forward),out hit, 0.5f, backStabLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

                if (enemyCharacterManager != null)
                {
                    //檢查隊伍ID(隊友與自己的背後位)
                    playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamageStandPoint.position;
                    //推
                    //轉視角我們的位置
                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMuiltiplier * rightWeapon.physicalDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    //播放動畫
                    playerAnimatorHandler.PlayTargetAnimation("Back Stab", true);
                    //讓敵人播放動畫
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
                    //造成傷害



                }
            }

            else if (Physics.Raycast(inputHander.criticalAttackRayCastStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

                if (enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
                {
                    playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamageStandPoint.position;

                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();

                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);

                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMuiltiplier * rightWeapon.physicalDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    playerAnimatorHandler.PlayTargetAnimation("Riposte", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Riposted", true);
                }
                
            }
        }

    }

}

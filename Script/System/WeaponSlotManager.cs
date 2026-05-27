using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class WeaponSlotManager : CharacterWeaponSlotManager
    {
        QuickSlotsUI quickSlotsUI;
        InputHandler inputHandler;

        PlayerManager playerManager;
        PlayerInventory playerInventory;
        PlayerStatsManager playerStatsManager;
        PlayerEffectsManager playerEffectsManager;
        PlayerAnimatorManager playerAnimatorManager;
        CameraHandler cameraHandler;


        protected override void Awake()
        {
            base.Awake();
            cameraHandler = FindObjectOfType<CameraHandler>();
            inputHandler = GetComponent<InputHandler>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerManager = GetComponent<PlayerManager>();
            playerInventory = GetComponent<PlayerInventory>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();

        }
        
        public override void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (weaponItem != null)
            {
                if (isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                    playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false,true);

                }
                else
                {
                    if (inputHandler.twoHandFlag)
                    {
                        //移動目前左手武器後面或不可見它
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        playerAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();

                    }

                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                    playerAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }

            }

            else
            {
                weaponItem = unarmedWeapon;
                
                if (isLeft)
                {
                    playerInventory.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                    playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    playerInventory.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                    playerAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }

        }

        public void SucessfullyTrowFireBomb()
        {
            Destroy(playerEffectsManager.instantiatedFXModel);
            BombConsumeableItem fireBombItem = playerInventory.currentConsumable as BombConsumeableItem;

            GameObject activeModelBomb = Instantiate(fireBombItem.liveBombModel,
                rightHandSlot.transform.position, cameraHandler.cameraPiovtTransform.rotation);
            activeModelBomb.transform.rotation = Quaternion.Euler(cameraHandler.cameraPiovtTransform.eulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0);
            // 炸彈傷害碰撞體
            BombDamageCollider damageCollider = activeModelBomb.GetComponentInChildren<BombDamageCollider>();
            // 增加距離投出在空中的剛體移動
            damageCollider.explosiveRadius = fireBombItem.baseDamage;
            damageCollider.explosionSplashDamage = fireBombItem.explosiveDamage;
            damageCollider.bombRigidbody.AddForce(activeModelBomb.transform.forward * fireBombItem.forwardVelocity);
            damageCollider.bombRigidbody.AddForce(activeModelBomb.transform.up * fireBombItem.upwardVelocity);
            damageCollider.teamIDNumber = playerStatsManager.teamIDNumber;

            LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            // 確認爆炸火焰
            // 創造爆炸與之後的傷害特效

        }



        #region 控制武器耐力值
        public void DrainStaminaLightAttack()
        {
            if (playerStatsManager != null)
            {
                playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
            }
            
        }

        public void DrainStaminaHeavyAttack()
        {
            if (playerStatsManager != null)
            {
                playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
            }
            
        }
        #endregion

    }
}

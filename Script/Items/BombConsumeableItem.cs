using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    [CreateAssetMenu(menuName = "Item/Consumables/Bomb Item")]
    public class BombConsumeableItem : ConsumableItem
    {
        [Header("變數")]
        public int upwardVelocity = 50;
        public int forwardVelocity = 50;
        public int bombMass = 1;

        [Header("類炸彈模型")]
        public GameObject liveBombModel;

        [Header("基礎傷害")]

        public int baseDamage = 200;
        public int explosiveDamage = 75;

        public override void AttemptToConsume(PlayerAnimatorManager playerAnimatorManager, WeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
        {
            if (currentItemAmount > 0)
            {
                //當物件出現時，先不讀取右手武器
                weaponSlotManager.rightHandSlot.UnloadWeapon();
                playerAnimatorManager.PlayTargetAnimation(consumeAnimation, true);
                // 物件該從右手出現
                GameObject bombModel = Instantiate(itemModel,
                    weaponSlotManager.rightHandSlot.transform.position,
                    Quaternion.identity, weaponSlotManager.rightHandSlot.transform);
                // 調用特效
                playerEffectsManager.instantiatedFXModel = bombModel;
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation("Shrug", true);
            }
        }
    }
}

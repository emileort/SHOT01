using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class ConsumableItem : Item
    {
        [Header("物品")]
        public int maxItemAmount;
        public int currentItemAmount;

        [Header("物品模型")]
        public GameObject itemModel;

        [Header("動畫")]
        public string consumeAnimation;
        public bool isInteracting;

        public virtual void AttemptToConsume(PlayerAnimatorManager playerAnimatorManager, WeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
        {
            if (currentItemAmount > 0)
            {
                playerAnimatorManager.PlayTargetAnimation(consumeAnimation, isInteracting, true);
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation("Shrug", true);
            }
        }


    }
}
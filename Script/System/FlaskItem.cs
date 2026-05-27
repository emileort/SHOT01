using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    [CreateAssetMenu(menuName ="Item/Consumables/Flask")]
    public class FlaskItem : ConsumableItem
    {
        [Header("瓶子類型")]
        public bool estusFlask;
        public bool ashenFlask;

        [Header("回復量")]
        public int healthRecoverAmount;
        public int focusPointsRecoverAmount;

        [Header("回復特效")]
        public GameObject recoveryFX;

        public override void AttemptToConsume(PlayerAnimatorManager playerAnimatorManager,
            WeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
        {
            base.AttemptToConsume(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
            GameObject flask = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
            // 增加血量與魔力
            playerEffectsManager.currentParticleFX = recoveryFX;
            playerEffectsManager.amountToBeHealed = healthRecoverAmount;
            playerEffectsManager.instantiatedFXModel = flask;
            // 瓶子在玩家手上的動畫
            weaponSlotManager.rightHandSlot.UnloadWeapon();
            // 玩家回復特效，假如我們喝下
        }
    }
}

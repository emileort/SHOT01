using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    [CreateAssetMenu(menuName = "Item/Consumables/Cure Effect Clump")]
    public class ClumpConsumeableItem : ConsumableItem
    {

        [Header("回復特效")]
        public GameObject clumpConsumeFX;


        [Header("康復特效")]
        public bool curePoison;

        public override void AttemptToConsume(PlayerAnimatorManager playerAnimatorManager,
            WeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
        {
            base.AttemptToConsume(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
            GameObject clump = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
            // 增加血量與魔力
            playerEffectsManager.currentParticleFX = clumpConsumeFX;
            playerEffectsManager.instantiatedFXModel = clump;
            if (curePoison)
            {
                playerEffectsManager.poisonBuildup = 0;
                playerEffectsManager.poisonAmount = playerEffectsManager.defaultPoisonAmount;
                playerEffectsManager.isPoisoned = false;

                if (playerEffectsManager.currentPoisonParticleFX != null)
                {
                    Destroy(playerEffectsManager.currentPoisonParticleFX);
                }
            }
            // 瓶子在玩家手上的動畫
            weaponSlotManager.rightHandSlot.UnloadWeapon();
            // 玩家回復特效，假如我們喝下
        }
    }
}

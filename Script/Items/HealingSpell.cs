using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    [CreateAssetMenu(menuName ="Spells/Healing Spell")]
    public class HealingSpell : SpellItem
    {
        public int healAmount;

        public override void AttemptToCastSpell(
            PlayerAnimatorManager animatorHandler,
            PlayerStatsManager playerStats,
            WeaponSlotManager weaponSlotManager,
            bool isLeftHanded)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager,isLeftHanded);
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, animatorHandler.transform);
            animatorHandler.PlayTargetAnimation(spellAnimation, true,false,isLeftHanded);
            Debug.Log("嘗試施放魔法....");
        }

        public override void SuccessfullyCastSpell(
            PlayerAnimatorManager animatorHandler,
            PlayerStatsManager playerStats,
            CameraHandler cameraHandler,
            WeaponSlotManager weaponSlotManager,
            bool isLeftHanded)
        {
            base.SuccessfullyCastSpell(animatorHandler, playerStats, cameraHandler, weaponSlotManager,isLeftHanded);
            GameObject instantiatedSpellFX = Instantiate(spellCastFX, animatorHandler.transform);
            playerStats.HealPlayer(healAmount);
            Debug.Log("魔法施放成功....");
        }

    }

}
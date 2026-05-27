using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class SpellItem : Item
    {
        public GameObject spellWarmUpFX;
        public GameObject spellCastFX;
        public string spellAnimation;

        [Header("法術消耗")]
        public int focusPointCost;

        [Header("法術類別")]

        public bool isFaithSpell;
        public bool isMagicSpell;
        public bool isPyroSpell;

        [Header("法術描述")]
        [TextArea]
        public string sepllDescription;

        public virtual void AttemptToCastSpell(
            PlayerAnimatorManager animatorHandler, 
            PlayerStatsManager playerStats,
            WeaponSlotManager weaponSlotManager,
            bool isLeftHanded)
        {
            Debug.Log("你試圖施放法術");
        }
        public virtual void SuccessfullyCastSpell(
            PlayerAnimatorManager animatorHandler,
            PlayerStatsManager playerStats,
            CameraHandler cameraHandler,
            WeaponSlotManager weaponSlotManager,
            bool isLeftHanded)
        {
            Debug.Log("你成功施放法術!");
            playerStats.DeductFocusPoints(focusPointCost);
        }

    }
}


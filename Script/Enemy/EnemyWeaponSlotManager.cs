using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{

    public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
    {

        public override void GrantWeaponAttackingPoiseBonus()
        {
            characterStatsManager.totalPoiseDefence = characterStatsManager.totalPoiseDefence + characterStatsManager.offensivePoiseBouns;
        }

        public override void ResetWeaponAttackingPoiseBonus()
        {
            characterStatsManager.totalPoiseDefence = characterStatsManager.armorPoiseBonus;
        }

    }
}

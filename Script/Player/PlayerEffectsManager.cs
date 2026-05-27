using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        PlayerStatsManager playerStatsManager;
        WeaponSlotManager weaponSlotManager;

        PoisonBuildUpBar poisonBuildUpBar;
        PoisonAmountBar poisonAmountBar;

        public GameObject currentParticleFX; // 元素瓶將會在玩家喝下元素瓶後產生特效
        public GameObject instantiatedFXModel;
        public int amountToBeHealed;

        protected override void Awake()
        {
            base.Awake();
            playerStatsManager = GetComponentInParent<PlayerStatsManager>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();

            poisonBuildUpBar = FindObjectOfType<PoisonBuildUpBar>();
            poisonAmountBar = FindObjectOfType<PoisonAmountBar>();
        }

        public void HealPlayerFromEffect()
        {
            playerStatsManager.HealPlayer(amountToBeHealed);
            GameObject healParticles = Instantiate(currentParticleFX, playerStatsManager.transform);
            Destroy(instantiatedFXModel.gameObject);
            weaponSlotManager.LoadBothWeaponsOnslots();
        }

        protected override void HandlePoisonBuildUp()
        {
            if (poisonBuildup <= 0)
            {
                poisonBuildUpBar.gameObject.SetActive(false);
            }
            else
            {
                poisonBuildUpBar.gameObject.SetActive(true);
            }
            base.HandlePoisonBuildUp();
            poisonBuildUpBar.SetPoisonBuildUpAmount(Mathf.RoundToInt(poisonBuildup));
        }

        protected override void HandleIsPoisonedEffect()
        {
            if (isPoisoned == false)
            {
                poisonAmountBar.gameObject.SetActive(false);
            }
            else
            {
                poisonAmountBar.gameObject.SetActive(true);
            }

            base.HandleIsPoisonedEffect();
            poisonAmountBar.SetPoisonAmount(Mathf.RoundToInt(poisonAmount));
        }
    }
}


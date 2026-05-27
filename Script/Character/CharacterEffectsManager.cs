using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        CharacterStatsManager characterStatsManager;
        
        [Header("傷害特效")]
        public GameObject bloodSplatterFX;

        [Header("武器特效")]
        public WeaponFX rightWeaponFX;
        public WeaponFX leftWeaponFX;

        [Header("毒狀態")]
        public GameObject defaultPoisonParticleFX;
        public GameObject currentPoisonParticleFX;
        public Transform buildUpTransform; //座標的累積量粒子特效將會
        public bool isPoisoned;
        public float poisonBuildup = 0; // 超過這個數值之後倒數100
        public float poisonAmount = 100; //累積到滿之前都會往100數
        public float defaultPoisonAmount = 100;  //累積多少才會進入毒狀態
        public float poisonTimer = 2; //記數在這中間會造成的傷害
        public int poisonDamage = 1;
        float timer;

        protected virtual void Awake()
        {
            characterStatsManager = GetComponent<CharacterStatsManager>();
        }

        public virtual void PlayWeaponFX(bool isLeft)
        {
            if (isLeft == false)
            {
                //玩家右手武器軌跡
                if (rightWeaponFX != null)
                {
                    rightWeaponFX.PlayWeaponFX();
                }
            }
            else
            {
                //玩家左手武器軌跡
                if (leftWeaponFX != null)
                {
                    leftWeaponFX.PlayWeaponFX();
                }
            }
        }

        public virtual void PlayBloodSpatterFX(Vector3 bloodSplatterLocation)
        {
            GameObject blood = Instantiate(bloodSplatterFX, bloodSplatterLocation, Quaternion.identity);
        }

        public virtual void HandleAllBuildUpEffects()
        {
            if (characterStatsManager.isDead)
                return;


            HandlePoisonBuildUp();
            HandleIsPoisonedEffect();

        }


        protected virtual void HandlePoisonBuildUp()
        {
            if (isPoisoned)
                return;

            if (poisonBuildup > 0 && poisonBuildup < 100)
            {
                poisonBuildup = poisonBuildup - 1 * Time.deltaTime;
            }
            else if (poisonBuildup >= 100)
            {
                isPoisoned = true;
                poisonBuildup = 0;

                if (buildUpTransform != null) 
                {
                    currentPoisonParticleFX = Instantiate(defaultPoisonParticleFX, buildUpTransform.transform);
                }
                else
                {
                    currentPoisonParticleFX = Instantiate(defaultPoisonParticleFX, characterStatsManager.transform);
                }
            }
        }

        protected virtual void HandleIsPoisonedEffect()
        {
            if (isPoisoned)
            {
                if (poisonAmount > 0)
                {
                    timer += Time.deltaTime;

                    if (timer >= poisonTimer)
                    {
                        characterStatsManager.TakePoisonDamage(poisonDamage);
                        timer = 0;
                    }
                    poisonAmount = poisonAmount - 1 * Time.deltaTime;
                }
                else
                {
                    isPoisoned = false;
                    poisonAmount = defaultPoisonAmount;
                    Destroy(currentPoisonParticleFX);
                }

            }
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class CharacterStatsManager : MonoBehaviour
    {
        AnimatorManager animatorManager;
        

        [Header("團隊ID")]
        public int teamIDNumber = 0;
        
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public int staminaLevel = 10;
        public float maxStamina;
        public float currentStamina;

        public int focusPointsLevel = 10;
        public float maxFocusPoints;
        public float currentFocusPoints;

        public int soulCount = 0;
        public int soulsAwardedOnDeath = 1000;

        [Header("平衡")]

        public float totalPoiseDefence;
        public float offensivePoiseBouns;
        public float armorPoiseBonus;
        public float totalPoiseResetTimer = 15;
        public float poiseResetTimer = 0;

        [Header("傷害吸收")]
        public float physicalDamageAbsorptionHead;
        public float physicalDamageAbsorptionBody;
        public float physicalDamageAbsorptionLegs;
        public float physicalDamageAbsorptionHand;

        public float fireDamageAbsorptionHead;
        public float fireDamageAbsorptionBody;
        public float fireDamageAbsorptionLegs;
        public float fireDamageAbsorptionHand;



        public bool isDead;

        protected virtual void Awake()
        {
            animatorManager = GetComponent<AnimatorManager>();
        }

        protected virtual void Update()
        {
            HandlePoiseResetTimer();
        }

        private void Start()
        {
            totalPoiseDefence = armorPoiseBonus;
        }

        public virtual void TakeDamage(int physicalDamage,int fireDamage, string damageAnimation)
        {
            if (isDead)
                return;

            animatorManager.EraseHandIKForWeapon();

            // 簡易總傷害公式
            float totalPhysicalDamageAbsarption = 1 -
                (1 - physicalDamageAbsorptionHead / 100) *
                (1 - physicalDamageAbsorptionBody / 100) *
                (1 - physicalDamageAbsorptionHand / 100) *
                (1 - physicalDamageAbsorptionLegs / 100);

            physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsarption));

            Debug.Log("總共吸收" + totalPhysicalDamageAbsarption + "%");

            float totalFireDamageAbsorption = 1 -
                (1 - fireDamageAbsorptionHead / 100) *
                (1 - fireDamageAbsorptionBody / 100) *
                (1 - fireDamageAbsorptionLegs / 100) *
                (1 - fireDamageAbsorptionHead / 100);

            fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

            // 加上各種屬性傷害
            float finalDamage = physicalDamage + fireDamage; 

            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

            Debug.Log("總傷害是" + finalDamage);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead=true;
            }

        }

        public virtual void TakeDamageNoAnimtion(int physicalDamage, int fireDamage)
        {
            if (isDead)
                return;

            float totalPhysicalDamageAbsarption = 1 -
                (1 - physicalDamageAbsorptionHead / 100) *
                (1 - physicalDamageAbsorptionBody / 100) *
                (1 - physicalDamageAbsorptionHand / 100) *
                (1 - physicalDamageAbsorptionLegs / 100);

            physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsarption));

            Debug.Log("總共吸收" + totalPhysicalDamageAbsarption + "%");

            float totalFireDamageAbsorption = 1 -
                (1 - fireDamageAbsorptionHead / 100) *
                (1 - fireDamageAbsorptionBody / 100) *
                (1 - fireDamageAbsorptionLegs / 100) *
                (1 - fireDamageAbsorptionHead / 100);

            fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

            // 加上各種屬性傷害
            float finalDamage = physicalDamage + fireDamage; 

            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

            Debug.Log("總傷害是" + finalDamage);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public virtual void TakePoisonDamage(int damage)
        {
            currentHealth = currentHealth - damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public virtual void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else
            {
                totalPoiseDefence = armorPoiseBonus;
            }
        }

    }
}

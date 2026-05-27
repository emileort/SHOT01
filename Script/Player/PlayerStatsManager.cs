using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager playerManager;

        public HealthBar healthBar;
        StaminaBar staminaBar;
        FocusPointsBar focusPointsBar;
        PlayerAnimatorManager playerAnimatorManager;

        public float staminaRegenerationAmount = 1;
        public float staminaRegenTimer = 0;


        private WaitForSeconds regenTicks = new WaitForSeconds(0.1f);
        private Coroutine regen;


        protected override void Awake()
        {
            base.Awake();
            playerManager = GetComponent<PlayerManager>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusPointsBar = FindObjectOfType<FocusPointsBar>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetCurrentHealth(currentHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
            staminaBar.SetCurrentStamina(currentStamina);

            maxFocusPoints = SetMaxFocusPointsFromFocusLevel();
            currentFocusPoints = maxFocusPoints;
            focusPointsBar.SetMaxFocusPoints(maxFocusPoints);
            focusPointsBar.SetCurrentFocusPoints(currentFocusPoints);

        }

        public override void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else if (poiseResetTimer <= 0 && !playerManager.isInteracting) 
            {
                totalPoiseDefence = armorPoiseBonus;
            }
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;

        }

        private float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;

        }

        private float SetMaxFocusPointsFromFocusLevel()
        {
            maxFocusPoints = focusPointsLevel * 10;
            return maxFocusPoints;

        }

        public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation)
        {
            if (playerManager.isInvulnerable)
                return;

            base.TakeDamage(physicalDamage, fireDamage, damageAnimation);
            healthBar.SetCurrentHealth(currentHealth);
            playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                playerAnimatorManager.PlayTargetAnimation("Dead_01", true);
            }

        }

        public override void TakeDamageNoAnimtion(int physicalDamage, int fireDamage)
        {
            base.TakeDamageNoAnimtion(physicalDamage, fireDamage);
            healthBar.SetCurrentHealth(currentHealth);
        }

        public override void TakePoisonDamage(int damage)
        {
            if (isDead)
                return;

            base.TakePoisonDamage(damage);
            healthBar.SetCurrentHealth(currentHealth);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                playerAnimatorManager.PlayTargetAnimation("Dead_01", true);
            }

        }

        public void TakeStaminaDamage(int damage)
        {
            currentStamina = currentStamina - damage;

            staminaBar.SetCurrentStamina(currentStamina);

            if (regen != null)
            {
                StopCoroutine(regen);
            }
            regen = StartCoroutine(StaminaRegen());

        }

        public void RegenerateStamina()
        {
            if (playerManager.isInteracting)
            {
                staminaRegenTimer = 0;
            }

            else
            {

                staminaRegenTimer += Time.deltaTime;

                if (currentStamina < maxStamina && staminaRegenTimer > 1f)
                {
                    currentStamina += staminaRegenerationAmount * Time.deltaTime;
                    staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));

                }
            }

        }

        public void HealPlayer(int healAmount)
        {
            currentHealth = currentHealth + healAmount;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            healthBar.SetCurrentHealth(currentHealth);
        }

        private IEnumerator StaminaRegen()
        {
            yield return new WaitForSeconds(2);

            while (currentStamina < maxStamina)
            {
                currentStamina += maxStamina / 100;
                staminaBar.SetCurrentStamina(currentStamina);
                yield return regenTicks;
            }

            regen = null;
        }

        public void DeductFocusPoints(int focusPoints)
        {
            currentFocusPoints = currentFocusPoints - focusPoints;

            if (currentFocusPoints < 0)
            {
                currentFocusPoints = 0;
            }

            focusPointsBar.SetCurrentFocusPoints(currentFocusPoints);
        }

        public void AddSouls(int souls)
        {
            soulCount = soulCount + souls;
        }

    }
}


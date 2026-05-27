using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class EnemyStatsManager : CharacterStatsManager
    {
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyBossManager enemyBossManager;
        WorldEventManager worldEventManager;
        UIBossHealthBar bossHealthBar;
        public UIEnemyHealthBar enemyHealthBar;

        public bool isBoss;

        protected override void Awake()
        {
            base.Awake();
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            enemyBossManager = GetComponent<EnemyBossManager>();
            bossHealthBar = FindObjectOfType<UIBossHealthBar>();
            worldEventManager = GetComponentInParent<WorldEventManager>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        void Start()
        {
            if (!isBoss)
            {
                enemyHealthBar.SetMaxHealth(maxHealth);
            }
            
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public override void TakeDamageNoAnimtion(int physicalDamage, int fireDamage)
        {
            base.TakeDamageNoAnimtion(physicalDamage, fireDamage);

            if (!isBoss)
            {
                enemyHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }

        }


        public override void TakePoisonDamage(int damage)
        {
            if (isDead)
                return;

            base.TakePoisonDamage(damage);

            if (!isBoss)
            {
                enemyHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
            }

        }


        public void BreakGuard()
        {
            enemyAnimatorManager.PlayTargetAnimation("Break Guard", true);
        }


        public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation)
        {
            
            base.TakeDamage(physicalDamage, fireDamage, damageAnimation);

            if (!isBoss)
            {
                enemyHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && enemyBossManager != null) 
            {
                enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }

            enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (!isBoss)
            {
                if (currentHealth <= 0)
                {
                    HandleDeath();
                }
            }
            else if(isBoss && enemyBossManager != null)
            {
                if (currentHealth <= 0)
                {
                    HandleDeath();
                    bossHealthBar.SetHealthBarToInactive();
                }
            }
        }


        public void HandleDeath()
        {
            currentHealth = 0;
            enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
            isDead = true;
        }

        

    }
}


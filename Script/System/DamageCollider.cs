using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
        protected Collider damageCollider;
        public bool enabledDamageColliderOnStarUp = false;

        [Header("團隊ID")]
        public int teamIDNumber = 0;

        [Header("姿勢")]

        public float poiseBreak;
        public float offensivePoiseBouns;

        [Header("傷害")]

        public int physicalDamage;
        public int fireDamage;
        public int magicDamage;
        public int lightningDamage;
        public int darkDamage;

        bool shieldHasBeenHit;
        bool hasBeenParried;
        protected string currentDamageAnimation;

        protected virtual void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = enabledDamageColliderOnStarUp;
        }
        
        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisaleDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if(collision.tag == "Character")
            {
                shieldHasBeenHit = false;
                hasBeenParried = false;

                CharacterStatsManager enemyStats = collision.GetComponent<CharacterStatsManager>();
                CharacterManager enemyManager = collision.GetComponent<CharacterManager>();
                CharacterEffectsManager enemyEffects = collision.GetComponent<CharacterEffectsManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                if (enemyManager != null)
                {
                    if (enemyStats.teamIDNumber == teamIDNumber)
                        return;

                    CheckForParry(enemyManager);
                    CheckForBlock(enemyManager,enemyStats,shield);
                }



                if (enemyStats != null)
                {
                    if (enemyStats.teamIDNumber == teamIDNumber)
                        return;

                    if (hasBeenParried)
                        return;
                    if (shieldHasBeenHit)
                        return;

                    enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTimer;
                    enemyStats.totalPoiseDefence = enemyStats.totalPoiseDefence - poiseBreak;

                    Debug.Log("玩家當前姿勢" + enemyStats.totalPoiseDefence);

                    Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    float directionHitFrom = Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up);
                    ChooseWithDirectionDamageCameFrom(directionHitFrom);
                    enemyEffects.PlayBloodSpatterFX(contactPoint);

                    if (enemyStats.totalPoiseDefence > poiseBreak)
                    {
                        enemyStats.TakeDamageNoAnimtion(physicalDamage, 0);
                    }
                    else
                    {
                        enemyStats.TakeDamage(physicalDamage, 0, currentDamageAnimation);
                    }
                }
            }

            if(collision.tag=="Illusionary Wall")
            {
                IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();

                illusionaryWall.wallHasBeenHit = true;
            }

        }

        protected virtual void CheckForParry(CharacterManager enemyManager)
        {

            if (enemyManager.isParrying)
            {
                // 確認假如你防禦中
                characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                hasBeenParried = true;
            }
        }

        protected virtual void CheckForBlock(CharacterManager enemyManager,CharacterStatsManager enemyStats , BlockingCollider shield)
        {
            if (shield != null && enemyManager.isBlocking)
            {
                float pyhsicalDamageAfterBlock = physicalDamage - (physicalDamage * shield.blockingPhysicalDamageAbsarption) / 100;
                float fireDamageAfterBlock = fireDamage - (fireDamage * shield.blockingFireDamageAbsorption) / 100;

                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(Mathf.RoundToInt(pyhsicalDamageAfterBlock), 0, "Block End");
                    shieldHasBeenHit = true;
                }
            }
        }

        protected virtual void ChooseWithDirectionDamageCameFrom(float direction)
        {
            if (direction >= 145 && direction <= 100)
            {
                currentDamageAnimation = "Damage_Forward_01";
            }
            else if (direction <= -145 && direction >= -100)
            {
                currentDamageAnimation = "Damage_Forward_01";
            }
            else if (direction >= -45 && direction <= 45)
            {
                currentDamageAnimation = "Damage_Back_01";
            }
            else if (direction >= -144 && direction <= -45)
            {
                currentDamageAnimation = "Damage_Left_01";
            }
            else if (direction >= 45 && direction <= 144)
            {
                currentDamageAnimation = "Damage_Right_01";
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{

    public class AmbushState : State
    {
        public bool isSleeping;
        public float detectionRadius = 2;
        public string sleepAnimation;
        public string wakeAnimation;

        public LayerMask detectionLayer;
        
        public PursueTargetState pursueTargetState;

        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (isSleeping && enemyManager.isInteracting == false)
            {
                enemyAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
            }
            #region 北ヘ夹浪代

            Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, detectionLayer);

            for(int i = 0; i < colliders.Length; i++)
            {
                CharacterStatsManager characterStats= colliders[i].transform.GetComponent<CharacterStatsManager>();
                if (characterStats != null)
                {
                    Vector3 targetDirection = characterStats.transform.position - enemyManager.transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

                    if(viewableAngle>enemyManager.minimumDirectionAngle
                        && viewableAngle < enemyManager.maximumDirectionAngle)
                    {
                        enemyManager.currentTarget = characterStats;
                        isSleeping = false;
                        enemyAnimatorManager.PlayTargetAnimation(wakeAnimation, true);
                    }
                }
            }

            #endregion

            #region 北篈ち传

            if (enemyManager.currentTarget != null)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }

            #endregion
        }
    }
}

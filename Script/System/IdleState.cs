using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class IdleState : State
    {
        public PursueTargetState pursueTargetState;
        public LayerMask detectionLayer;
        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            #region 控制敵人目標距離
            //看到目標
            //選擇狀態與找尋目標
            //假如不返回這狀態

            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
               CharacterStatsManager characterStats = colliders[i].transform.GetComponent<CharacterStatsManager>();

                if (characterStats != null)
                {
                    // 確認團隊ID
                    if (characterStats.teamIDNumber != enemyStats.teamIDNumber)
                    {
                        Vector3 targetDirection = characterStats.transform.position - transform.position;
                        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                        if (viewableAngle > enemyManager.minimumDirectionAngle && viewableAngle < enemyManager.maximumDirectionAngle)
                        {
                            enemyManager.currentTarget = characterStats;
                        }
                    }
                }
            }
            #endregion

            #region 控制下一個狀態
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


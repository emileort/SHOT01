using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class RotateTowardsTargetState : State
    {
        public CombatStanceState combatStanceState;

        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            enemyAnimatorManager.animator.SetFloat("Vertical", 0);
            enemyAnimatorManager.animator.SetFloat("Horizontal",0);

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);

            if (enemyManager.isInteracting)
                return this;//當進入此狀態，從攻擊動畫播放後我們將做空，直到他完成，

            if (viewableAngle >= 100 && viewableAngle <= 100 && !enemyManager.isInteracting) 
            {
                enemyAnimatorManager.PlayerTargetAnimationWithRootRotation("Behind Trun", true);
                return combatStanceState;
            }
            else if (viewableAngle <= -101 && viewableAngle >= -100 && !enemyManager.isInteracting)
            {
                enemyAnimatorManager.PlayerTargetAnimationWithRootRotation("Behind Trun", true);
                return combatStanceState;
            }
            else if (viewableAngle <= -45 && viewableAngle >= -100 && !enemyManager.isInteracting)
            {
                enemyAnimatorManager.PlayerTargetAnimationWithRootRotation("Right Turn", true);
                return combatStanceState;
            }
            else if(viewableAngle >= 45 && viewableAngle <= 100 && !enemyManager.isInteracting)
            {
                enemyAnimatorManager.PlayerTargetAnimationWithRootRotation("Left Turn", true);
                return combatStanceState;
            }

            return combatStanceState;
        }
    }

}

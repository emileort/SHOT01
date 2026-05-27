using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class WorldEventManager : MonoBehaviour
    {
        public List<FogWall> fogWalls;
        public UIBossHealthBar bossHealthBar;
        public EnemyBossManager boss;

        public bool bossFightIsActive; //當前Boss的戰鬥開關
        public bool bossHasBeenAwakened; //喚醒Boss與在戰鬥結束前的狀態
        public bool bossHasBeenDefeated; //Boss戰敗後

        private void Awake()
        {
            bossHealthBar = FindObjectOfType<UIBossHealthBar>();
            boss = FindObjectOfType<EnemyBossManager>();
        }


        public void ActivateBossFight()
        {
            bossFightIsActive = true;
            bossHasBeenAwakened = true;
            bossHealthBar.SetIBossHealthBarToActive();

            foreach(var fogWall in fogWalls)
            {
                fogWall.ActivateFogWall();
            }
        }

        public void BossHasBeenDefeated()
        {
            bossFightIsActive = false;
            bossHasBeenAwakened = false;
            bossHasBeenDefeated = true;

            bossHealthBar.SetHealthBarToInactive();

            foreach (var fogWall in fogWalls)
            {
                fogWall.DeactivateFogWall();
            }

        }

    }
}

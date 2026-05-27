using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    [CreateAssetMenu(menuName ="Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("動畫分歧")]
        public AnimatorOverrideController weaponController;
        public string offHandIdleAnimation= "Left_Arm_idle_01";

        [Header("武器類型")]
        public WeaponType weaponType;

        [Header("傷害")]
        public int physicalDamage;
        public int fireDamage;
        public int criticalDamageMuiltiplier = 4;

        [Header("姿勢")]

        public float poiseBreak;
        public float offensivePoiseBouns;

        [Header("防禦")]
        public float physicalDamageAbsorption;

        /*[Header("待機動畫")]
        public string right_hand_idle;
        public string left_hand_idle;
        public string th_idle;

        [Header("單手攻擊動畫")]
        public string OH_Light_Attack_1;
        public string OH_Light_Attack_2;
        public string OH_Heavy_Attack_1;
        public string Th_light_Attack_1;
        public string Th_light_Attack_2;
        public string Th_Heavy_Attack_1;
        
        */
        [Header("武器藝術")]
        public string Weapon_art;

        [Header("耐力消耗")]
        public int baseStamina;
        public float lightAttackMultiplier;
        public float heavyAttackMultiplier;

    }

}

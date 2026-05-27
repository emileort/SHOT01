using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{

    public class CharacterInventoryManager : MonoBehaviour
    {
        protected CharacterWeaponSlotManager characterWeaponSlotManager;

        [Header("快速換道具")]
        public ConsumableItem currentConsumable;
        public SpellItem currentSpell;
        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;

        [Header("當前裝備")]
        public HelmetEquipment currentHelmetEquipment;
        public TorsoEquipment currentTorsoEquipment;
        public LegEquipment currentLegEquipment;
        public HandEquipment currentHandEquipment;


        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
        public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];

        public int currentRightWeaponIndex = -1;
        public int currentLeftWeaponIndex = -1;

        private void Awake()
        {
            characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
        }

        private void Start()
        {
            characterWeaponSlotManager.LoadBothWeaponsOnslots();
        }


    }
}

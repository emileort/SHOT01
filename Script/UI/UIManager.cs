using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMO
{
    public class UIManager : MonoBehaviour
    {
        public PlayerInventory playerInventory;
        public EquipmentWindowUI equipmentWindowUI;
        private QuickSlotsUI quickSlotsUI;

        [Header("UIµøµ¡")]
        public GameObject hudWindow;
        public GameObject selectWindow;
        public GameObject equipmentScreenWindon;
        public GameObject weaponInventoryWindow;
        public GameObject equipmentWindow;

        [Header("«ö¶s")]

        [SerializeField] Button mainMenuButton;

        [Header("¸Ë³Æ¿ï¾Üµ¡")]
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool rightHandSlot03Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;
        public bool leftHandSlot03Selected;

        [Header("ªZ¾¹®w")]
        public GameObject weaponInventorySlotPrefab;
        public Transform weaponInventorySlotsParent;
        WeaponInventorySlot[] weaponInventorySlots;

        void OnEnable()
        {
            mainMenuButton.onClick.AddListener(OnmainMenuButton);
        }

        private void Awake()
        {
            quickSlotsUI = GetComponentInChildren<QuickSlotsUI>();
        }

        private void Start()
        {
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
            quickSlotsUI.UpdateCurrentSpellIcon(playerInventory.currentSpell);
            quickSlotsUI.UpdateCurrentConsumableIcon(playerInventory.currentConsumable);
        }

        public void UpdateUI()
        {
            #region ªZ¾¹®w¤Á´«
            for(int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if (i < playerInventory.weaponsInventory.Count)
                {
                    if (weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
        }
        public void OpenSelectWindow()
        {
            selectWindow.SetActive(true);
        }

        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
        }

        public void CloseAllInventoryWindow()
        {
            ResetAllSelectedSlots();
            weaponInventoryWindow.SetActive(false);
            equipmentScreenWindon.SetActive(false);
        }

        public void ResetAllSelectedSlots()
        {
            rightHandSlot01Selected = false;
            rightHandSlot02Selected = false;
            rightHandSlot03Selected = false;
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = false;
            leftHandSlot03Selected = false;
        }

        void OnmainMenuButton()
        {
            SceneLoader.Instance.LoadMainMenuScene();
        }

    }
}


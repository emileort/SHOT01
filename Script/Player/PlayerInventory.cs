using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMO
{
    public class PlayerInventory : CharacterInventoryManager
    {
        public List<WeaponItem> weaponsInventory;

        public void ChangeRightWeapon()
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;

            // if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
            // {
            //     currentRightWeaponIndex = -1;
            //     rightWeapon = unarmedWeapon;
            //     weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
            // }

            // else if (weaponsInRightHandSlots[currentRightWeaponIndex] != null)
            // {
            //     rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            //     weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            // }

            // else
            // {
            //     currentRightWeaponIndex = currentRightWeaponIndex + 1;
            // }

            // 若當前右手武器指數為0且右手武器數列0不等於空
            if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
            {
                // 右手武器=右手武器數列(當前右手武器指數)
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                characterWeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            }
            // 若當前右手武器指數為0且右手武器數列0等於空
            else if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
            {
                // 當前右手武器指數+1
                currentRightWeaponIndex = currentRightWeaponIndex + 1;
            }
            // 若當前右手武器指數為1且右手武器數列1不等於空
            else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
            {
                // 右手武器=右手武器數列(當前右手武器指數)
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                characterWeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            }
            // 若當前右手武器指數為1且右手武器數列1等於空
            else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] == null) 
            {
                // 當前右手武器指數+1
                currentRightWeaponIndex = currentRightWeaponIndex + 1;
            }
            // 若當前右手武器指數為1且右手武器數列2不等於空
            else if (currentRightWeaponIndex == 2 && weaponsInRightHandSlots[2] != null)
            {
                // 右手武器=右手武器數列(當前右手武器指數)
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                characterWeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            }
            // 若當前右手武器指數為2且右手武器數列2等於空
            else if (currentRightWeaponIndex == 2 && weaponsInRightHandSlots[2] == null)
            {
                // 當前右手武器指數+1
                currentRightWeaponIndex = currentRightWeaponIndex + 1;
            }
            // 若當前右手武器指數大於右手武器數列長度-1
            if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
            {
                currentRightWeaponIndex = -1;
                rightWeapon = characterWeaponSlotManager.unarmedWeapon;
                characterWeaponSlotManager.LoadWeaponOnSlot(characterWeaponSlotManager.unarmedWeapon, false);
            }

        }


        public void ChangeLeftWeapon()
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;

            // if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
            // {
            //     currentLeftWeaponIndex = -1;
            //     leftWeapon = unarmedWeapon;
            //     weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);
            // }

            // else if (weaponsInLeftHandSlots[currentLeftWeaponIndex] != null)
            // {
            //     leftWeapon = weaponsInLeftHandSlots[currentRightWeaponIndex];
            //     weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            // }

            // else
            // {
            //     currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            // }
            if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] != null)
            {
                leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                characterWeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            }

            else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] == null)
            {
                currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            }

            else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] != null)
            {
                leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                characterWeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            }

            else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] == null)
            {
                currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            }

            else if (currentLeftWeaponIndex == 2 && weaponsInLeftHandSlots[2] != null)
            {
                leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                characterWeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            }

            else if (currentLeftWeaponIndex == 2 && weaponsInLeftHandSlots[2] == null)
            {
                currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            }

            if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
            {
                currentLeftWeaponIndex = -1;
                leftWeapon = characterWeaponSlotManager.unarmedWeapon;
                characterWeaponSlotManager.LoadWeaponOnSlot(characterWeaponSlotManager.unarmedWeapon, true);
            }

        }
    }

}

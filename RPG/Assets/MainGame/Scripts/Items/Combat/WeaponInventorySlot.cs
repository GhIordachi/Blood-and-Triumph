using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI {
    public class WeaponInventorySlot : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        WeaponItem weapon;

        private void Awake()
        {
            uiManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(WeaponItem newItem)
        {
            weapon = newItem;
            icon.sprite = weapon.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearInventorySlot()
        {
            weapon = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void EquipThisItem()
        {
            if(uiManager.rightHandSlot01Selected && weapon.isWeaponRightHanded)
            {
                if (uiManager.player.playerStatsManager.strengthLevel >= weapon.strengthLevelRequirement 
                    && uiManager.player.playerStatsManager.faithLevel >= weapon.faithLevelRequirement)
                {
                    if (uiManager.player.playerInventoryManager.weaponsInRightHandSlots[0] != null)
                        uiManager.player.playerInventoryManager.weaponsInventory.Add(uiManager.player.playerInventoryManager.weaponsInRightHandSlots[0]);
                    uiManager.player.playerInventoryManager.weaponsInRightHandSlots[0] = weapon;
                    uiManager.player.playerInventoryManager.weaponsInventory.Remove(weapon);
                }
            }
            else if(uiManager.rightHandSlot02Selected && weapon.isWeaponRightHanded)
            {
                if (uiManager.player.playerStatsManager.strengthLevel >= weapon.strengthLevelRequirement
                    && uiManager.player.playerStatsManager.faithLevel >= weapon.faithLevelRequirement)
                {
                    if (uiManager.player.playerInventoryManager.weaponsInRightHandSlots[1] != null)
                        uiManager.player.playerInventoryManager.weaponsInventory.Add(uiManager.player.playerInventoryManager.weaponsInRightHandSlots[1]);
                    uiManager.player.playerInventoryManager.weaponsInRightHandSlots[1] = weapon;
                    uiManager.player.playerInventoryManager.weaponsInventory.Remove(weapon);
                }
            }
            else if (uiManager.rightHandSlot03Selected && weapon.isWeaponRightHanded)
            {
                if (uiManager.player.playerStatsManager.strengthLevel >= weapon.strengthLevelRequirement
                    && uiManager.player.playerStatsManager.faithLevel >= weapon.faithLevelRequirement)
                {
                    if (uiManager.player.playerInventoryManager.weaponsInRightHandSlots[2] != null)
                        uiManager.player.playerInventoryManager.weaponsInventory.Add(uiManager.player.playerInventoryManager.weaponsInRightHandSlots[2]);
                    uiManager.player.playerInventoryManager.weaponsInRightHandSlots[2] = weapon;
                    uiManager.player.playerInventoryManager.weaponsInventory.Remove(weapon);
                }
            }
            else if (uiManager.rightHandSlot04Selected && weapon.isWeaponRightHanded)
            {
                if (uiManager.player.playerStatsManager.strengthLevel >= weapon.strengthLevelRequirement
                    && uiManager.player.playerStatsManager.faithLevel >= weapon.faithLevelRequirement)
                {
                    if (uiManager.player.playerInventoryManager.weaponsInRightHandSlots[3] != null)
                        uiManager.player.playerInventoryManager.weaponsInventory.Add(uiManager.player.playerInventoryManager.weaponsInRightHandSlots[3]);
                    uiManager.player.playerInventoryManager.weaponsInRightHandSlots[3] = weapon;
                    uiManager.player.playerInventoryManager.weaponsInventory.Remove(weapon);
                }
            }
            else if (uiManager.leftHandSlot01Selected && !weapon.isWeaponRightHanded)
            {
                if (uiManager.player.playerStatsManager.strengthLevel >= weapon.strengthLevelRequirement
                    && uiManager.player.playerStatsManager.faithLevel >= weapon.faithLevelRequirement)
                {
                    if (uiManager.player.playerInventoryManager.weaponsInLeftHandSlots[0] != null)
                        uiManager.player.playerInventoryManager.weaponsInventory.Add(uiManager.player.playerInventoryManager.weaponsInLeftHandSlots[0]);
                    uiManager.player.playerInventoryManager.weaponsInLeftHandSlots[0] = weapon;
                    uiManager.player.playerInventoryManager.weaponsInventory.Remove(weapon);
                }
            }
            else if (uiManager.leftHandSlot02Selected && !weapon.isWeaponRightHanded)
            {
                if (uiManager.player.playerStatsManager.strengthLevel >= weapon.strengthLevelRequirement
                    && uiManager.player.playerStatsManager.faithLevel >= weapon.faithLevelRequirement)
                {
                    if (uiManager.player.playerInventoryManager.weaponsInLeftHandSlots[1] != null)
                        uiManager.player.playerInventoryManager.weaponsInventory.Add(uiManager.player.playerInventoryManager.weaponsInLeftHandSlots[1]);
                    uiManager.player.playerInventoryManager.weaponsInLeftHandSlots[1] = weapon;
                    uiManager.player.playerInventoryManager.weaponsInventory.Remove(weapon);
                }
            }
            else if (uiManager.leftHandSlot03Selected && !weapon.isWeaponRightHanded)
            {
                if (uiManager.player.playerStatsManager.strengthLevel >= weapon.strengthLevelRequirement
                    && uiManager.player.playerStatsManager.faithLevel >= weapon.faithLevelRequirement)
                {
                    if (uiManager.player.playerInventoryManager.weaponsInLeftHandSlots[2] != null)
                        uiManager.player.playerInventoryManager.weaponsInventory.Add(uiManager.player.playerInventoryManager.weaponsInLeftHandSlots[2]);
                    uiManager.player.playerInventoryManager.weaponsInLeftHandSlots[2] = weapon;
                    uiManager.player.playerInventoryManager.weaponsInventory.Remove(weapon);
                }
            }
            else if (uiManager.leftHandSlot04Selected && !weapon.isWeaponRightHanded)
            {
                if (uiManager.player.playerStatsManager.strengthLevel >= weapon.strengthLevelRequirement
                    && uiManager.player.playerStatsManager.faithLevel >= weapon.faithLevelRequirement)
                {
                    if (uiManager.player.playerInventoryManager.weaponsInLeftHandSlots[3] != null)
                        uiManager.player.playerInventoryManager.weaponsInventory.Add(uiManager.player.playerInventoryManager.weaponsInLeftHandSlots[3]);
                    uiManager.player.playerInventoryManager.weaponsInLeftHandSlots[3] = weapon;
                    uiManager.player.playerInventoryManager.weaponsInventory.Remove(weapon);
                }
            }
            else
            {
                return;
            }

            uiManager.player.playerInventoryManager.rightWeapon = uiManager.player.playerInventoryManager.weaponsInRightHandSlots[uiManager.player.playerInventoryManager.currentRightWeaponIndex];
            uiManager.player.playerInventoryManager.leftWeapon = uiManager.player.playerInventoryManager.weaponsInLeftHandSlots[uiManager.player.playerInventoryManager.currentLeftWeaponIndex];

            uiManager.player.playerWeaponSlotManager.LoadBothWeaponsOnSlots();

            uiManager.equipmentWindowUI.LoadWeaponOnEquipmentScreen(uiManager.player);
            uiManager.ResetAllSelectedSlots();
        }

        public void ShowItemStats()
        {
            uiManager.itemStatsWindowUI.UpdateWeaponItemStats(weapon);
        }
    }
}

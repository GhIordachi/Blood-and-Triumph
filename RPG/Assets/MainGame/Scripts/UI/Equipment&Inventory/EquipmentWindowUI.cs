using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI {
    public class EquipmentWindowUI : MonoBehaviour
    {
        public WeaponEquipmentSlotUI[] weaponEquipmentSlotUI;
        public HeadEquipmentSlotUI headEquipmentSlotUI;
        public BodyEquipmentSlotUI bodyEquipmentSlotUI;
        public LegEquipmentSlotUI legEquipmentSlotUI;
        public HandEquipmentSlotUI handEquipmentSlotUI;

        public void LoadWeaponOnEquipmentScreen(PlayerInventoryManager playerInventory)
        {
            for (int i = 0; i < weaponEquipmentSlotUI.Length; i++)
            {
                if (weaponEquipmentSlotUI[i].rightHandSlot01)
                {
                    weaponEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);
                }
                else if (weaponEquipmentSlotUI[i].rightHandSlot02)
                {
                    weaponEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);
                }
                else if (weaponEquipmentSlotUI[i].rightHandSlot03)
                {
                    weaponEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[2]);
                }
                else if (weaponEquipmentSlotUI[i].rightHandSlot04)
                {
                    weaponEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[3]);
                }
                else if (weaponEquipmentSlotUI[i].leftHandSlot01)
                {
                    weaponEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[0]);
                }
                else if (weaponEquipmentSlotUI[i].leftHandSlot02)
                {
                    weaponEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[1]);
                }
                else if (weaponEquipmentSlotUI[i].leftHandSlot03)
                {
                    weaponEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[2]);
                }
                else if (weaponEquipmentSlotUI[i].leftHandSlot04)
                {
                    weaponEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[3]);
                }
            }
        }

        public void LoadArmorOnEquipmentScreen(PlayerInventoryManager playerInventoryManager)
        {
            if(playerInventoryManager.currentHelmetEquipment != null)
            {
                headEquipmentSlotUI.AddItem(playerInventoryManager.currentHelmetEquipment);
            }
            else
            {
                headEquipmentSlotUI.ClearItem();
            }

            if (playerInventoryManager.currentBodyEquipment != null)
            {
                bodyEquipmentSlotUI.AddItem(playerInventoryManager.currentBodyEquipment);
            }
            else
            {
                bodyEquipmentSlotUI.ClearItem();
            }

            if (playerInventoryManager.currentLegEquipment != null)
            {
                legEquipmentSlotUI.AddItem(playerInventoryManager.currentLegEquipment);
            }
            else
            {
                legEquipmentSlotUI.ClearItem();
            }

            if (playerInventoryManager.currentHandEquipment != null)
            {
                handEquipmentSlotUI.AddItem(playerInventoryManager.currentHandEquipment);
            }
            else
            {
                handEquipmentSlotUI.ClearItem();
            }
        }
    }
}

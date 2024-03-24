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
        public RingItemSlotUI[] ringItemSlotUI;
        public SpellSlotUI spellSlotUI;
        public ConsumableSlotUI consumableSlotUI;
        public AmmoSlotUI ammoSlotUI;

        public void LoadWeaponOnEquipmentScreen(PlayerManager player)
        {
            for (int i = 0; i < weaponEquipmentSlotUI.Length; i++)
            {
                if (weaponEquipmentSlotUI[i].rightHandSlot01)
                {
                    weaponEquipmentSlotUI[i].AddItem(player.playerInventoryManager.weaponsInRightHandSlots[0]);
                }
                else if (weaponEquipmentSlotUI[i].rightHandSlot02)
                {
                    weaponEquipmentSlotUI[i].AddItem(player.playerInventoryManager.weaponsInRightHandSlots[1]);
                }
                else if (weaponEquipmentSlotUI[i].rightHandSlot03)
                {
                    weaponEquipmentSlotUI[i].AddItem(player.playerInventoryManager.weaponsInRightHandSlots[2]);
                }
                else if (weaponEquipmentSlotUI[i].rightHandSlot04)
                {
                    weaponEquipmentSlotUI[i].AddItem(player.playerInventoryManager.weaponsInRightHandSlots[3]);
                }
                else if (weaponEquipmentSlotUI[i].leftHandSlot01)
                {
                    weaponEquipmentSlotUI[i].AddItem(player.playerInventoryManager.weaponsInLeftHandSlots[0]);
                }
                else if (weaponEquipmentSlotUI[i].leftHandSlot02)
                {
                    weaponEquipmentSlotUI[i].AddItem(player.playerInventoryManager.weaponsInLeftHandSlots[1]);
                }
                else if (weaponEquipmentSlotUI[i].leftHandSlot03)
                {
                    weaponEquipmentSlotUI[i].AddItem(player.playerInventoryManager.weaponsInLeftHandSlots[2]);
                }
                else if (weaponEquipmentSlotUI[i].leftHandSlot04)
                {
                    weaponEquipmentSlotUI[i].AddItem(player.playerInventoryManager.weaponsInLeftHandSlots[3]);
                }
            }
        }

        public void LoadArmorOnEquipmentScreen(PlayerManager player)
        {
            if(player.playerInventoryManager.currentHelmetEquipment != null)
            {
                headEquipmentSlotUI.AddItem(player.playerInventoryManager.currentHelmetEquipment);
            }
            else
            {
                headEquipmentSlotUI.ClearItem();
            }

            if (player.playerInventoryManager.currentBodyEquipment != null)
            {
                bodyEquipmentSlotUI.AddItem(player.playerInventoryManager.currentBodyEquipment);
            }
            else
            {
                bodyEquipmentSlotUI.ClearItem();
            }

            if (player.playerInventoryManager.currentLegEquipment != null)
            {
                legEquipmentSlotUI.AddItem(player.playerInventoryManager.currentLegEquipment);
            }
            else
            {
                legEquipmentSlotUI.ClearItem();
            }

            if (player.playerInventoryManager.currentHandEquipment != null)
            {
                handEquipmentSlotUI.AddItem(player.playerInventoryManager.currentHandEquipment);
            }
            else
            {
                handEquipmentSlotUI.ClearItem();
            }
        }

        public void LoadRingItemOnEquipmentScreen(PlayerManager player)
        {
            for (int i = 0; i < ringItemSlotUI.Length; i++)
            {
                if (ringItemSlotUI[i].ringItem01)
                {
                    ringItemSlotUI[i].AddItem(player.playerInventoryManager.ringSlot01);
                }
                else if (ringItemSlotUI[i].ringItem02)
                {
                    ringItemSlotUI[i].AddItem(player.playerInventoryManager.ringSlot02);
                }
                else if (ringItemSlotUI[i].ringItem03)
                {
                    ringItemSlotUI[i].AddItem(player.playerInventoryManager.ringSlot03);
                }
                else if (ringItemSlotUI[i].ringItem04)
                {
                    ringItemSlotUI[i].AddItem(player.playerInventoryManager.ringSlot04);
                }
            }
        }

        public void LoadSpellOnEquipmentScreen(PlayerManager player)
        {
            if (player.playerInventoryManager.currentSpell != null)
            {
                spellSlotUI.AddItem(player.playerInventoryManager.currentSpell);
            }
            else
            {
                spellSlotUI.ClearItem();
            }
        }

        public void LoadConsumableOnEquipmentScreen(PlayerManager player)
        {
            if (player.playerInventoryManager.currentConsumable != null)
            {
                consumableSlotUI.AddItem(player.playerInventoryManager.currentConsumable);
            }
            else
            {
                consumableSlotUI.ClearItem();
            }
        }

        public void LoadAmmoOnEquipmentScreen(PlayerManager player)
        {
            if (player.playerInventoryManager.currentAmmo != null)
            {
                ammoSlotUI.AddItem(player.playerInventoryManager.currentAmmo);
            }
            else
            {
                ammoSlotUI.ClearItem();
            }
        }
    }
}

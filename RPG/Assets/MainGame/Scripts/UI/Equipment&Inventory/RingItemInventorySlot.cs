using RPGCharacterAnims;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class RingItemInventorySlot : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        RingItem item;

        private void Awake()
        {
            uiManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(RingItem newItem)
        {
            item = newItem;
            icon.sprite = item.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearInventorySlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void EquipThisItem()
        {
            if (uiManager.ringItemSlot01Selected)
            {
                if (uiManager.player.playerInventoryManager.ringSlot01 != null)
                    uiManager.player.playerInventoryManager.ringItemInventory.Add(uiManager.player.playerInventoryManager.ringSlot01);
                uiManager.player.playerInventoryManager.ringSlot01 = item;
                uiManager.player.playerInventoryManager.ringItemInventory.Remove(item);
            }
            else if (uiManager.ringItemSlot02Selected)
            {
                if (uiManager.player.playerInventoryManager.ringSlot02 != null)
                    uiManager.player.playerInventoryManager.ringItemInventory.Add(uiManager.player.playerInventoryManager.ringSlot02);
                uiManager.player.playerInventoryManager.ringSlot02 = item;
                uiManager.player.playerInventoryManager.ringItemInventory.Remove(item);
            }
            else if (uiManager.ringItemSlot03Selected)
            {
                if (uiManager.player.playerInventoryManager.ringSlot03 != null)
                    uiManager.player.playerInventoryManager.ringItemInventory.Add(uiManager.player.playerInventoryManager.ringSlot03);
                uiManager.player.playerInventoryManager.ringSlot03 = item;
                uiManager.player.playerInventoryManager.ringItemInventory.Remove(item);
            }
            else if (uiManager.ringItemSlot04Selected)
            {
                if (uiManager.player.playerInventoryManager.ringSlot04 != null)
                    uiManager.player.playerInventoryManager.ringItemInventory.Add(uiManager.player.playerInventoryManager.ringSlot04);
                uiManager.player.playerInventoryManager.ringSlot04 = item;
                uiManager.player.playerInventoryManager.ringItemInventory.Remove(item);
            }
            else
            {
                return;
            }

            uiManager.equipmentWindowUI.LoadRingItemOnEquipmentScreen(uiManager.player);
            uiManager.ResetAllSelectedSlots();
        }

        public void ShowItemStats()
        {
            uiManager.itemStatsWindowUI.UpdateRingItemStats(item);
        }
    }
}

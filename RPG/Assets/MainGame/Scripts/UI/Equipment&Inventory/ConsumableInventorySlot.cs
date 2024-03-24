using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class ConsumableInventorySlot : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        ConsumableItem item;

        private void Awake()
        {
            uiManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(ConsumableItem newItem)
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
            if (uiManager.consumableSlotSelected)
            {
                uiManager.player.playerInventoryManager.consumableInventory.Add(uiManager.player.playerInventoryManager.currentConsumable);
                uiManager.player.playerInventoryManager.currentConsumable = item;
                uiManager.player.playerInventoryManager.consumableInventory.Remove(item);
            }
            else
            {
                return;
            }

            uiManager.equipmentWindowUI.LoadConsumableOnEquipmentScreen(uiManager.player);
            uiManager.ResetAllSelectedSlots();
        }

        public void ShowItemStats()
        {
            //uiManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }
    }
}

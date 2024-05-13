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
                if (uiManager.player.playerStatsManager.strengthLevel >= item.strengthLevelRequirement
                    && uiManager.player.playerStatsManager.faithLevel >= item.faithLevelRequirement)
                {
                    if (uiManager.player.playerInventoryManager.currentConsumable != null)
                        uiManager.player.playerInventoryManager.consumableInventory.Add(uiManager.player.playerInventoryManager.currentConsumable);
                    uiManager.player.playerInventoryManager.currentConsumable = item;
                    uiManager.quickSlotsUI.UpdateCurrentConsumableIcon(item);
                    uiManager.player.playerInventoryManager.consumableInventory.Remove(item);
                }
            }
            else
            {
                return;
            }

            uiManager.equipmentWindowUI.LoadConsumableOnEquipmentScreen(uiManager.player);
            uiManager.ResetAllSelectedSlots();
        }

        public void PurchaseThisItem()
        {
            if (uiManager.player.playerInventoryManager.currentGold >= item.value)
            {
                uiManager.vendorSelectedConsumable = item;
                uiManager.confirmPurchaseWindow.SetActive(true);
                uiManager.CloseAllVendorsConfirmWindows();
                uiManager.confirmConsumablePurchaseWindow.SetActive(true);
                uiManager.notEnoughMoneyWindow.SetActive(false);
            }
            else
            {
                uiManager.confirmPurchaseWindow.SetActive(true);
                uiManager.notEnoughMoneyWindow.SetActive(true);
                uiManager.CloseAllVendorsConfirmWindows();
                uiManager.vendorSelectedConsumable = null;
            }
        }

        public void FinishPurchaseUI()
        {
            if (uiManager.vendorSelectedConsumable != null)
                FinishPurchase(uiManager.vendorSelectedConsumable);
        }

        public void FinishPurchase(ConsumableItem consumable)
        {
            uiManager.player.playerInventoryManager.consumableInventory.Add(consumable);
            uiManager.player.playerInventoryManager.currentGold = uiManager.player.playerInventoryManager.currentGold - consumable.value;
            if (uiManager.player.playerInventoryManager.currentGold < 0)
                uiManager.player.playerInventoryManager.currentGold = 0;
            uiManager.mageVendorInventoryManager.consumableInventory.Remove(consumable);
        }

        public void ShowItemStats()
        {
            //uiManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }
    }
}

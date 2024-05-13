using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class LegEquipmentInventorySlot : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        LegEquipment item;

        private void Awake()
        {
            uiManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(LegEquipment newItem)
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
            if (uiManager.legEquipmentSlotSelected)
            {
                if (uiManager.player.playerStatsManager.strengthLevel >= item.strengthLevelRequirement
                    && uiManager.player.playerStatsManager.faithLevel >= item.faithLevelRequirement)
                {
                    if (uiManager.player.playerInventoryManager.currentLegEquipment != null)
                    {
                        uiManager.player.playerInventoryManager.legEquipmentInventory.Add(uiManager.player.playerInventoryManager.currentLegEquipment);
                    }
                    uiManager.player.playerInventoryManager.currentLegEquipment = item;
                    uiManager.player.playerInventoryManager.legEquipmentInventory.Remove(item);
                    uiManager.player.playerEquipmentManager.EquipAllArmor();
                }
            }
            else
            {
                return;
            }

            uiManager.equipmentWindowUI.LoadArmorOnEquipmentScreen(uiManager.player);
            uiManager.ResetAllSelectedSlots();
        }

        public void PurchaseThisItem()
        {
            if (uiManager.player.playerInventoryManager.currentGold >= item.value)
            {
                uiManager.vendorSelectedLeg = item;
                uiManager.confirmPurchaseWindow.SetActive(true);
                uiManager.CloseAllVendorsConfirmWindows();
                uiManager.confirmLegPurchaseWindow.SetActive(true);
                uiManager.notEnoughMoneyWindow.SetActive(false);
            }
            else
            {
                uiManager.confirmPurchaseWindow.SetActive(true);
                uiManager.notEnoughMoneyWindow.SetActive(true);
                uiManager.CloseAllVendorsConfirmWindows();
                uiManager.vendorSelectedLeg = null;
            }
        }

        public void FinishPurchaseUI()
        {
            if (uiManager.vendorSelectedLeg != null)
                FinishPurchase(uiManager.vendorSelectedLeg);
        }

        public void FinishPurchase(LegEquipment leg)
        {
            uiManager.player.playerInventoryManager.legEquipmentInventory.Add(leg);
            uiManager.player.playerInventoryManager.currentGold = uiManager.player.playerInventoryManager.currentGold - leg.value;
            if (uiManager.player.playerInventoryManager.currentGold < 0)
                uiManager.player.playerInventoryManager.currentGold = 0;
            uiManager.armorVendorInventoryManager.legEquipmentInventory.Remove(leg);
        }

        public void ShowItemStats()
        {
            uiManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }
    }
}

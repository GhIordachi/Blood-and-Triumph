using RPGCharacterAnims;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class HeadEquipmentInventorySlot : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        HelmetEquipment item;

        private void Awake()
        {
            uiManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(HelmetEquipment newItem)
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
            if (uiManager.headEquipmentSlotSelected)
            {
                if (uiManager.player.playerStatsManager.strengthLevel >= item.strengthLevelRequirement
                    && uiManager.player.playerStatsManager.faithLevel >= item.faithLevelRequirement)
                {
                    if (uiManager.player.playerInventoryManager.currentHelmetEquipment != null)
                    {
                        uiManager.player.playerInventoryManager.headEquipmentInventory.Add(uiManager.player.playerInventoryManager.currentHelmetEquipment);
                    }
                    uiManager.player.playerInventoryManager.currentHelmetEquipment = item;
                    uiManager.player.playerInventoryManager.headEquipmentInventory.Remove(item);
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
                uiManager.vendorSelectedHelmet = item;
                uiManager.confirmPurchaseWindow.SetActive(true);
                uiManager.CloseAllVendorsConfirmWindows();
                uiManager.confirmHelmetPurchaseWindow.SetActive(true);
                uiManager.notEnoughMoneyWindow.SetActive(false);
            }
            else
            {
                uiManager.confirmPurchaseWindow.SetActive(true);
                uiManager.notEnoughMoneyWindow.SetActive(true);
                uiManager.CloseAllVendorsConfirmWindows();
                uiManager.vendorSelectedHelmet = null;
            }
        }

        public void FinishPurchaseUI()
        {
            if (uiManager.vendorSelectedHelmet != null)
                FinishPurchase(uiManager.vendorSelectedHelmet);
        }

        public void FinishPurchase(HelmetEquipment helmet)
        {
            uiManager.player.playerInventoryManager.headEquipmentInventory.Add(helmet);
            uiManager.player.playerInventoryManager.currentGold = uiManager.player.playerInventoryManager.currentGold - helmet.value;
            if (uiManager.player.playerInventoryManager.currentGold < 0)
                uiManager.player.playerInventoryManager.currentGold = 0;
            uiManager.armorVendorInventoryManager.headEquipmentInventory.Remove(helmet);
        }

        public void ShowItemStats()
        {
            uiManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class HandEquipmentInventorySlot : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        HandEquipment item;

        private void Awake()
        {
            uiManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(HandEquipment newItem)
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
            if (uiManager.handEquipmentSlotSelected)
            {
                if (uiManager.player.playerStatsManager.strengthLevel >= item.strengthLevelRequirement
                    && uiManager.player.playerStatsManager.faithLevel >= item.faithLevelRequirement)
                {
                    if (uiManager.player.playerInventoryManager.currentHandEquipment != null)
                    {
                        uiManager.player.playerInventoryManager.handEquipmentInventory.Add(uiManager.player.playerInventoryManager.currentHandEquipment);
                    }
                    uiManager.player.playerInventoryManager.currentHandEquipment = item;
                    uiManager.player.playerInventoryManager.handEquipmentInventory.Remove(item);
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
                uiManager.vendorSelectedHand = item;
                uiManager.confirmPurchaseWindow.SetActive(true);
                uiManager.CloseAllVendorsConfirmWindows();
                uiManager.confirmHandPurchaseWindow.SetActive(true);
                uiManager.notEnoughMoneyWindow.SetActive(false);
            }
            else
            {
                uiManager.confirmPurchaseWindow.SetActive(true);
                uiManager.notEnoughMoneyWindow.SetActive(true);
                uiManager.CloseAllVendorsConfirmWindows();
                uiManager.vendorSelectedHand = null;
            }
        }

        public void FinishPurchaseUI()
        {
            if (uiManager.vendorSelectedHand != null)
                FinishPurchase(uiManager.vendorSelectedHand);
        }

        public void FinishPurchase(HandEquipment hand)
        {
            uiManager.player.playerInventoryManager.handEquipmentInventory.Add(hand);
            uiManager.player.playerInventoryManager.currentGold = uiManager.player.playerInventoryManager.currentGold - hand.value;
            if (uiManager.player.playerInventoryManager.currentGold < 0)
                uiManager.player.playerInventoryManager.currentGold = 0;
            uiManager.armorVendorInventoryManager.handEquipmentInventory.Remove(hand);
        }

        public void ShowItemStats()
        {
            uiManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }
    }
}

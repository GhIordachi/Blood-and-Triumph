using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class BodyEquipmentInventorySlot : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        BodyEquipment item;

        private void Awake()
        {
            uiManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(BodyEquipment newItem)
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
            if (uiManager.bodyEquipmentSlotSelected)
            {
                if (uiManager.player.playerStatsManager.strengthLevel >= item.strengthLevelRequirement
                    && uiManager.player.playerStatsManager.faithLevel >= item.faithLevelRequirement)
                {
                    if (uiManager.player.playerInventoryManager.currentBodyEquipment != null)
                    {
                        uiManager.player.playerInventoryManager.bodyEquipmentInventory.Add(uiManager.player.playerInventoryManager.currentBodyEquipment);
                    }
                    uiManager.player.playerInventoryManager.currentBodyEquipment = item;
                    uiManager.player.playerInventoryManager.bodyEquipmentInventory.Remove(item);
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
                uiManager.vendorSelectedBody = item;
                uiManager.confirmPurchaseWindow.SetActive(true);
                uiManager.CloseAllVendorsConfirmWindows();
                uiManager.confirmBodyPurchaseWindow.SetActive(true);
                uiManager.notEnoughMoneyWindow.SetActive(false);
            }
            else
            {
                uiManager.confirmPurchaseWindow.SetActive(true);
                uiManager.notEnoughMoneyWindow.SetActive(true);
                uiManager.CloseAllVendorsConfirmWindows();
                uiManager.vendorSelectedBody = null;
            }
        }

        public void FinishPurchaseUI()
        {
            if (uiManager.vendorSelectedBody != null)
                FinishPurchase(uiManager.vendorSelectedBody);
        }

        public void FinishPurchase(BodyEquipment body)
        {
            uiManager.player.playerInventoryManager.bodyEquipmentInventory.Add(body);
            uiManager.player.playerInventoryManager.currentGold = uiManager.player.playerInventoryManager.currentGold - body.value;
            if (uiManager.player.playerInventoryManager.currentGold < 0)
                uiManager.player.playerInventoryManager.currentGold = 0;
            uiManager.armorVendorInventoryManager.bodyEquipmentInventory.Remove(body);
        }

        public void ShowItemStats()
        {
            uiManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class SpellItemInventorySlot : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        SpellItem item;

        private void Awake()
        {
            uiManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(SpellItem newItem)
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
            if (uiManager.spellSlotSelected)
            {
                if (uiManager.player.playerStatsManager.strengthLevel >= item.strengthLevelRequirement
                    && uiManager.player.playerStatsManager.faithLevel >= item.faithLevelRequirement)
                {
                    if (uiManager.player.playerInventoryManager.currentSpell != null)
                        uiManager.player.playerInventoryManager.spellInventory.Add(uiManager.player.playerInventoryManager.currentSpell);
                    uiManager.player.playerInventoryManager.currentSpell = item;
                    uiManager.quickSlotsUI.UpdateCurrentSpellIcon(item);
                    uiManager.player.playerInventoryManager.spellInventory.Remove(item);
                }
            }
            else
            {
                return;
            }

            uiManager.equipmentWindowUI.LoadSpellOnEquipmentScreen(uiManager.player);
            uiManager.ResetAllSelectedSlots();
        }

        public void PurchaseThisItem()
        {
            if (uiManager.player.playerInventoryManager.currentGold >= item.value)
            {
                uiManager.vendorSelectedSpell = item;
                uiManager.confirmPurchaseWindow.SetActive(true);
                uiManager.CloseAllVendorsConfirmWindows();
                uiManager.confirmSpellPurchaseWindow.SetActive(true);
                uiManager.notEnoughMoneyWindow.SetActive(false);
            }
            else
            {
                uiManager.confirmPurchaseWindow.SetActive(true);
                uiManager.notEnoughMoneyWindow.SetActive(true);
                uiManager.CloseAllVendorsConfirmWindows();
                uiManager.vendorSelectedSpell = null;
            }
        }

        public void FinishPurchaseUI()
        {
            if (uiManager.vendorSelectedSpell != null)
                FinishPurchase(uiManager.vendorSelectedSpell);
        }

        public void FinishPurchase(SpellItem spell)
        {
            uiManager.player.playerInventoryManager.spellInventory.Add(spell);
            uiManager.player.playerInventoryManager.currentGold = uiManager.player.playerInventoryManager.currentGold - spell.value;
            if (uiManager.player.playerInventoryManager.currentGold < 0)
                uiManager.player.playerInventoryManager.currentGold = 0;
            uiManager.mageVendorInventoryManager.spellInventory.Remove(spell);
        }

        public void ShowItemStats()
        {
            uiManager.itemStatsWindowUI.UpdateSpellItemStats(item);
        }
    }
}

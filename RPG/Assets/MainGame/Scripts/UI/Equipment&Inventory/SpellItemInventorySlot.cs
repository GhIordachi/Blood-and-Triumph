using System.Collections;
using System.Collections.Generic;
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
                if(uiManager.player.playerInventoryManager.currentSpell != null)
                    uiManager.player.playerInventoryManager.spellInventory.Add(uiManager.player.playerInventoryManager.currentSpell);
                uiManager.player.playerInventoryManager.currentSpell = item;
                uiManager.quickSlotsUI.UpdateCurrentSpellIcon(item);
                uiManager.player.playerInventoryManager.spellInventory.Remove(item);
            }
            else
            {
                return;
            }

            uiManager.equipmentWindowUI.LoadSpellOnEquipmentScreen(uiManager.player);
            uiManager.ResetAllSelectedSlots();
        }

        public void ShowItemStats()
        {
            uiManager.itemStatsWindowUI.UpdateSpellItemStats(item);
        }
    }
}

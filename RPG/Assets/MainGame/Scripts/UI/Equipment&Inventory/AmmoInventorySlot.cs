using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class AmmoInventorySlot : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        RangedAmmoItem item;

        private void Awake()
        {
            uiManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(RangedAmmoItem newItem)
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
            if (uiManager.ammoSlotSelected)
            {
                if (uiManager.player.playerStatsManager.strengthLevel >= item.strengthLevelRequirement
                    && uiManager.player.playerStatsManager.faithLevel >= item.faithLevelRequirement)
                {
                    if (uiManager.player.playerInventoryManager.currentAmmo != null)
                        uiManager.player.playerInventoryManager.ammoInventory.Add(uiManager.player.playerInventoryManager.currentAmmo);
                    uiManager.player.playerInventoryManager.currentAmmo = item;
                    uiManager.player.playerInventoryManager.ammoInventory.Remove(item);
                }
            }
            else
            {
                return;
            }

            uiManager.equipmentWindowUI.LoadAmmoOnEquipmentScreen(uiManager.player);
            uiManager.ResetAllSelectedSlots();
        }

        public void ShowItemStats()
        {
            //uiManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }
    }
}

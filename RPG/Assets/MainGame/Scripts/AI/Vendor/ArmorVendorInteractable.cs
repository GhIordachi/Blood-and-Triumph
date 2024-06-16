using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class ArmorVendorInteractable : Interactable
    {
        public override void Interact(PlayerManager playerManager)
        {
            playerManager.UIManager.armorVendorShopWindow.SetActive(true);
            playerManager.UIManager.equipmentScreenWindow.SetActive(true);
            playerManager.UIManager.itemStatsWindow.SetActive(true);
            playerManager.UIManager.hudWindow.SetActive(false);
            playerManager.UIManager.UpdateVendorUI();
            playerManager.UIManager.UpdateUI();
            playerManager.inputHandler.inventoryFlag = true;
        }
    }
}

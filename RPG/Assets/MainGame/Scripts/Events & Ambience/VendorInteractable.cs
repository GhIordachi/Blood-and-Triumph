using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class VendorInteractable : Interactable
    {
        public override void Interact(PlayerManager playerManager)
        {
            playerManager.UIManager.vendorShopWindow.SetActive(true);
            playerManager.UIManager.weaponInventoryWindow.SetActive(true);
            playerManager.UIManager.itemStatsWindow.SetActive(true);
        }
    }
}

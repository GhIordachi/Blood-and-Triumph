using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class JunkInventorySlot : MonoBehaviour
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

        public void ShowItemStats()
        {
            //uiManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }
    }
}

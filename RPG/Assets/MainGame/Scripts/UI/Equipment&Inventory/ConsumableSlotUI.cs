using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class ConsumableSlotUI : MonoBehaviour
    {
        UIManager uIManager;
        public Image icon;
        ConsumableItem item;

        private void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(ConsumableItem consumable)
        {
            if (consumable != null)
            {
                item = consumable;
                icon.sprite = item.itemIcon;
                icon.enabled = true;
                gameObject.SetActive(true);
            }
            else
            {
                ClearItem();
            }
        }

        public void ClearItem()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
        }

        public void SelectThisSlot()
        {
            uIManager.consumableSlotSelected = true;
            //uIManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }
    }
}

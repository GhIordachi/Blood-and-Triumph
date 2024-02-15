using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class LegEquipmentSlotUI : MonoBehaviour
    {
        UIManager uIManager;
        public Image icon;
        LegEquipment item;

        private void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(LegEquipment legEquipment)
        {
            if (legEquipment != null)
            {
                item = legEquipment;
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
            uIManager.legEquipmentSlotSelected = true;
            uIManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }
    }
}

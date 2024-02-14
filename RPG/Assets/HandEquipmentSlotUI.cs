using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class HandEquipmentSlotUI : MonoBehaviour
    {
        UIManager uIManager;
        public Image icon;
        HandEquipment item;

        private void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(HandEquipment handEquipment)
        {
            if (handEquipment != null)
            {
                item = handEquipment;
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
            uIManager.handEquipmentSlotSelected = true;
        }
    }
}

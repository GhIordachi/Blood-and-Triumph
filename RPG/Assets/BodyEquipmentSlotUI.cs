using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class BodyEquipmentSlotUI : MonoBehaviour
    {
        UIManager uIManager;
        public Image icon;
        BodyEquipment item;

        private void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(BodyEquipment bodyEquipment)
        {
            if (bodyEquipment != null)
            {
                item = bodyEquipment;
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
            uIManager.bodyEquipmentSlotSelected = true;
        }
    }
}

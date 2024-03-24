using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace GI
{
    public class RingItemSlotUI : MonoBehaviour
    {
        UIManager uiManager;
        public Image icon;
        RingItem ringItem;

        public bool ringItem01;
        public bool ringItem02;
        public bool ringItem03;
        public bool ringItem04;      

        private void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(RingItem ring)
        {
            if (ring != null)
            {
                ringItem = ring;
                icon.sprite = ringItem.itemIcon;
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
            ringItem = null;
            icon.sprite = null;
            icon.enabled = false;
        }

        public void SelectThisSlot()
        {
            uiManager.ResetAllSelectedSlots();

            if (ringItem01)
            {
                uiManager.ringItemSlot01Selected = true;
            }
            else if (ringItem02)
            {
                uiManager.ringItemSlot02Selected = true;
            }
            else if (ringItem03)
            {
                uiManager.ringItemSlot03Selected = true;
            }
            else if (ringItem04)
            {
                uiManager.ringItemSlot04Selected = true;
            }

            uiManager.itemStatsWindowUI.UpdateRingItemStats(ringItem);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class AmmoSlotUI : MonoBehaviour
    {
        UIManager uIManager;
        public Image icon;
        RangedAmmoItem item;

        private void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(RangedAmmoItem ammo)
        {
            if (ammo != null)
            {
                item = ammo;
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
            uIManager.ammoSlotSelected = true;
            //uIManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }
    }
}

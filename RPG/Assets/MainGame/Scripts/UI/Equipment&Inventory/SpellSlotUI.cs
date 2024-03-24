using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class SpellSlotUI : MonoBehaviour
    {
        UIManager uIManager;
        public Image icon;
        SpellItem item;

        private void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(SpellItem spell)
        {
            if (spell != null)
            {
                item = spell;
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
            uIManager.spellSlotSelected = true;
            //uIManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }
    }
}

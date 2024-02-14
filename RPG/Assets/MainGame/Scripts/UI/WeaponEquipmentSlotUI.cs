using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI {
    public class WeaponEquipmentSlotUI : MonoBehaviour
    {
        UIManager uIManager;
        public Image icon;
        WeaponItem weapon;
        public WeaponItem replaceWeapon;

        public bool rightHandSlot01;
        public bool rightHandSlot02;
        public bool rightHandSlot03;
        public bool rightHandSlot04;
        public bool leftHandSlot01;
        public bool leftHandSlot02;
        public bool leftHandSlot03;
        public bool leftHandSlot04;

        private void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(WeaponItem newWeapon)
        {
            if(newWeapon != null)
            {
                weapon = newWeapon;
                icon.sprite = weapon.itemIcon;
                icon.enabled = true;
                gameObject.SetActive(true);
            }
            else
            {
                weapon = replaceWeapon;
                icon.sprite = null;
                icon.enabled = false;
                gameObject.SetActive(true);
            }
        }

        public void ClearItem()
        {
            weapon = null;
            icon.sprite = null;
            icon.enabled = false;
        }

        public void SelectThisSlot()
        {
            if (rightHandSlot01)
            {
                uIManager.rightHandSlot01Selected = true;
            }
            else if (rightHandSlot02)
            {
                uIManager.rightHandSlot02Selected = true;
            }
            else if (rightHandSlot03)
            {
                uIManager.rightHandSlot03Selected = true;
            }
            else if (rightHandSlot04)
            {
                uIManager.rightHandSlot04Selected = true;
            }
            else if (leftHandSlot01)
            {
                uIManager.leftHandSlot01Selected= true;
            }
            else if (leftHandSlot02)
            {
                uIManager.leftHandSlot02Selected = true;
            }
            else if (leftHandSlot03)
            {
                uIManager.leftHandSlot03Selected = true;
            }
            else if (leftHandSlot04)
            {
                uIManager.leftHandSlot04Selected = true;
            }
        }
    }
}

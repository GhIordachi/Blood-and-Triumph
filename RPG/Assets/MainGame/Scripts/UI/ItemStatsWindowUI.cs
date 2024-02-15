using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class ItemStatsWindowUI : MonoBehaviour
    {
        public Text itemNameText;
        public Image itemIconImage;

        [Header("Equipment Stats Windows")]
        public GameObject weaponStats;
        public GameObject armorStats;

        [Header("Weapon Stats")]
        public Text physicalDamageText;
        public Text magicalDamageText;
        public Text physicalAbsorptionText;
        public Text magicalAbsorptionText;

        [Header("Armor Stats")]
        public Text armorPhysicalAbsorptionText;
        public Text armorMagicalAbsorptionText;
        public Text armorPoisonResistanceText;

        public void UpdateWeaponItemStats(WeaponItem weapon)
        {
            CloseAllStatWindows();
            if(weapon != null)
            {
                if (weapon.itemName != null)
                {
                    itemNameText.text = weapon.itemName;
                }
                else
                {
                    itemNameText.text = "";
                }

                if (weapon.itemIcon != null)
                {
                    itemIconImage.gameObject.SetActive(true);
                    itemIconImage.sprite = weapon.itemIcon;
                }
                else
                {
                    itemIconImage.gameObject.SetActive(false);
                    itemIconImage.sprite = null;
                }

                physicalDamageText.text = weapon.physicalDamage.ToString();
                physicalAbsorptionText.text = weapon.physicalDamageAbsorption.ToString();
                //Magic damage
                //magic absorption + trebuie adaugat si la arme si equipment

                weaponStats.SetActive(true);
            }
            else
            {
                itemNameText.text = "";
                itemIconImage.gameObject.SetActive(false);
                itemIconImage.sprite = null;
                weaponStats.SetActive(false);
            }
            
        }

        public void UpdateArmorItemStats(EquipmentItem armor)
        {
            CloseAllStatWindows();
            if (armor != null)
            {
                if (armor.itemName != null)
                {
                    itemNameText.text = armor.itemName;
                }
                else
                {
                    itemNameText.text = "";
                }

                if (armor.itemIcon != null)
                {
                    itemIconImage.gameObject.SetActive(true);
                    itemIconImage.sprite = armor.itemIcon;
                }
                else
                {
                    itemIconImage.gameObject.SetActive(false);
                    itemIconImage.sprite = null;
                }

                armorPhysicalAbsorptionText.text = armor.physicalDefense.ToString();
                armorMagicalAbsorptionText.text = armor.magicalDefense.ToString();
                armorPoisonResistanceText.text = armor.poisonResistance.ToString();
                //Magic armor etc.
                //magic absorption + trebuie adaugat si la arme si equipment

                armorStats.SetActive(true);
            }
            else
            {
                itemNameText.text = "";
                itemIconImage.gameObject.SetActive(false);
                itemIconImage.sprite = null;
                armorStats.SetActive(false);
            }

        }

        private void CloseAllStatWindows()
        {
            weaponStats.SetActive(false);
            armorStats.SetActive(false);
        }
    }
}

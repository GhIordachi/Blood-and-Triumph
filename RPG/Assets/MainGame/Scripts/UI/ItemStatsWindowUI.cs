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
        public GameObject ringItemStats;

        [Header("Weapon Stats")]
        public Text physicalDamageText;
        public Text magicalDamageText;
        public Text physicalAbsorptionText;
        public Text magicalAbsorptionText;

        [Header("Armor Stats")]
        public Text armorPhysicalAbsorptionText;
        public Text armorMagicalAbsorptionText;
        public Text armorPoisonResistanceText;

        [Header("Ring Item Text")]
        public Text ringItemDescription;

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
                physicalAbsorptionText.text = weapon.physicalBlockingDamageAbsorption.ToString();
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
                armorMagicalAbsorptionText.text = armor.magicDefense.ToString();
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

        public void UpdateRingItemStats(RingItem ring)
        {
            CloseAllStatWindows();

            if(ring != null)
            {
                if (ring.itemName != null)
                {
                    itemNameText.text = ring.itemName;
                }
                else
                {
                    itemNameText.text = "";
                }

                if (ring.itemIcon != null)
                {
                    itemIconImage.gameObject.SetActive(true);
                    itemIconImage.sprite = ring.itemIcon;
                }
                else
                {
                    itemIconImage.gameObject.SetActive(false);
                    itemIconImage.sprite = null;
                }

                ringItemDescription.text = ring.itemEffectInformation.ToString();
                ringItemStats.SetActive(true);
            }
            else
            {
                itemNameText.text = "";
                itemIconImage.gameObject.SetActive(false);
                itemIconImage.sprite = null;
                ringItemStats.SetActive(false);
            }
        }

        private void CloseAllStatWindows()
        {
            weaponStats.SetActive(false);
            armorStats.SetActive(false);
            ringItemStats.SetActive(false);
        }
    }
}

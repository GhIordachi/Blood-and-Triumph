using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GI
{
    public class WorldItemDataBase : MonoBehaviour
    {
        public static WorldItemDataBase Instance;

        public List<WeaponItem> weaponItems = new List<WeaponItem>();

        public List<RangedAmmoItem> rangedAmmoItems = new List<RangedAmmoItem>();

        public List<EquipmentItem> equipmentItems = new List<EquipmentItem>();

        public List<SpellItem> spellItems = new List<SpellItem>();

        public List<RingItem> ringItems = new List<RingItem>();

        public List<ConsumableItem> consumableItems = new List<ConsumableItem>();

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public WeaponItem GetWeaponItemByID(int weaponID) 
        { 
            return weaponItems.FirstOrDefault(weapon => weapon.itemID == weaponID);
        }

        public RangedAmmoItem GetRangedAmmoItemByID(int rangedAmmoID)
        {
            return rangedAmmoItems.FirstOrDefault(rangedAmmo => rangedAmmo.itemID == rangedAmmoID);
        }

        public EquipmentItem GetEquipmentItemByID(int equipmentID)
        {
            return equipmentItems.FirstOrDefault(equipment => equipment.itemID == equipmentID);
        }

        public SpellItem GetSpellItemByID(int spellID)
        {
            return spellItems.FirstOrDefault(spell => spell.itemID == spellID);
        }

        public RingItem GetRingItemByID(int ringID)
        {
            return ringItems.FirstOrDefault(ring => ring.itemID == ringID);
        }

        public ConsumableItem GetConsumableItemByID(int consumableID)
        {
            return consumableItems.FirstOrDefault(consumable => consumable.itemID == consumableID);
        }
    }
}

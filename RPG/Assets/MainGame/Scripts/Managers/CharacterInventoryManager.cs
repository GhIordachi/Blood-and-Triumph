using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class CharacterInventoryManager : MonoBehaviour
    {
        protected CharacterManager character;

        [Header("Character's Currency")]
        public int currentGold;

        [Header("Current Item Being Used")]
        public Item currentItemBeingUsed;

        [Header("Quick Slots Items")]
        public SpellItem currentSpell;
        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;
        public ConsumableItem currentConsumable;
        public RangedAmmoItem currentAmmo;

        [Header("Current Equipment")]
        public HelmetEquipment currentHelmetEquipment;
        public BodyEquipment currentBodyEquipment;
        public LegEquipment currentLegEquipment;
        public HandEquipment currentHandEquipment;
        public RingItem ringSlot01;
        public RingItem ringSlot02;
        public RingItem ringSlot03;
        public RingItem ringSlot04;

        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[3];
        public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[3];

        public int currentRightWeaponIndex = 0;
        public int currentLeftWeaponIndex = 0;

        private void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        private void Start()
        {
            character.characterWeaponSlotManager.LoadBothWeaponsOnSlots();
        }

        //Call in your save function after loading equipment && form the UI
        public virtual void LoadRingEffects()
        {
            if(ringSlot01 != null)
            {
                ringSlot01.EquipRing(character);
            }
            if (ringSlot02 != null)
            {
                ringSlot02.EquipRing(character);
            }
            if (ringSlot03 != null)
            {
                ringSlot03.EquipRing(character);
            }
            if (ringSlot04 != null)
            {
                ringSlot04.EquipRing(character);
            }
        }
    }
}

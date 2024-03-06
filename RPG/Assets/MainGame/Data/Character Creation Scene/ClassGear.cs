using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [System.Serializable]
    public class ClassGear
    {
        [Header("Class Name")]
        public string className;

        [Header("Weapons")]
        public WeaponItem primaryWeapon;
        public WeaponItem offHandWeapon;
        public RangedAmmoItem ammoItem;
        //public WeaponItem secondaryWeapon;

        [Header("Armor")]
        public HelmetEquipment headEquipment;
        public BodyEquipment bodyEquipment;
        public LegEquipment legEquipment;
        public HandEquipment handEquipment;

        //public SpellItem startingSpell;
    }
}

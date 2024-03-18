using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class EquipmentItem : Item
    {
        [Header("Defense Bonus")]
        public float physicalDefense;
        public float fireDefense;
        public float magicDefense;

        [Header("Weight")]
        public float weight;

        [Header("Resistances")]
        public float poisonResistance;
    }
}

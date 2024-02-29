using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [CreateAssetMenu(menuName = "Item/Melee Weapon Item")]
    public class MeleeWeaponItem : WeaponItem
    {
        public bool canBeBuffed = true;
    }
}

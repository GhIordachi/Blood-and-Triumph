using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [CreateAssetMenu(menuName = "Item Actions/Parry Action")]
    public class ParryAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting)
                return;

            WeaponItem parryingWeapon = character.characterInventoryManager.currentItemBeingUsed as WeaponItem;

            //Check if parrying weapon is a fast parry weapon or a medium parry weapon
            if(parryingWeapon.weaponType == WeaponType.SmallShield)
            {
                //Fast parry Anim
                character.characterAnimatorManager.PlayTargetAnimation("Parry_01", true);
            }
            else if (parryingWeapon.weaponType == WeaponType.Shield)
            {
                //Normal parry Anim
                character.characterAnimatorManager.PlayTargetAnimation("Parry_01", true);
            }
        }
    }
}

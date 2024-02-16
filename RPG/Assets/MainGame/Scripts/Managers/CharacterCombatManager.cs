using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class CharacterCombatManager : MonoBehaviour
    {
        [Header("Attack Type")]
        public AttackType currentAttackType;

        public virtual void DrainStaminaBasedOnAttack()
        {
            
        }
    }
}

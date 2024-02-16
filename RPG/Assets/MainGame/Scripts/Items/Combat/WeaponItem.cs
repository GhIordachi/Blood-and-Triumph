using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI 
{
    [CreateAssetMenu(menuName = "Item/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Animator Replacer")]
        public AnimatorOverrideController weaponController;
        public string offHandIdleAnimation = "Left_Arm_Idle_01";

        [Header("Weapon Type")]
        public WeaponType weaponType;

        [Header("Damage")]
        public int physicalDamage;
        public int fireDamage;

        [Header("Damage Modifiers")]
        public float lightAttackDamageModifier;
        public float heavyAttackDamageModifier;
        public int criticalDamageMultiplier = 4;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        [Header("Absorption")]
        public float physicalDamageAbsorption;

        [Header("Stamina Costs")]
        public int baseStaminaCost;
        public float lightAttackStaminaMultiplier;
        public float heavyAttackStaminaMultiplier;

        [Header("Item Actions")]
        public ItemAction oh_tap_RB_Action;
        public ItemAction oh_hold_RB_Action;
        public ItemAction oh_tap_LB_Action;
        public ItemAction oh_hold_LB_Action;
        public ItemAction oh_tap_RT_Action;
        public ItemAction oh_hold_RT_Action;
        public ItemAction oh_tap_LT_Action;
        public ItemAction oh_hold_LT_Action;

        [Header("Two Handed Item Actions")]
        public ItemAction th_tap_RB_Action;
        public ItemAction th_hold_RB_Action;
        public ItemAction th_tap_LB_Action;
        public ItemAction th_hold_LB_Action;
        public ItemAction th_tap_RT_Action;
        public ItemAction th_hold_RT_Action;
        public ItemAction th_tap_LT_Action;
        public ItemAction th_hold_LT_Action;

        [Header("Sound FX")]
        public AudioClip[] weaponWhooshes;
    }
}

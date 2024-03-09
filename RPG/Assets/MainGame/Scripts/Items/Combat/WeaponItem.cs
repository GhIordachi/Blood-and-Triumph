using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI 
{
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Animator Replacer")]
        public AnimatorOverrideController weaponController;
        //public string offHandIdleAnimation = " ";

        [Header("Weapon Type")]
        public WeaponType weaponType;

        [Header("Damage")]
        public int physicalDamage;
        public int fireDamage;

        [Header("Damage Modifiers")]
        public float lightAttackDamageModifier = 1;
        public float heavyAttackDamageModifier = 2;
        public int criticalDamageMultiplier = 4;
        public float guardBreakModifier = 1;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        [Header("Absorption")]
        public float physicalBlockingDamageAbsorption;
        public float fireBlockingDamageAbsorption;

        [Header("Stability")]
        public int stability = 67;

        [Header("Stamina Costs")]
        public int baseStaminaCost;
        public float lightAttackStaminaMultiplier;
        public float heavyAttackStaminaMultiplier;

        [Header("Item Actions")]
        public ItemAction oh_tap_Left_Click;
        public ItemAction oh_hold_Left_Click;
        public ItemAction oh_tap_Right_Click;
        public ItemAction oh_hold_Right_Click;
        public ItemAction oh_tap_R_Action;
        public ItemAction oh_hold_R_Action;
        public ItemAction oh_tap_Q_Action;
        public ItemAction oh_hold_Q_Action;

        [Header("Two Handed Item Actions")]
        public ItemAction th_tap_Left_Click;
        public ItemAction th_hold_Left_Click;
        public ItemAction th_tap_Right_Click;
        public ItemAction th_hold_Right_Click;
        public ItemAction th_tap_R_Action;
        public ItemAction th_hold_R_Action;
        public ItemAction th_tap_Q_Action;
        public ItemAction th_hold_Q_Action;

        [Header("Sound FX")]
        public AudioClip[] weaponWhooshes;
        public AudioClip[] blockingNoises;
    }
}

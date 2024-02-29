using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public enum WeaponType
    {
        PyromancyCaster,
        FaithCaster,
        SpellCaster,
        Unarmed,
        StraightSword,
        Shield,
        Bow,
        SmallShield
    }

    public enum AmmoType
    {
        Arrow,
        Bolt
    }

    public enum AttackType
    {
        light,
        heavy
    }

    public enum AICombatStyle
    {
        swordAndShield,
        archer
    }

    public enum AIAttackActionType
    {
        meleeAttackAction,
        magicAttackAction,
        rangedAttackAction
    }

    public enum DamageType
    {
        Physical,
        Fire
    }

    public enum BuffClass
    {
        Physical,
        Fire
    }

    public enum EffectParticleType
    {
        poison
    }

    public enum EncumbranceLevel
    {
        Light,  
        Medium,
        Heavy,
        Overloaded
    }

    public class Enums : MonoBehaviour
    {
        
    }
}

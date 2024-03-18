using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        protected CharacterManager character;

        [Header("Unarmed Weapon")]
        public WeaponItem unarmedWeapon;

        [Header("Weapon Slots")]
        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;
        public WeaponHolderSlot backSlot;

        [Header("Damage Colliders")]
        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        [Header("Hand IK Targets")]
        public RightHandIKTarget rightHandIKTarget;
        public LeftHandIKTarget leftHandIKTarget;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            LoadWeaponHolderSlots();
        }

        protected virtual void LoadWeaponHolderSlots()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.isBackSlot)
                {
                    backSlot = weaponSlot;
                }
            }
        }

        public void LoadBothWeaponsOnSlots()
        {
            LoadWeaponOnSlot(character.characterInventoryManager.rightWeapon, false);
            LoadWeaponOnSlot(character.characterInventoryManager.leftWeapon, true);
        }

        public virtual void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (weaponItem != null)
            {
                if (isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponItem(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    //character.characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    if (character.isTwoHandingWeapon)
                    {
                        backSlot.LoadWeaponItem(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        character.characterAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();
                    }
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponItem(weaponItem);
                    LoadRightWeaponDamageCollider();
                    LoadTwoHandIKTargets(character.isTwoHandingWeapon);
                    character.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                weaponItem = unarmedWeapon;

                if (isLeft)
                {
                    character.characterInventoryManager.leftWeapon = weaponItem;
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponItem(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    //character.characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    character.characterInventoryManager.rightWeapon = weaponItem;
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponItem(weaponItem);
                    LoadRightWeaponDamageCollider();
                    character.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }

        }

        protected virtual void LoadLeftWeaponDamageCollider()
        {
            if (leftHandSlot.curentWeaponModel.GetComponentInChildren<DamageCollider>() != null)
            {
                leftHandDamageCollider = leftHandSlot.curentWeaponModel.GetComponentInChildren<DamageCollider>();

                leftHandDamageCollider.physicalDamage = character.characterInventoryManager.leftWeapon.physicalDamage;
                leftHandDamageCollider.fireDamage = character.characterInventoryManager.leftWeapon.fireDamage;
                leftHandDamageCollider.magicDamage = character.characterInventoryManager.leftWeapon.magicDamage;

                leftHandDamageCollider.characterManager = character;
                leftHandDamageCollider.teamIDNumber = character.characterStatsManager.teamIDNumber;

                leftHandDamageCollider.poiseDamage = character.characterInventoryManager.leftWeapon.poiseBreak;
                character.characterEffectsManager.leftWeaponManager = leftHandSlot.curentWeaponModel.GetComponentInChildren<WeaponManager>();
            }
        }

        protected virtual void LoadRightWeaponDamageCollider()
        {
            if (rightHandSlot.curentWeaponModel.GetComponentInChildren<DamageCollider>() != null)
            {
                rightHandDamageCollider = rightHandSlot.curentWeaponModel.GetComponentInChildren<DamageCollider>();

                rightHandDamageCollider.physicalDamage = character.characterInventoryManager.rightWeapon.physicalDamage;
                rightHandDamageCollider.fireDamage = character.characterInventoryManager.rightWeapon.fireDamage;
                rightHandDamageCollider.magicDamage = character.characterInventoryManager.rightWeapon.magicDamage;

                rightHandDamageCollider.characterManager = character;
                rightHandDamageCollider.teamIDNumber = character.characterStatsManager.teamIDNumber;

                rightHandDamageCollider.poiseDamage = character.characterInventoryManager.rightWeapon.poiseBreak;
                character.characterEffectsManager.rightWeaponManager = rightHandSlot.curentWeaponModel.GetComponentInChildren<WeaponManager>();
            }
        }

        public virtual void LoadTwoHandIKTargets(bool isTwoHandingWeapon)
        {
            leftHandIKTarget = rightHandSlot.curentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();
            rightHandIKTarget = rightHandSlot.curentWeaponModel.GetComponentInChildren<RightHandIKTarget>();

            character.characterAnimatorManager.SetHandIKForWeapon(rightHandIKTarget, leftHandIKTarget, isTwoHandingWeapon);
        }

        public virtual void OpenDamageCollider()
        {
            character.characterSoundFXManager.PlayRandomWeaponWhoosh();

            if (character.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
            else if (leftHandDamageCollider != null)
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
        }

        public virtual void CloseDamageCollider()
        {
            if (leftHandDamageCollider != null)
            {
                leftHandDamageCollider.DisableDamageCollider();
            }
            if (rightHandDamageCollider != null)
            {
                rightHandDamageCollider.DisableDamageCollider();
            }
        }

        public virtual void GrantWeaponAttackingPoiseBonus()
        {
            WeaponItem currentWeaponBeingUsed = character.characterInventoryManager.currentItemBeingUsed as WeaponItem;
            character.characterStatsManager.totalPoiseDefence = character.characterStatsManager.totalPoiseDefence + currentWeaponBeingUsed.offensivePoiseBonus;
        }

        public virtual void ResetWeaponAttackingPoiseBonus()
        {
            character.characterStatsManager.totalPoiseDefence = character.characterStatsManager.armorPoiseBonus;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        protected CharacterManager characterManager;
        protected CharacterStatsManager characterStatsManager;
        protected CharacterEffectsManager characterEffectsManager;
        protected CharacterInventoryManager characterInventoryManager;
        protected CharacterAnimatorManager characterAnimatorManager;

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
            characterManager = GetComponent<CharacterManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterInventoryManager = GetComponent<CharacterInventoryManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
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
            LoadWeaponOnSlot(characterInventoryManager.rightWeapon, false);
            LoadWeaponOnSlot(characterInventoryManager.leftWeapon, true);
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
                    characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    if (characterManager.isTwoHandingWeapon)
                    {
                        backSlot.LoadWeaponItem(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        characterAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();
                    }
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponItem(weaponItem);
                    LoadRightWeaponDamageCollider();
                    LoadTwoHandIKTargets(characterManager.isTwoHandingWeapon);
                    characterAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                weaponItem = unarmedWeapon;

                if (isLeft)
                {
                    characterInventoryManager.leftWeapon = weaponItem;
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponItem(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    characterInventoryManager.rightWeapon = weaponItem;
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponItem(weaponItem);
                    LoadRightWeaponDamageCollider();
                    characterAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }

        }

        protected virtual void LoadLeftWeaponDamageCollider()
        {
            if (leftHandSlot.curentWeaponModel.GetComponentInChildren<DamageCollider>() != null)
            {
                leftHandDamageCollider = leftHandSlot.curentWeaponModel.GetComponentInChildren<DamageCollider>();

                leftHandDamageCollider.physicalDamage = characterInventoryManager.leftWeapon.physicalDamage;
                leftHandDamageCollider.fireDamage = characterInventoryManager.leftWeapon.fireDamage;

                leftHandDamageCollider.characterManager = characterManager;
                leftHandDamageCollider.teamIDNumber = characterStatsManager.teamIDNumber;

                leftHandDamageCollider.poiseBreak = characterInventoryManager.leftWeapon.poiseBreak;
                characterEffectsManager.leftWeaponFX = leftHandSlot.curentWeaponModel.GetComponentInChildren<WeaponFX>();
            }
        }

        protected virtual void LoadRightWeaponDamageCollider()
        {
            if (rightHandSlot.curentWeaponModel.GetComponentInChildren<DamageCollider>() != null)
            {
                rightHandDamageCollider = rightHandSlot.curentWeaponModel.GetComponentInChildren<DamageCollider>();

                rightHandDamageCollider.physicalDamage = characterInventoryManager.rightWeapon.physicalDamage;
                rightHandDamageCollider.fireDamage = characterInventoryManager.rightWeapon.fireDamage;

                rightHandDamageCollider.characterManager = characterManager;
                rightHandDamageCollider.teamIDNumber = characterStatsManager.teamIDNumber;

                rightHandDamageCollider.poiseBreak = characterInventoryManager.rightWeapon.poiseBreak;
                characterEffectsManager.rightWeaponFX = rightHandSlot.curentWeaponModel.GetComponentInChildren<WeaponFX>();
            }
        }

        public virtual void LoadTwoHandIKTargets(bool isTwoHandingWeapon)
        {
            leftHandIKTarget = rightHandSlot.curentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();
            rightHandIKTarget = rightHandSlot.curentWeaponModel.GetComponentInChildren<RightHandIKTarget>();

            characterAnimatorManager.SetHandIKForWeapon(rightHandIKTarget, leftHandIKTarget, isTwoHandingWeapon);
        }

        public virtual void OpenDamageCollider()
        {
            if (characterManager.isUsingRightHand)
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
            WeaponItem currentWeaponBeingUsed = characterInventoryManager.currentItemBeingUsed as WeaponItem;
            characterStatsManager.totalPoiseDefence = characterStatsManager.totalPoiseDefence + currentWeaponBeingUsed.offensivePoiseBonus;
        }

        public virtual void ResetWeaponAttackingPoiseBonus()
        {
            characterStatsManager.totalPoiseDefence = characterStatsManager.armorPoiseBonus;
        }
    }
}

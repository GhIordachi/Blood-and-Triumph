using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class WeaponSlotManager : MonoBehaviour
    {
        PlayerManager playerManager;
        PlayerInventory playerInventory;

        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;
        WeaponHolderSlot backSlot;

        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        public WeaponItem attackingWeapon;

        Animator animator;

        QuickSlotsUI quickSlotsUI;

        PlayerStats playerStats;

        InputHandler inputHandler;

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            animator = GetComponent<Animator>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            playerStats = GetComponentInParent<PlayerStats>();
            inputHandler = GetComponentInParent<InputHandler>();

            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach(WeaponHolderSlot weaponSlot  in weaponHolderSlots)
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
            LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            LoadWeaponOnSlot(playerInventory.leftWeapon, true);
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if(isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponItem(weaponItem);
                LoadLeftWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                #region Handle Left Weapon Idle Animations
                if (weaponItem != null)
                {
                    animator.CrossFade(weaponItem.left_hand_idle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Left Arm Empty", 0.2f);
                }
                #endregion
            }
            else
            {
                if (inputHandler.twoHandFlag)
                {
                    backSlot.LoadWeaponItem(leftHandSlot.currentWeapon);
                    leftHandSlot.UnloadWeaponAndDestroy();
                    animator.CrossFade(weaponItem.th_idle, 0.2f);
                }
                else
                {
                    #region Handle Right Weapon Idle Animations

                    animator.CrossFade("Both Arms Empty", 0.2f);

                    backSlot.UnloadWeaponAndDestroy();

                    if (weaponItem != null)
                    {
                        animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
                    }
                    else
                    {
                        animator.CrossFade("Right Arm Empty", 0.2f);
                    }
                    #endregion
                }
                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponItem(weaponItem);
                LoadRightWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
            }
        }

        #region Handle Weapon's Damage Collider

        private void LoadLeftWeaponDamageCollider()
        {
            if (leftHandSlot.curentWeaponModel.GetComponentInChildren<DamageCollider>() != null)
            {
                leftHandDamageCollider = leftHandSlot.curentWeaponModel.GetComponentInChildren<DamageCollider>();
                leftHandDamageCollider.currentWeaponDamage = playerInventory.leftWeapon.baseDamage;
                leftHandDamageCollider.poiseBreak = playerInventory.leftWeapon.poiseBreak;
            }
        }

        private void LoadRightWeaponDamageCollider()
        {
            if (rightHandSlot.curentWeaponModel.GetComponentInChildren<DamageCollider>() != null)
            {
                rightHandDamageCollider = rightHandSlot.curentWeaponModel.GetComponentInChildren<DamageCollider>();
                rightHandDamageCollider.currentWeaponDamage = playerInventory.rightWeapon.baseDamage;
                rightHandDamageCollider.poiseBreak = playerInventory.rightWeapon.poiseBreak;
            }
        }

        public void OpenDamageCollider()
        {
            if (playerManager.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
            else if (leftHandDamageCollider != null)
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
        }

        public void CloseDamageCollider()
        {
            if(leftHandDamageCollider != null)
            {
                leftHandDamageCollider.DisableDamageCollider();
            }
            if(rightHandDamageCollider != null)
            {
                rightHandDamageCollider.DisableDamageCollider();
            }
        }

        #endregion

        #region Handle Weapon's Stamina Drainage
        public void DrainStaminaLightAttack()
        {
            Debug.Log("Light Attack Multiplier: " + attackingWeapon.lightAttackMultiplier);
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            Debug.Log("Heavy Attack Multiplier: " + attackingWeapon.heavyAttackMultiplier);
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }

        #endregion

        #region Handle Weapon's Poise Bonus

        public void GrantWeaponAttackingPoiseBonus()
        {
            playerStats.totalPoiseDefence = playerStats.totalPoiseDefence + attackingWeapon.offensivePoiseBonus;
        }

        public void ResetWeaponAttackingPoiseBonus()
        {
            playerStats.totalPoiseDefence = playerStats.armorPoiseBonus;
        }

        #endregion
    }
}

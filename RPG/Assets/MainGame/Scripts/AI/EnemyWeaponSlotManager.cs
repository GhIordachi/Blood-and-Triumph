using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class EnemyWeaponSlotManager : MonoBehaviour
    {
        public WeaponItem rightHandWeapon;
        public WeaponItem leftHandWeapon;

        WeaponHolderSlot rightHandSlot;
        WeaponHolderSlot leftHandSlot;

        DamageCollider leftHandDamageCollider;
        DamageCollider rightHandDamageCollider;

        EnemyStats enemyStats;

        private void Awake()
        {
            enemyStats = GetComponentInParent<EnemyStats>();
            LoadWeaponHolderSlots();
        }

        private void Start()
        {
            LoadWeaponsOnBothHands();
        }

        private void LoadWeaponHolderSlots()
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
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weapon;
                leftHandSlot.LoadWeaponItem(weapon);
                //Load weapon's damage collider
                LoadWeaponsDamageCollider(true);
            }
            else
            {
                rightHandSlot.currentWeapon = weapon;
                rightHandSlot.LoadWeaponItem(weapon);
                //Load weapon's damage collider
                LoadWeaponsDamageCollider(false);
            }
        }

        public void LoadWeaponsOnBothHands()
        {
            if (rightHandWeapon != null)
            {
                LoadWeaponOnSlot(rightHandWeapon, false);
            }
            if (leftHandWeapon != null)
            {
                LoadWeaponOnSlot(leftHandWeapon, true);
            }
        }

        public void LoadWeaponsDamageCollider(bool isLeft)
        {
            if(isLeft)
            {
                leftHandDamageCollider = leftHandSlot.curentWeaponModel.GetComponentInChildren<DamageCollider>();
                leftHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
            }
            else
            {
                rightHandDamageCollider = rightHandSlot.curentWeaponModel.GetComponentInChildren<DamageCollider>();
                rightHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
            }
        }

        public void OpenDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void CloseDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }

        public void DrainStaminaLightAttack()
        {

        }

        public void DrainStaminaHeavyAttack()
        {

        }

        //public void EnableCombo()
        //{
        //    //anim.SetBool("canDoCombo", true);
        //}

        //public void DisableCombo()
        //{
        //    //anim.SetBool("canDoCombo", false);
        //}

        #region Handle Weapon's Poise Bonus

        public void GrantWeaponAttackingPoiseBonus()
        {
            enemyStats.totalPoiseDefence = enemyStats.totalPoiseDefence + enemyStats.offensivePoiseBonus;
        }

        public void ResetWeaponAttackingPoiseBonus()
        {
            enemyStats.totalPoiseDefence = enemyStats.armorPoiseBonus;
        }

        #endregion
    }
}

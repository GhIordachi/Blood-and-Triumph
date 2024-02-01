using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
    {
        QuickSlotsUI quickSlotsUI;
        InputHandler inputHandler;
        PlayerManager playerManager;
        PlayerInventoryManager playerInventoryManager;
        PlayerStatsManager playerStatsManager;
        PlayerEffectsManager playerEffectsManager;
        PlayerAnimatorManager playerAnimatorManager;
        CameraHandler cameraHandler;

        protected override void Awake()
        {
            base.Awake();
            cameraHandler = FindObjectOfType<CameraHandler>();
            inputHandler = GetComponent<InputHandler>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerManager = GetComponent<PlayerManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
        }

        public override void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if(weaponItem != null)
            {
                if (isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponItem(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                    playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    if (inputHandler.twoHandFlag)
                    {
                        backSlot.LoadWeaponItem(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        playerAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();
                    }
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponItem(weaponItem);
                    LoadRightWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                    playerAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                weaponItem = unarmedWeapon;

                if (isLeft)
                {
                    playerInventoryManager.leftWeapon = weaponItem;
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponItem(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                    playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    playerInventoryManager.rightWeapon = weaponItem;
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponItem(weaponItem);
                    LoadRightWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                    playerAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            
        }

        public void SuccessfullyThrowFireBomb()
        {
            Destroy(playerEffectsManager.instantiatedFXModel);
            BombConsumeableItem fireBombItem = playerInventoryManager.currentConsumable as BombConsumeableItem;

            GameObject activeModelBomb = Instantiate(fireBombItem.liveBombModel, rightHandSlot.transform.position, cameraHandler.cameraPivotTransform.rotation);
            activeModelBomb.transform.rotation = 
                Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0);
            BombDamageCollider damageCollider = activeModelBomb.GetComponentInChildren<BombDamageCollider>();

            damageCollider.explosionDamage = fireBombItem.baseDamage;
            damageCollider.explosionSplashDamage = fireBombItem.explosiveDamage;
            damageCollider.bombRigidBody.AddForce(activeModelBomb.transform.forward * fireBombItem.forwardVelocity);
            damageCollider.bombRigidBody.AddForce(activeModelBomb.transform.up * fireBombItem.upwardVelocity);
            damageCollider.teamIDNumber = playerStatsManager.teamIDNumber;
            LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);            
        }

        public void DrainStaminaLightAttack()
        {
            Debug.Log("Light Attack Multiplier: " + attackingWeapon.lightAttackMultiplier);
            playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            Debug.Log("Heavy Attack Multiplier: " + attackingWeapon.heavyAttackMultiplier);
            playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }
    }
}

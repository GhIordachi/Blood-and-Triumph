using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
    {
        PlayerManager player;        

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
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
                    player.UIManager.quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                    player.playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    if (player.inputHandler.twoHandFlag)
                    {
                        backSlot.LoadWeaponItem(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        player.playerAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();
                    }
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponItem(weaponItem);
                    LoadRightWeaponDamageCollider();
                    player.UIManager.quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                    player.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                weaponItem = unarmedWeapon;

                if (isLeft)
                {
                    player.playerInventoryManager.leftWeapon = weaponItem;
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponItem(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    player.UIManager.quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                    player.playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    player.playerInventoryManager.rightWeapon = weaponItem;
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponItem(weaponItem);
                    LoadRightWeaponDamageCollider();
                    player.UIManager.quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                    player.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            
        }

        public void SuccessfullyThrowFireBomb()
        {
            Destroy(player.playerEffectsManager.instantiatedFXModel);
            BombConsumeableItem fireBombItem = player.playerInventoryManager.currentConsumable as BombConsumeableItem;

            GameObject activeModelBomb = Instantiate(fireBombItem.liveBombModel, rightHandSlot.transform.position, player.cameraHandler.cameraPivotTransform.rotation);
            activeModelBomb.transform.rotation = 
                Quaternion.Euler(player.cameraHandler.cameraPivotTransform.eulerAngles.x, player.lockOnTransform.eulerAngles.y, 0);
            BombDamageCollider damageCollider = activeModelBomb.GetComponentInChildren<BombDamageCollider>();

            damageCollider.explosionDamage = fireBombItem.baseDamage;
            damageCollider.explosionSplashDamage = fireBombItem.explosiveDamage;
            damageCollider.bombRigidBody.AddForce(activeModelBomb.transform.forward * fireBombItem.forwardVelocity);
            damageCollider.bombRigidBody.AddForce(activeModelBomb.transform.up * fireBombItem.upwardVelocity);
            damageCollider.teamIDNumber = player.playerStatsManager.teamIDNumber;
            LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);            
        }
    }
}

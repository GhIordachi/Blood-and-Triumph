using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI {
    public class PlayerCombatManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerManager playerManager;
        CameraHandler cameraHandler;
        PlayerAnimatorManager playerAnimatorManager;
        PlayerEquipmentManager playerEquipmentManager;
        PlayerStatsManager playerStatsManager;
        PlayerInventoryManager playerInventoryManager;
        PlayerWeaponSlotManager playerWeaponSlotManager;
        PlayerEffectsManager playerEffectsManager;

        [Header("Attack Animations")]
        string oh_light_attack_01 = "OH_Light_Attack_01";
        string oh_light_attack_02 = "OH_Light_Attack_02";
        string oh_heavy_attack_01 = "OH_Heavy_Attack_01";
        string oh_heavy_attack_02 = "OH_Heavy_Attack_02";

        string th_light_attack_01 = "TH_Light_Attack_01";
        string th_light_attack_02 = "TH_Light_Attack_02";
        string th_heavy_attack_01 = "TH_Heavy_Attack_01";
        string th_heavy_attack_02 = "TH_Heavy_Attack_02";

        string weapon_art = "Weapon_Art";

        public string lastAttack;

        LayerMask backStabLayer = 1 << 15;
        LayerMask riposteLayer = 1 << 16;

        private void Awake()
        {
            cameraHandler = FindAnyObjectByType<CameraHandler>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerManager = GetComponent<PlayerManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            inputHandler = GetComponent<InputHandler>();
        }

        public void HandleRBAction()
        {
            playerAnimatorManager.EraseHandIKForWeapon();

            if (playerInventoryManager.rightWeapon.weaponType == WeaponType.StraightSword ||
                playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
            {
                PerformRBMeleeAction();
            }
            else if (playerInventoryManager.rightWeapon.weaponType == WeaponType.SpellCaster ||
                playerInventoryManager.rightWeapon.weaponType == WeaponType.FaithCaster ||
                playerInventoryManager.rightWeapon.weaponType == WeaponType.PyromancyCaster)
            {
                PerformRBMagicAction(playerInventoryManager.rightWeapon);
            }
        }

        public void HandleRTAction()
        {
            if (playerInventoryManager.rightWeapon.weaponType == WeaponType.StraightSword)
            {
                PerformRTMeleeAction();
            }
        }

        public void HandleBlockAction()
        {
            PerformBlockingAction();
        }

        public void HandleParryAction()
        {
            if (playerInventoryManager.leftWeapon.weaponType == WeaponType.Shield ||
                playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
            {
                //Perform shield weapon art
                PerformParryWeaponArt(inputHandler.twoHandFlag);
            }
            else if (playerInventoryManager.leftWeapon.weaponType == WeaponType.StraightSword)
            {
                //do a light attack
            }
        }



        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
                return;

            if (inputHandler.comboFlag)
            {
                playerAnimatorManager.animator.SetBool("canDoCombo", false);

                if (lastAttack == oh_light_attack_01)
                {
                    playerAnimatorManager.PlayTargetAnimation(oh_light_attack_02, true);
                }
                else if(lastAttack == th_light_attack_01)
                {
                    playerAnimatorManager.PlayTargetAnimation(th_light_attack_02, true);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
                return;

            playerWeaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                playerAnimatorManager.PlayTargetAnimation(th_light_attack_01, true);
                lastAttack =th_light_attack_01;
            }
            else
            {                
                playerAnimatorManager.PlayTargetAnimation(oh_light_attack_01, true);
                lastAttack = oh_light_attack_01;
            }
            
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
                return;

            playerAnimatorManager.animator.SetBool("isUsingRightHand", true);

            playerWeaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                playerAnimatorManager.PlayTargetAnimation(th_heavy_attack_01, true);
                lastAttack = th_heavy_attack_01;
            }
            else
            {                
                playerAnimatorManager.PlayTargetAnimation(oh_heavy_attack_01, true);
                lastAttack = oh_heavy_attack_01;
            }
            
        }



        private void PerformRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventoryManager.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;
                if (playerManager.canDoCombo)
                    return;

                playerAnimatorManager.animator.SetBool("isUsingRightHand", true);
                HandleLightAttack(playerInventoryManager.rightWeapon);
            }

            playerEffectsManager.PlayWeaponFX(false);
        }

        private void PerformRTMeleeAction()
        {
            if (playerManager.isInteracting)
                return;
            if (playerManager.canDoCombo)
                return;

            playerAnimatorManager.animator.SetBool("isUsingRightHand", true);
            HandleHeavyAttack(playerInventoryManager.rightWeapon);
        }

        private void PerformBlockingAction()
        {
            if (playerManager.isInteracting)
                return;

            if (playerManager.isBlocking)
                return;

            playerAnimatorManager.PlayTargetAnimation("Block", false, true);
            playerEquipmentManager.OpenBlockingCollider();
            playerManager.isBlocking = true;
        }



        private void PerformRBMagicAction(WeaponItem weapon)
        {
            if (playerManager.isInteracting)
                return;

            if (weapon.weaponType == WeaponType.FaithCaster)
            {
                if(playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isFaithSpell)
                {
                    if(playerStatsManager.currentFocusPoints >= playerInventoryManager.currentSpell.focusPointCost)
                    {
                        playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);
                    }
                    else
                    {
                        playerAnimatorManager.PlayTargetAnimation("No", true);
                    }
                }
            }
            else if(weapon.weaponType == WeaponType.PyromancyCaster)
            {
                if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isPyroSpell)
                {
                    if (playerStatsManager.currentFocusPoints >= playerInventoryManager.currentSpell.focusPointCost)
                    {
                        playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);
                    }
                    else
                    {
                        playerAnimatorManager.PlayTargetAnimation("No", true);
                    }
                }
            }
        }

        private void PerformParryWeaponArt(bool isTwoHanding)
        {
            if (playerManager.isInteracting)
                return;

            if (isTwoHanding)
            {
                //perform weapon art for right weapon
            }
            else
            {
                //Perform weapon art for left weapon
                playerAnimatorManager.PlayTargetAnimation(weapon_art, true);
            }
        }

        private void SuccessfullyCastSpell()
        {
            playerInventoryManager.currentSpell.SuccessfullyCastSpell(playerAnimatorManager, playerStatsManager, cameraHandler, playerWeaponSlotManager);
            playerAnimatorManager.animator.SetBool("isFiringSpell", true);
        }


        public void AttemptBackStabOrRiposte()
        {
            if (playerStatsManager.currentStamina <= 0)
                return;

            RaycastHit hit;

            if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

                if(enemyCharacterManager != null)
                {
                    //Check for Team ID so you can't back stab allies or yourself
                    playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamageStandPoint.position;

                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    playerAnimatorManager.PlayTargetAnimation("Back Stab", true);
                    enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
                }
            }
            else if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
            {
                //Check for Team ID
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

                if(enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
                {
                    playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamageStandPoint.position;

                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    playerAnimatorManager.PlayTargetAnimation("Riposte", true);
                    enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true);
                }
            }
        }

    }
}

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
        string oh_running_attack_01 = "OH_Running_Attack_01";
        string oh_jumping_attack_01 = "OH_Jumping_Attack_01";

        string th_light_attack_01 = "TH_Light_Attack_01";
        string th_light_attack_02 = "TH_Light_Attack_02";
        string th_heavy_attack_01 = "TH_Heavy_Attack_01";
        string th_heavy_attack_02 = "TH_Heavy_Attack_02";
        string th_running_attack_01 = "TH_Running_Attack_01";
        string th_jumping_attack_01 = "TH_Jumping_Attack_01";

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

        public void HandleHoldRBAction()
        {
            if (playerManager.isTwoHandingWeapon)
            {
                PerformRBRangedAttack();
            }
            else
            {
                //Do a melee Attack (Bow Bash)
            }
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
                PerformMagicAction(playerInventoryManager.rightWeapon, false);
            }
        }

        public void HandleRTAction()
        {
            playerAnimatorManager.EraseHandIKForWeapon();

            if (playerInventoryManager.rightWeapon.weaponType == WeaponType.StraightSword ||
                playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
            {
                PerformRTMeleeAction();
            }
            else if (playerInventoryManager.rightWeapon.weaponType == WeaponType.SpellCaster ||
                playerInventoryManager.rightWeapon.weaponType == WeaponType.FaithCaster ||
                playerInventoryManager.rightWeapon.weaponType == WeaponType.PyromancyCaster)
            {
                PerformMagicAction(playerInventoryManager.rightWeapon, false);
            }
        }

        public void HandleBlockAction()
        {
            if(playerManager.isTwoHandingWeapon)
            {
                if(playerInventoryManager.rightWeapon.weaponType == WeaponType.Bow)
                {
                    PerformLBAimingAction();
                }
            }
            else
            {
                if(playerInventoryManager.leftWeapon.weaponType == WeaponType.Shield || 
                    playerInventoryManager.leftWeapon.weaponType == WeaponType.StraightSword)
                {
                    PerformLBBlockingAction();
                }
                else if(playerInventoryManager.leftWeapon.weaponType == WeaponType.FaithCaster ||
                    playerInventoryManager.leftWeapon.weaponType == WeaponType.PyromancyCaster ||
                    playerInventoryManager.leftWeapon.weaponType == WeaponType.SpellCaster)
                {
                    PerformMagicAction(playerInventoryManager.leftWeapon, true);
                    playerAnimatorManager.animator.SetBool("isUsingLeftHand", true);
                }
            }
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



        void HandleLightWeaponCombo(WeaponItem weapon)
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

        void HandleHeavyWeaponCombo(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
                return;

            if (inputHandler.comboFlag)
            {
                playerAnimatorManager.animator.SetBool("canDoCombo", false);

                if (lastAttack == oh_heavy_attack_01)
                {
                    playerAnimatorManager.PlayTargetAnimation(oh_heavy_attack_02, true);
                }
                else if (lastAttack == th_heavy_attack_01)
                {
                    playerAnimatorManager.PlayTargetAnimation(th_heavy_attack_02, true);
                }
            }
        }

        void HandleLightAttack(WeaponItem weapon)
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

        void HandleHeavyAttack(WeaponItem weapon)
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

        void HandleRunningAttack(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
                return;

            playerWeaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                playerAnimatorManager.PlayTargetAnimation(th_running_attack_01, false);
                lastAttack = th_running_attack_01;
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation(oh_running_attack_01, false);
                lastAttack = oh_running_attack_01;
            }
        }

        void HandleJumpingAttack(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
                return;

            playerWeaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                playerAnimatorManager.PlayTargetAnimation(th_jumping_attack_01, false);
                lastAttack = th_jumping_attack_01;
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation(oh_jumping_attack_01, false);
                lastAttack = oh_jumping_attack_01;
            }
        }



        private void DrawArrowAction()
        {
            playerAnimatorManager.animator.SetBool("isHoldingArrow", true);
            playerAnimatorManager.PlayTargetAnimation("Bow_TH_Draw_01", true);
            GameObject loadedArrow = Instantiate(playerInventoryManager.currentAmmo.loadedItemModel,playerWeaponSlotManager.leftHandSlot.transform);
            Invoke("PlayBowAnimation", 1f);
            playerEffectsManager.currentRangeFX = loadedArrow;
        }

        private void PlayBowAnimation()
        {
            Animator bowAnimator = playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", true);
            bowAnimator.Play("Bow_TH_Draw_01");
        }

        public void FireArrowAction()
        {
            //Create the live arrow instantiation location
            ArrowInstantiationLocation arrowInstantiationLocation;
            arrowInstantiationLocation = playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();

            //Animate the bow firing the arrow
            Animator bowAnimator = playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", false);
            bowAnimator.Play("Bow_TH_Fire_01");
            Destroy(playerEffectsManager.currentRangeFX); //Destroys the loaded arrow model

            //Reset the players holding arrow flag
            playerAnimatorManager.PlayTargetAnimation("Bow_TH_Fire_01", true);
            playerAnimatorManager.animator.SetBool("isHoldingArrow", false);

            //Create and fire the live arrow
            GameObject liveArrow = Instantiate(playerInventoryManager.currentAmmo.liveAmmoModel, arrowInstantiationLocation.transform.position, cameraHandler.cameraPivotTransform.rotation);
            Rigidbody rigidbody = liveArrow.GetComponentInChildren<Rigidbody>();
            RangedProjectileDamageCollider damageCollider = liveArrow.GetComponentInChildren<RangedProjectileDamageCollider>();

            if (playerManager.isAiming)
            {
                Ray ray = cameraHandler.cameraObject.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hitPoint;

                if(Physics.Raycast(ray, out hitPoint, 100.0f))
                {
                    liveArrow.transform.LookAt(hitPoint.point);
                }
                else
                {
                    liveArrow.transform.rotation = Quaternion.Euler(cameraHandler.cameraTransform.localEulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0);
                }
            }
            else
            {
                //Give ammo velocity
                if (cameraHandler.currentLockOnTarget != null)
                {
                    Quaternion arrowRotation = 
                        Quaternion.LookRotation(cameraHandler.currentLockOnTarget.lockOnTransform.position - liveArrow.gameObject.transform.position);
                    liveArrow.transform.rotation = arrowRotation;
                }
                else
                {
                    liveArrow.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0);
                }
            }
            
            rigidbody.AddForce(liveArrow.transform.forward * playerInventoryManager.currentAmmo.forwardVelocity);
            rigidbody.AddForce(liveArrow.transform.up * playerInventoryManager.currentAmmo.upwardVelocity);
            rigidbody.useGravity = playerInventoryManager.currentAmmo.useGravity;
            rigidbody.mass = playerInventoryManager.currentAmmo.ammoMass;
            liveArrow.transform.parent = null;

            //Set live arrow damage
            damageCollider.characterManager = playerManager;
            damageCollider.ammoItem = playerInventoryManager.currentAmmo;
            damageCollider.physicalDamage = playerInventoryManager.currentAmmo.physiscalDamage;
        }



        private void PerformRBMeleeAction()
        {
            playerAnimatorManager.animator.SetBool("isUsingRightHand", true);

            if (playerManager.isSprinting)
            {
                HandleRunningAttack(playerInventoryManager.rightWeapon);
                playerEffectsManager.PlayWeaponFX(false);
                return;
            }

            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleLightWeaponCombo(playerInventoryManager.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;
                if (playerManager.canDoCombo)
                    return;

                HandleLightAttack(playerInventoryManager.rightWeapon);
            }

            playerEffectsManager.PlayWeaponFX(false);
        }

        private void PerformRBRangedAttack()
        {
            if(playerStatsManager.currentStamina <= 0) return;

            playerAnimatorManager.EraseHandIKForWeapon();
            playerAnimatorManager.animator.SetBool("isUsingRightHand", true);

            if(!playerManager.isHoldingArrow)
            {
                if (playerInventoryManager.currentAmmo != null)
                {
                    DrawArrowAction();
                }
                else
                {
                    playerAnimatorManager.PlayTargetAnimation("No", true);
                }
            }
        }

        private void PerformRTMeleeAction()
        {
            playerAnimatorManager.animator.SetBool("isUsingRightHand", true);

            if (playerManager.isSprinting)
            {
                HandleJumpingAttack(playerInventoryManager.rightWeapon);
                playerEffectsManager.PlayWeaponFX(false);
                return;
            }

            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleHeavyWeaponCombo(playerInventoryManager.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;
                if (playerManager.canDoCombo)
                    return;

                HandleHeavyAttack(playerInventoryManager.rightWeapon);
            }

            playerEffectsManager.PlayWeaponFX(false);
        }

        private void PerformLBAimingAction()
        {
            if (playerManager.isAiming)
                return;

            inputHandler.uiManager.crossHair.SetActive(true);
            playerManager.isAiming = true;
        }

        private void PerformLBBlockingAction()
        {
            if (playerManager.isInteracting)
                return;

            if (playerManager.isBlocking)
                return;

            playerAnimatorManager.PlayTargetAnimation("Block", false, true);
            playerEquipmentManager.OpenBlockingCollider();
            playerManager.isBlocking = true;
        }



        private void PerformMagicAction(WeaponItem weapon, bool isLeftHanded)
        {
            if (playerManager.isInteracting)
                return;

            if (weapon.weaponType == WeaponType.FaithCaster)
            {
                if(playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isFaithSpell)
                {
                    if(playerStatsManager.currentFocusPoints >= playerInventoryManager.currentSpell.focusPointCost)
                    {
                        playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager, isLeftHanded);
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
                        playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager, isLeftHanded);
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
            playerInventoryManager.currentSpell.SuccessfullyCastSpell
                (playerAnimatorManager, playerStatsManager, cameraHandler, playerWeaponSlotManager, playerManager.isUsingLeftHand);
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

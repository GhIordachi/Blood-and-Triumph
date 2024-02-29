using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;

        [Header("Movement Stats")]
        [SerializeField]
        float walkingSpeed = 3;
        [SerializeField]
        float movementSpeed = 5;
        [SerializeField]
        float sprintSpeed = 7;
        [SerializeField]
        float rotationSpeed = 10;

        [Header("Stamina Costs")]
        [SerializeField]
        int rollStaminaCost = 15;
        int backStepStaminaCost = 12;
        int sprintStaminaCost = 1;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();
        }

        public void HandleRotation()
        {
            if (player.canRotate)
            {
                if (player.isAiming)
                {
                    Quaternion targetRotation = Quaternion.Euler(0, player.cameraHandler.cameraTransform.eulerAngles.y, 0);
                    Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = playerRotation;
                }
                else
                {
                    if (player.inputHandler.lockOnFlag)
                    {
                        if (player.isSprinting || player.inputHandler.rollFlag)
                        {
                            Vector3 targetDirection = Vector3.zero;
                            targetDirection = player.cameraHandler.cameraTransform.forward * player.inputHandler.vertical;
                            targetDirection += player.cameraHandler.cameraTransform.right * player.inputHandler.horizontal;
                            targetDirection.Normalize();
                            targetDirection.y = 0;

                            if (targetDirection == Vector3.zero)
                                targetDirection = transform.forward;

                            Quaternion tr = Quaternion.LookRotation(targetDirection);
                            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                            transform.rotation = targetRotation;
                        }
                        else
                        {
                            Vector3 rotationDirection = moveDirection;
                            rotationDirection = player.cameraHandler.currentLockOnTarget.transform.position - transform.position;
                            rotationDirection.y = 0;
                            rotationDirection.Normalize();

                            Quaternion tr = Quaternion.LookRotation(rotationDirection);
                            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                            transform.rotation = targetRotation;
                        }

                    }
                    else
                    {
                        Vector3 targetDir = Vector3.zero;
                        float moveOverride = player.inputHandler.moveAmount;

                        targetDir = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
                        targetDir += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;

                        targetDir.Normalize();
                        targetDir.y = 0;

                        if (targetDir == Vector3.zero)
                            targetDir = player.transform.forward;

                        float rs = rotationSpeed;

                        Quaternion tr = Quaternion.LookRotation(targetDir);
                        Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, rs * Time.deltaTime);

                        player.transform.rotation = targetRotation;
                    }
                }
            }
            
        }

        public void HandleGroundMovement()
        {
            if (player.inputHandler.rollFlag)
                return;

            if (player.isInteracting)
                return;

            if(!player.isGrounded) 
                return;

            moveDirection = player.cameraHandler.transform.forward * player.inputHandler.vertical;
            moveDirection = moveDirection + player.cameraHandler.transform.right * player.inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            if (player.isSprinting && player.inputHandler.moveAmount > 0.5f)
            {
                player.characterController.Move(moveDirection * sprintSpeed * Time.deltaTime);
                player.playerStatsManager.DeductSprintingStamina(sprintStaminaCost);
            }
            else
            {
                if(player.inputHandler.moveAmount > 0.5f)
                {
                    player.characterController.Move(moveDirection * movementSpeed * Time.deltaTime);
                }
                else if(player.inputHandler.moveAmount <= 0.5f)
                {
                    player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
                }
            }

            if (player.inputHandler.lockOnFlag && player.isSprinting == false)
            {
                player.playerAnimatorManager.UpdateAnimatorValues(player.inputHandler.vertical, player.inputHandler.horizontal, player.isSprinting);
            }
            else
            {
                player.playerAnimatorManager.UpdateAnimatorValues(player.inputHandler.moveAmount, 0, player.isSprinting);
            }
        }

        public void HandleRollingAndSprinting()
        {
            if (player.animator.GetBool("isInteracting"))
                return;

            //Check if we have stamina, if we do not, return.
            if (player.playerStatsManager.currentStamina <= 0)
                return;

            if(player.inputHandler.rollFlag)
            {
                player.inputHandler.rollFlag = false;

                moveDirection = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
                moveDirection += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;

                if(player.inputHandler.moveAmount > 0)
                {
                    switch(player.playerStatsManager.encumbraceLevel)
                    {
                        case EncumbranceLevel.Light:
                            player.playerAnimatorManager.PlayTargetAnimation("Rolling", true);
                            break;
                        case EncumbranceLevel.Medium:
                            player.playerAnimatorManager.PlayTargetAnimation("Rolling", true);
                            break;
                        case EncumbranceLevel.Heavy:
                            player.playerAnimatorManager.PlayTargetAnimation("Heavy Roll", true);
                            break;
                        case EncumbranceLevel.Overloaded:
                            player.playerAnimatorManager.PlayTargetAnimation("Heavy Roll", true);
                            break;
                        default:
                            break;
                    }

                    player.playerAnimatorManager.EraseHandIKForWeapon();
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    player.transform.rotation = rollRotation;
                    player.playerStatsManager.DeductStamina(rollStaminaCost);
                }
                else
                {
                    switch (player.playerStatsManager.encumbraceLevel)
                    {
                        case EncumbranceLevel.Light:
                            player.playerAnimatorManager.PlayTargetAnimation("StepBack", true);
                            break;
                        case EncumbranceLevel.Medium:
                            player.playerAnimatorManager.PlayTargetAnimation("StepBack", true);
                            break;
                        case EncumbranceLevel.Heavy:
                            player.playerAnimatorManager.PlayTargetAnimation("StepBack", true);
                            break;
                        case EncumbranceLevel.Overloaded:
                            player.playerAnimatorManager.PlayTargetAnimation("StepBack", true);
                            break;
                        default:
                            break;
                    }

                    player.playerAnimatorManager.EraseHandIKForWeapon();
                    player.playerStatsManager.DeductStamina(backStepStaminaCost);
                }
            }
        }

        public void HandleJumping()
        {            
            if (player.isInteracting)
                return;

            if (player.playerStatsManager.currentStamina <= 0)
                return;

            if (player.inputHandler.jump_Input)
            {
                player.inputHandler.jump_Input = false;

                if (player.inputHandler.moveAmount > 0)
                {
                    moveDirection = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
                    moveDirection += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;
                    player.playerAnimatorManager.PlayTargetAnimation("Jump", true);
                    player.playerAnimatorManager.EraseHandIKForWeapon();
                    moveDirection.y = 0;
                    Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                    player.transform.rotation = jumpRotation;
                }
            }
        }
    }
}

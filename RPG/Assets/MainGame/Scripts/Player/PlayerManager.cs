using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GI
{
    public class PlayerManager : CharacterManager
    { 
        [Header("Camera")]
        public CameraHandler cameraHandler;

        [Header("UI")]
        public UIManager UIManager;

        [Header("Input")]
        public InputHandler inputHandler;

        [Header("Player")]
        public PlayerLocomotionManager playerLocomotionManager;
        public PlayerStatsManager playerStatsManager;
        public PlayerWeaponSlotManager playerWeaponSlotManager;
        public PlayerCombatManager playerCombatManager;
        public PlayerInventoryManager playerInventoryManager;
        public PlayerEffectsManager playerEffectsManager;
        public PlayerAnimatorManager playerAnimatorManager;
        public PlayerEquipmentManager playerEquipmentManager;

        [Header("Interactables")]
        InteractableUI interactableUI;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;

        protected override void Awake()
        {
            base.Awake();
            cameraHandler = FindObjectOfType<CameraHandler>();
            UIManager = FindObjectOfType<UIManager>();
            interactableUI = FindObjectOfType<InteractableUI>();
            inputHandler = GetComponent<InputHandler>();
            animator = GetComponent<Animator>();

            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        }

        void Update()
        {
            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("canDoCombo");
            canRotate = animator.GetBool("canRotate");
            isInvulnerable = animator.GetBool("isInvulnerable");
            isFiringSpell = animator.GetBool("isFiringSpell");
            isHoldingArrow = animator.GetBool("isHoldingArrow");
            isPerformingFullyChargedAttack = animator.GetBool("isPerformingFullyChargedAttack");
            animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
            animator.SetBool("isBlocking", isBlocking);
            animator.SetBool("isInAir", isInAir);
            animator.SetBool("isDead", isDead);

            inputHandler.TickInput();            
            playerLocomotionManager.HandleRollingAndSprinting();         
            playerLocomotionManager.HandleJumping();
            playerStatsManager.RegenerateStamina();

            CheckForInteractableObject();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            playerLocomotionManager.HandleFalling(playerLocomotionManager.moveDirection);
            playerLocomotionManager.HandleMovement();
            playerLocomotionManager.HandleRotation();
            playerEffectsManager.HandleAllBuildUpEffects();
        }

        private void LateUpdate()
        {
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.t_Input = false;
            inputHandler.inventory_Input = false;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget();
                cameraHandler.HandleCameraRotation();
            }

            if (isInAir)
            {
                playerLocomotionManager.inAirTimer = playerLocomotionManager.inAirTimer + Time.deltaTime;
            }
        }

        #region Player Interactions
        public void CheckForInteractableObject()
        {
            RaycastHit hit;

            if(Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {
                if(hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if(interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);

                        if (inputHandler.t_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            else
            {
                if(interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if(itemInteractableGameObject != null && inputHandler.t_Input)
                {
                    itemInteractableGameObject.SetActive(false);
                }
            }
        }

        public void OpenChestInteraction(Transform playerStandingHereWhenOpeningChest)
        {
            playerLocomotionManager.rigidbody.velocity = Vector3.zero; //Stops the player from ice skating
            transform.position = playerStandingHereWhenOpeningChest.transform.position;
            //Change the animation to an open chest animation
            playerAnimatorManager.PlayTargetAnimation("Pick Up Item", true);
        }

        public void PassThroughFogWallInteraction(Transform fogWallEntrance)
        {
            playerLocomotionManager.rigidbody.velocity = Vector3.zero; //Stops the player from ice skating

            Vector3 rotationDirection = fogWallEntrance.transform.forward;
            Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = turnRotation;

            playerAnimatorManager.PlayTargetAnimation("Pass Through Fog", true);
        }

        #endregion

    }
}
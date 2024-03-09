using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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

            WorldSaveGameManager.instance.player = this;
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();

            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("canDoCombo");
            canRotate = animator.GetBool("canRotate");
            isInvulnerable = animator.GetBool("isInvulnerable");
            isFiringSpell = animator.GetBool("isFiringSpell");
            isHoldingArrow = animator.GetBool("isHoldingArrow");
            isPerformingFullyChargedAttack = animator.GetBool("isPerformingFullyChargedAttack");
            animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
            animator.SetBool("isBlocking", isBlocking);
            animator.SetBool("isDead", isDead);

            inputHandler.TickInput();            
            playerLocomotionManager.HandleRollingAndSprinting();         
            playerLocomotionManager.HandleJumping();
            playerStatsManager.RegenerateStamina();

            playerLocomotionManager.HandleGroundMovement();
            playerLocomotionManager.HandleRotation();

            CheckForInteractableObject();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        private void LateUpdate()
        {
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.left_Arrow_Input = false;
            inputHandler.right_Arrow_Input = false;
            inputHandler.t_Input = false;
            inputHandler.inventory_Input = false;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget();
                cameraHandler.HandleCameraRotation();
            }
        }

        #region Player Interactions
        public void CheckForInteractableObject()
        {
            RaycastHit hit;

            if (cameraHandler == null)
                return;

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
            playerLocomotionManager.GetComponent<Rigidbody>().velocity = Vector3.zero; //Stops the player from ice skating
            transform.position = playerStandingHereWhenOpeningChest.transform.position;
            //Change the animation to an open chest animation
            playerAnimatorManager.PlayTargetAnimation("Pick Up Item", true);
        }

        public void PassThroughFogWallInteraction(Transform fogWallEntrance)
        {
            playerLocomotionManager.GetComponent<Rigidbody>().velocity = Vector3.zero; //Stops the player from ice skating

            Vector3 rotationDirection = fogWallEntrance.transform.forward;
            Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = turnRotation;

            playerAnimatorManager.PlayTargetAnimation("Pass Through Fog", true);
        }

        #endregion

        public void SaveCharacterDataToCurrentSaveData(ref CharacterSaveData currentCharacterSaveData)
        {
            Scene currentScene = SceneManager.GetActiveScene();

            currentCharacterSaveData.characterName = playerStatsManager.characterName;
            currentCharacterSaveData.characterLevel = playerStatsManager.playerLevel;
            currentCharacterSaveData.currentScene = currentScene.buildIndex;

            currentCharacterSaveData.xPosition = transform.position.x;
            currentCharacterSaveData.yPosition = transform.position.y;
            currentCharacterSaveData.zPosition = transform.position.z;

            currentCharacterSaveData.characterRightHandWeaponID = playerInventoryManager.rightWeapon.itemID;
            currentCharacterSaveData.characterLeftHandWeaponID = playerInventoryManager.leftWeapon.itemID;
            
            if(playerEquipmentManager.hair != null)
            {
                currentCharacterSaveData.currentHairID = playerEquipmentManager.hairID;
            }

            if (playerEquipmentManager.eyebrows != null)
            {
                currentCharacterSaveData.currentEyebrowID = playerEquipmentManager.eyebrowID;
            }

            if (playerEquipmentManager.beard != null)
            {
                currentCharacterSaveData.currentBeardID = playerEquipmentManager.beardID;
            }

            currentCharacterSaveData.currentHairColor = playerEquipmentManager.hairColor;

            if (playerInventoryManager.currentHelmetEquipment != null)
            {
                currentCharacterSaveData.currentHeadGearItemID = playerInventoryManager.currentHelmetEquipment.itemID;
            }
            else
            {
                currentCharacterSaveData.currentHeadGearItemID = -1;
            }
            if (playerInventoryManager.currentBodyEquipment != null)
            {
                currentCharacterSaveData.currentChestGearItemID = playerInventoryManager.currentBodyEquipment.itemID;
            }
            else
            {
                currentCharacterSaveData.currentChestGearItemID = -1;
            }
            if (playerInventoryManager.currentLegEquipment != null)
            {
                currentCharacterSaveData.currentLegGearItemID = playerInventoryManager.currentLegEquipment.itemID;
            }
            else
            {
                currentCharacterSaveData.currentLegGearItemID = -1;
            }
            if (playerInventoryManager.currentHandEquipment != null)
            {
                currentCharacterSaveData.currentHandGearItemID = playerInventoryManager.currentHandEquipment.itemID;
            }
            else
            {
                currentCharacterSaveData.currentHandGearItemID = -1;
            }
        }

        public void LoadCharacterDataFromCurrentCharacterSaveData(ref CharacterSaveData currentCharacterSaveData)
        {
            playerStatsManager.characterName = currentCharacterSaveData.characterName;
            playerStatsManager.playerLevel = currentCharacterSaveData.characterLevel;

            //Asign the position saved in the file
            transform.position = new Vector3(currentCharacterSaveData.xPosition, currentCharacterSaveData.yPosition, currentCharacterSaveData.zPosition);

            //Facial Features
            playerEquipmentManager.hairID = currentCharacterSaveData.currentHairID;
            playerEquipmentManager.eyebrowID = currentCharacterSaveData.currentEyebrowID;
            playerEquipmentManager.beardID = currentCharacterSaveData.currentBeardID;
            playerEquipmentManager.hairColor = currentCharacterSaveData.currentHairColor;

            //Equipment
            playerInventoryManager.rightWeapon = WorldItemDataBase.Instance.GetWeaponItemByID(currentCharacterSaveData.characterRightHandWeaponID);
            playerInventoryManager.leftWeapon = WorldItemDataBase.Instance.GetWeaponItemByID(currentCharacterSaveData.characterLeftHandWeaponID);
            playerWeaponSlotManager.LoadBothWeaponsOnSlots();

            EquipmentItem headEquipment = WorldItemDataBase.Instance.GetEquipmentItemByID(currentCharacterSaveData.currentHeadGearItemID);

            //if this item exists in the database, we apply it
            if(headEquipment != null )
            {
                playerInventoryManager.currentHelmetEquipment = headEquipment as HelmetEquipment;
            }

            EquipmentItem bodyEquipment = WorldItemDataBase.Instance.GetEquipmentItemByID(currentCharacterSaveData.currentChestGearItemID);

            if(bodyEquipment != null )
            {
                playerInventoryManager.currentBodyEquipment = bodyEquipment as BodyEquipment;
            }

            EquipmentItem legEquipment = WorldItemDataBase.Instance.GetEquipmentItemByID(currentCharacterSaveData.currentLegGearItemID);

            if (legEquipment != null)
            {
                playerInventoryManager.currentLegEquipment = legEquipment as LegEquipment;
            }

            EquipmentItem handEquipment = WorldItemDataBase.Instance.GetEquipmentItemByID(currentCharacterSaveData.currentHandGearItemID);

            if (handEquipment != null)
            {
                playerInventoryManager.currentHandEquipment = handEquipment as HandEquipment;
            }

            playerEquipmentManager.EquipAllArmor();
        }
    }
}
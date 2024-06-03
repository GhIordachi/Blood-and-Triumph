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
            isMounted = animator.GetBool("isMounted");
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

        private void LateUpdate()
        {
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.left_Arrow_Input = false;
            inputHandler.right_Arrow_Input = false;
            inputHandler.e_Input = false;
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
                if (hit.collider.CompareTag("Interactable"))
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if(interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);

                        if (inputHandler.e_Input)
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

                if(itemInteractableGameObject != null && inputHandler.e_Input)
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
            //General Information
            currentCharacterSaveData.characterName = playerStatsManager.characterName;
            currentCharacterSaveData.currentScene = currentScene.buildIndex;
            currentCharacterSaveData.currentGold = playerInventoryManager.currentGold;
            //Player Levels
            currentCharacterSaveData.currentXPCount = playerStatsManager.currentXPCount;
            currentCharacterSaveData.characterLevel = playerStatsManager.playerLevel;
            currentCharacterSaveData.healthLevel = playerStatsManager.healthLevel;
            currentCharacterSaveData.staminaLevel = playerStatsManager.staminaLevel;
            currentCharacterSaveData.poiseLevel = playerStatsManager.poiseLevel;
            currentCharacterSaveData.focusLevel = playerStatsManager.focusLevel;
            currentCharacterSaveData.strengthLevel = playerStatsManager.strengthLevel;
            currentCharacterSaveData.faithLevel = playerStatsManager.faithLevel;
            //Player Position
            currentCharacterSaveData.xPosition = transform.position.x;
            currentCharacterSaveData.yPosition = transform.position.y;
            currentCharacterSaveData.zPosition = transform.position.z;
            //Player Current Equiped Items
            currentCharacterSaveData.characterRightHandWeaponID = playerInventoryManager.rightWeapon.itemID;
            currentCharacterSaveData.characterLeftHandWeaponID = playerInventoryManager.leftWeapon.itemID;
            currentCharacterSaveData.characterCurrentRightWeaponIndex = playerInventoryManager.currentRightWeaponIndex;
            currentCharacterSaveData.characterCurrentLeftWeaponIndex = playerInventoryManager.currentLeftWeaponIndex;

            if(playerInventoryManager.currentSpell != null)
                currentCharacterSaveData.characterCurrentSpellID = playerInventoryManager.currentSpell.itemID;
            if (playerInventoryManager.currentConsumable != null)
                currentCharacterSaveData.characterCurrentConsumableID = playerInventoryManager.currentConsumable.itemID;
            if (playerInventoryManager.currentAmmo != null)
                currentCharacterSaveData.characterCurrentAmmoID = playerInventoryManager.currentAmmo.itemID;

            if(playerInventoryManager.ringSlot01 != null)
                currentCharacterSaveData.ringSlot01 = playerInventoryManager.ringSlot01.itemID;
            if (playerInventoryManager.ringSlot02 != null)
                currentCharacterSaveData.ringSlot02 = playerInventoryManager.ringSlot02.itemID;
            if (playerInventoryManager.ringSlot03 != null)
                currentCharacterSaveData.ringSlot03 = playerInventoryManager.ringSlot03.itemID;
            if (playerInventoryManager.ringSlot04 != null)
                currentCharacterSaveData.ringSlot04 = playerInventoryManager.ringSlot04.itemID;

            if (playerEquipmentManager.hair != null)
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

            // Save right hand weapons
            currentCharacterSaveData.weaponsInRightHandByID.Clear();
            for (int i = 0; i < 4; i++)
            {
                if (playerInventoryManager.weaponsInRightHandSlots[i] != null)
                    currentCharacterSaveData.weaponsInRightHandByID.Add(playerInventoryManager.weaponsInRightHandSlots[i].itemID);
                else
                    currentCharacterSaveData.weaponsInRightHandByID.Add(-1);
            }

            // Save left hand weapons
            currentCharacterSaveData.weaponsInLeftHandByID.Clear();
            for (int i = 0; i < 4; i++)
            {
                if (playerInventoryManager.weaponsInLeftHandSlots[i] != null)
                    currentCharacterSaveData.weaponsInLeftHandByID.Add(playerInventoryManager.weaponsInLeftHandSlots[i].itemID);
                else
                    currentCharacterSaveData.weaponsInLeftHandByID.Add(-1);
            }

            //Player Inventory
            for (int i = 0; i <playerInventoryManager.weaponsInventory.Count; i++)
            {
                currentCharacterSaveData.weaponsInInventoryByID.Add(playerInventoryManager.weaponsInventory[i].itemID);
            }

            for (int i = 0; i < playerInventoryManager.headEquipmentInventory.Count; i++)
            {
                currentCharacterSaveData.helmetsInInventoryByID.Add(playerInventoryManager.headEquipmentInventory[i].itemID);
            }

            for (int i = 0; i < playerInventoryManager.bodyEquipmentInventory.Count; i++)
            {
                currentCharacterSaveData.bodyInInventoryByID.Add(playerInventoryManager.bodyEquipmentInventory[i].itemID);
            }

            for (int i = 0; i < playerInventoryManager.legEquipmentInventory.Count; i++)
            {
                currentCharacterSaveData.legsInInventoryByID.Add(playerInventoryManager.legEquipmentInventory[i].itemID);
            }

            for (int i = 0; i < playerInventoryManager.handEquipmentInventory.Count; i++)
            {
                currentCharacterSaveData.armsInInventoryByID.Add(playerInventoryManager.handEquipmentInventory[i].itemID);
            }

            for (int i = 0; i < playerInventoryManager.ringItemInventory.Count; i++)
            {
                currentCharacterSaveData.ringsInInventoryByID.Add(playerInventoryManager.ringItemInventory[i].itemID);
            }

            for (int i = 0; i < playerInventoryManager.spellInventory.Count; i++)
            {
                currentCharacterSaveData.spellsInInventoryByID.Add(playerInventoryManager.spellInventory[i].itemID);
            }

            for (int i = 0; i < playerInventoryManager.consumableInventory.Count; i++)
            {
                currentCharacterSaveData.consumablesInInventoryByID.Add(playerInventoryManager.consumableInventory[i].itemID);
            }

            for (int i = 0; i < playerInventoryManager.ammoInventory.Count; i++)
            {
                currentCharacterSaveData.ammoInInventoryByID.Add(playerInventoryManager.ammoInventory[i].itemID);
            }
        }

        public void LoadCharacterDataFromCurrentCharacterSaveData(ref CharacterSaveData currentCharacterSaveData)
        {
            playerStatsManager.characterName = currentCharacterSaveData.characterName;
            playerInventoryManager.currentGold = currentCharacterSaveData.currentGold;

            playerStatsManager.currentXPCount = currentCharacterSaveData.currentXPCount;
            playerStatsManager.playerLevel = currentCharacterSaveData.characterLevel;
            playerStatsManager.healthLevel = currentCharacterSaveData.healthLevel;
            playerStatsManager.staminaLevel = currentCharacterSaveData.staminaLevel;
            playerStatsManager.focusLevel = currentCharacterSaveData.focusLevel;
            playerStatsManager.poiseLevel = currentCharacterSaveData.poiseLevel;
            playerStatsManager.strengthLevel = currentCharacterSaveData.strengthLevel;
            playerStatsManager.faithLevel = currentCharacterSaveData.faithLevel;

            //Asign the position saved in the file
            transform.position = new Vector3(currentCharacterSaveData.xPosition, currentCharacterSaveData.yPosition, currentCharacterSaveData.zPosition);

            //Facial Features
            playerEquipmentManager.hairID = currentCharacterSaveData.currentHairID;
            playerEquipmentManager.eyebrowID = currentCharacterSaveData.currentEyebrowID;
            playerEquipmentManager.beardID = currentCharacterSaveData.currentBeardID;
            playerEquipmentManager.hairColor = currentCharacterSaveData.currentHairColor;

            //Items Equiped
            playerInventoryManager.rightWeapon = WorldItemDataBase.Instance.GetWeaponItemByID(currentCharacterSaveData.characterRightHandWeaponID);
            playerInventoryManager.leftWeapon = WorldItemDataBase.Instance.GetWeaponItemByID(currentCharacterSaveData.characterLeftHandWeaponID);
            playerInventoryManager.currentRightWeaponIndex = currentCharacterSaveData.characterCurrentRightWeaponIndex;
            playerInventoryManager.currentLeftWeaponIndex = currentCharacterSaveData.characterCurrentLeftWeaponIndex;
            playerWeaponSlotManager.LoadBothWeaponsOnSlots();

            playerInventoryManager.currentSpell = WorldItemDataBase.Instance.GetSpellItemByID(currentCharacterSaveData.characterCurrentSpellID);
            playerInventoryManager.currentConsumable = WorldItemDataBase.Instance.GetConsumableItemByID(currentCharacterSaveData.characterCurrentConsumableID);
            playerInventoryManager.currentAmmo = WorldItemDataBase.Instance.GetRangedAmmoItemByID(currentCharacterSaveData.characterCurrentAmmoID);
            playerInventoryManager.ringSlot01 = WorldItemDataBase.Instance.GetRingItemByID(currentCharacterSaveData.ringSlot01);
            playerInventoryManager.ringSlot02 = WorldItemDataBase.Instance.GetRingItemByID(currentCharacterSaveData.ringSlot02);
            playerInventoryManager.ringSlot03 = WorldItemDataBase.Instance.GetRingItemByID(currentCharacterSaveData.ringSlot03);
            playerInventoryManager.ringSlot04 = WorldItemDataBase.Instance.GetRingItemByID(currentCharacterSaveData.ringSlot04);

            // Load right hand weapons
            for (int i = 0; i < currentCharacterSaveData.weaponsInRightHandByID.Count; i++)
            {
                int weaponID = currentCharacterSaveData.weaponsInRightHandByID[i];
                WeaponItem weapon = WorldItemDataBase.Instance.GetWeaponItemByID(weaponID);
                if (weapon != null)
                {
                    playerInventoryManager.weaponsInRightHandSlots[i] = weapon;
                }

                if(weaponID == -1)
                {
                    playerInventoryManager.weaponsInRightHandSlots[i] = null;
                }
            }

            // Load left hand weapons
            for (int i = 0; i < currentCharacterSaveData.weaponsInLeftHandByID.Count; i++)
            {
                int weaponID = currentCharacterSaveData.weaponsInLeftHandByID[i];
                WeaponItem weapon = WorldItemDataBase.Instance.GetWeaponItemByID(weaponID);
                if (weapon != null)
                {
                    playerInventoryManager.weaponsInLeftHandSlots[i] = weapon;
                }

                if (weaponID == -1)
                {
                    playerInventoryManager.weaponsInLeftHandSlots[i] = null;
                }
            }

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

            // Player's Inventory
            playerInventoryManager.weaponsInventory.Clear();
            playerInventoryManager.headEquipmentInventory.Clear();
            playerInventoryManager.bodyEquipmentInventory.Clear();
            playerInventoryManager.legEquipmentInventory.Clear();
            playerInventoryManager.handEquipmentInventory.Clear();
            playerInventoryManager.ringItemInventory.Clear();
            playerInventoryManager.spellInventory.Clear();
            playerInventoryManager.consumableInventory.Clear();
            playerInventoryManager.ammoInventory.Clear();

            // Load weapons
            for (int i = 0; i < currentCharacterSaveData.weaponsInInventoryByID.Count; i++)
            {
                int weaponID = currentCharacterSaveData.weaponsInInventoryByID[i];
                WeaponItem weapon = WorldItemDataBase.Instance.GetWeaponItemByID(weaponID);
                if (weapon != null)
                {
                    playerInventoryManager.weaponsInventory.Add(weapon);
                }
            }

            // Load helmets
            for (int i = 0; i < currentCharacterSaveData.helmetsInInventoryByID.Count; i++)
            {
                int helmetID = currentCharacterSaveData.helmetsInInventoryByID[i];
                EquipmentItem item = WorldItemDataBase.Instance.GetEquipmentItemByID(helmetID);
                HelmetEquipment helmet = item as HelmetEquipment;
                if (helmet != null)
                {
                    playerInventoryManager.headEquipmentInventory.Add(helmet);
                }
            }

            // Load body equipment
            for (int i = 0; i < currentCharacterSaveData.bodyInInventoryByID.Count; i++)
            {
                int bodyID = currentCharacterSaveData.bodyInInventoryByID[i];
                EquipmentItem item = WorldItemDataBase.Instance.GetEquipmentItemByID(bodyID);
                BodyEquipment body = item as BodyEquipment;
                if (body != null)
                {
                    playerInventoryManager.bodyEquipmentInventory.Add(body);
                }
            }

            // Load leg equipment
            for (int i = 0; i < currentCharacterSaveData.legsInInventoryByID.Count; i++)
            {
                int legID = currentCharacterSaveData.legsInInventoryByID[i];
                EquipmentItem item = WorldItemDataBase.Instance.GetEquipmentItemByID(legID);
                LegEquipment leg = item as LegEquipment;
                if (leg != null)
                {
                    playerInventoryManager.legEquipmentInventory.Add(leg);
                }
            }

            // Load hand equipment
            for (int i = 0; i < currentCharacterSaveData.armsInInventoryByID.Count; i++)
            {
                int handID = currentCharacterSaveData.armsInInventoryByID[i];
                EquipmentItem item = WorldItemDataBase.Instance.GetEquipmentItemByID(handID);
                HandEquipment hand = item as HandEquipment;
                if (hand != null)
                {
                    playerInventoryManager.handEquipmentInventory.Add(hand);
                }
            }

            // Load rings
            for (int i = 0; i < currentCharacterSaveData.ringsInInventoryByID.Count; i++)
            {
                int ringID = currentCharacterSaveData.ringsInInventoryByID[i];
                RingItem ring = WorldItemDataBase.Instance.GetRingItemByID(ringID);
                if (ring != null)
                {
                    playerInventoryManager.ringItemInventory.Add(ring);
                }
            }

            // Load spells
            for (int i = 0; i < currentCharacterSaveData.spellsInInventoryByID.Count; i++)
            {
                int spellID = currentCharacterSaveData.spellsInInventoryByID[i];
                SpellItem spell = WorldItemDataBase.Instance.GetSpellItemByID(spellID);
                if (spell != null)
                {
                    playerInventoryManager.spellInventory.Add(spell);
                }
            }

            // Load consumables
            for (int i = 0; i < currentCharacterSaveData.consumablesInInventoryByID.Count; i++)
            {
                int consumableID = currentCharacterSaveData.consumablesInInventoryByID[i];
                ConsumableItem consumable = WorldItemDataBase.Instance.GetConsumableItemByID(consumableID);
                if (consumable != null)
                {
                    playerInventoryManager.consumableInventory.Add(consumable);
                }
            }

            // Load ammo
            for (int i = 0; i < currentCharacterSaveData.ammoInInventoryByID.Count; i++)
            {
                int ammoID = currentCharacterSaveData.ammoInInventoryByID[i];
                RangedAmmoItem ammo = WorldItemDataBase.Instance.GetRangedAmmoItemByID(ammoID);
                if (ammo != null)
                {
                    playerInventoryManager.ammoInventory.Add(ammo);
                }
            }


            playerEquipmentManager.EquipAllArmor();

            playerStatsManager.SetMaxHealthFromHealthLevel();
            playerStatsManager.currentHealth = playerStatsManager.maxHealth;
            playerStatsManager.healthBar.SetMaxHealth(playerStatsManager.maxHealth);
            playerStatsManager.healthBar.SetCurrentHealth(playerStatsManager.currentHealth);

            playerStatsManager.SetMaxStaminaFromStaminaLevel();
            playerStatsManager.currentStamina = playerStatsManager.maxStamina;
            playerStatsManager.staminaBar.SetMaxStamina(playerStatsManager.maxStamina);
            playerStatsManager.staminaBar.SetCurrentStamina(playerStatsManager.currentStamina);

            playerStatsManager.SetMaxFocusPointsFromFocusLevel();
            playerStatsManager.currentFocusPoints = playerStatsManager.maxFocusPoints;
            playerStatsManager.focusPointBar.SetMaxFocusPoint(playerStatsManager.maxFocusPoints);
            playerStatsManager.focusPointBar.SetCurrentFocusPoint(playerStatsManager.currentFocusPoints);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI {
    public class WeaponPickUp : Interactable
    {
        //This is a unique ID for this item spawn in the game world, each item you place in your world should have it's own Unique ID
        [Header("Item Information")]
        [SerializeField] int itemPickUpID;
        [SerializeField] bool hasBeenLooted;

        [Header("Item")]
        public WeaponItem weapon;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();

            //If the save data does not contain this item, we must have never looted it, so we add it to the list and list it as not looted
            if (!WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.ContainsKey(itemPickUpID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.Add(itemPickUpID, false);
            }

            hasBeenLooted = WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld[itemPickUpID];

            if (hasBeenLooted)
            {
                gameObject.SetActive(false);
            }
        }

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            // Notify the character data this item has been looted from the world, so it does not spawn again
            if (WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.ContainsKey(itemPickUpID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.Remove(itemPickUpID);
            }

            WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.Add(itemPickUpID, true);

            hasBeenLooted = true;

            // Places the item in the players Inventory
            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventoryManager playerInventory;
            PlayerLocomotionManager playerLocomotion;
            PlayerAnimatorManager animatorHandler;

            playerInventory = playerManager.GetComponent<PlayerInventoryManager>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotionManager>();
            animatorHandler = playerManager.GetComponentInChildren<PlayerAnimatorManager>();

            playerLocomotion.GetComponent<Rigidbody>().velocity = Vector3.zero; // Stops the player from moving whilst picking up the item
            animatorHandler.PlayTargetAnimation("Pick Up Item", true); // Plays the animation of looting the item
            playerInventory.weaponsInventory.Add(weapon);
            playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = weapon.itemName;
            if(weapon.itemIcon != null)
                playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
            playerManager.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}

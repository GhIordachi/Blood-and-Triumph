using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class OpenChest : Interactable
    {
        Animator animator;
        OpenChest openChest;

        public Transform playerStandingPosition;
        public GameObject itemSpawner;
        public WeaponItem itemInChest;

        protected override void Awake()
        {
            animator = GetComponent<Animator>();
            openChest = GetComponent<OpenChest>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            //Rotate the player towards the chest
            Vector3 rotationDirection = transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetDirection = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
            playerManager.transform.rotation = targetDirection;

            //Lock his transform infront of the chest
            playerManager.OpenChestInteraction(playerStandingPosition);

            //Open the chest lid and animate the player
            animator.Play("Open Chest");

            //Spawn an item inside the chest the player can pick it up
            StartCoroutine(SpawnItemInChest());
            WeaponPickUp weaponPickUp = itemSpawner.GetComponent<WeaponPickUp>();

            if(weaponPickUp != null)
            {
                weaponPickUp.weapon = itemInChest;
            }
        }

        private IEnumerator SpawnItemInChest()
        {
            yield return new WaitForSeconds(1f);
            Instantiate(itemSpawner, transform);
            Destroy(openChest);
        }
    }
}

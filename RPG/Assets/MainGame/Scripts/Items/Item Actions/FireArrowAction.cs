using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [CreateAssetMenu(menuName = "Item Actions/Fire Arrow Action")]
    public class FireArrowAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (!character.isHoldingArrow)
                return;

            PlayerManager player = character as PlayerManager;
            //Create the live arrow instantiation location
            ArrowInstantiationLocation arrowInstantiationLocation;
            arrowInstantiationLocation = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();

            //Animate the bow firing the arrow
            Animator bowAnimator = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", false);
            bowAnimator.Play("Bow_TH_Fire_01");
            Destroy(character.characterEffectsManager.instantiatedFXModel); //Destroys the loaded arrow model

            //Reset the players holding arrow flag
            character.characterAnimatorManager.PlayTargetAnimation("Bow_TH_Fire_01", true);
            character.animator.SetBool("isHoldingArrow", false);

            //Fire the arrow as a player character
            if(player != null)
            {
                Debug.Log("Am intrat");
                //Create and fire the live arrow
                GameObject liveArrow = Instantiate(character.characterInventoryManager.currentAmmo.liveAmmoModel, 
                    arrowInstantiationLocation.transform.position, player.cameraHandler.cameraPivotTransform.rotation);
                Rigidbody rigidbody = liveArrow.GetComponent<Rigidbody>();
                RangedProjectileDamageCollider damageCollider = liveArrow.GetComponent<RangedProjectileDamageCollider>();

                if (character.isAiming)
                {
                    Ray ray = player.cameraHandler.cameraObject.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                    RaycastHit hitPoint;

                    if (Physics.Raycast(ray, out hitPoint, 100.0f))
                    {
                        liveArrow.transform.LookAt(hitPoint.point);
                    }
                    else
                    {
                        liveArrow.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraTransform.localEulerAngles.x, character.lockOnTransform.eulerAngles.y, 0);
                    }
                }
                else
                {
                    //Give ammo velocity
                    if (player.cameraHandler.currentLockOnTarget != null)
                    {
                        Quaternion arrowRotation =
                            Quaternion.LookRotation(player.cameraHandler.currentLockOnTarget.lockOnTransform.position - liveArrow.gameObject.transform.position);
                        liveArrow.transform.rotation = arrowRotation;
                    }
                    else
                    {
                        liveArrow.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraPivotTransform.eulerAngles.x, character.lockOnTransform.eulerAngles.y, 0);
                    }
                }

                rigidbody.AddForce(liveArrow.transform.forward * player.playerInventoryManager.currentAmmo.forwardVelocity);
                rigidbody.AddForce(liveArrow.transform.up * player.playerInventoryManager.currentAmmo.upwardVelocity);
                rigidbody.useGravity = player.playerInventoryManager.currentAmmo.useGravity;
                rigidbody.mass = player.playerInventoryManager.currentAmmo.ammoMass;
                liveArrow.transform.parent = null;

                //Set live arrow damage
                damageCollider.characterManager = character;
                damageCollider.ammoItem = player.playerInventoryManager.currentAmmo;
                damageCollider.physicalDamage = player.playerInventoryManager.currentAmmo.physiscalDamage;
            }
            //... as an A.I character
            else
            {
                AICharacterManager enemy = character as AICharacterManager;
                //Create and fire the live arrow
                GameObject liveArrow = Instantiate(character.characterInventoryManager.currentAmmo.liveAmmoModel,
                    arrowInstantiationLocation.transform.position, Quaternion.identity);
                Rigidbody rigidbody = liveArrow.GetComponent<Rigidbody>();
                RangedProjectileDamageCollider damageCollider = liveArrow.GetComponent<RangedProjectileDamageCollider>();

                //Give ammo velocity
                if (enemy.currentTarget != null)
                {
                    Quaternion arrowRotation =
                        Quaternion.LookRotation(enemy.currentTarget.lockOnTransform.position - liveArrow.gameObject.transform.position);
                    liveArrow.transform.rotation = arrowRotation;
                }

                rigidbody.AddForce(liveArrow.transform.forward * enemy.characterInventoryManager.currentAmmo.forwardVelocity);
                rigidbody.AddForce(liveArrow.transform.up * enemy.characterInventoryManager.currentAmmo.upwardVelocity);
                rigidbody.useGravity = enemy.characterInventoryManager.currentAmmo.useGravity;
                rigidbody.mass = enemy.characterInventoryManager.currentAmmo.ammoMass;
                liveArrow.transform.parent = null;

                //Set live arrow damage
                damageCollider.characterManager = character;
                damageCollider.ammoItem = enemy.characterInventoryManager.currentAmmo;
                damageCollider.physicalDamage = enemy.characterInventoryManager.currentAmmo.physiscalDamage;
                damageCollider.teamIDNumber = enemy.characterStatsManager.teamIDNumber;
            }
        }
    }
}

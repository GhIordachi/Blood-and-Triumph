using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class BonfireInteractable : Interactable
    {
        [Header("Bonfire Teleport Transform")]
        public Transform bonfireTeleportTransform;

        [Header("Activation Status")]
        public bool hasBeenActivated;

        [Header("Bonfire FX")]
        public ParticleSystem activationFX;
        public ParticleSystem fireFX;
        public AudioClip bonfireActivationSoundFX;

        AudioSource audioSource;

        private void Awake()
        {
            //If the bonfire has already been activated by the player, play the "Fire" FX when the bonfire is loaded into the scene
            if(hasBeenActivated)
            {
                fireFX.gameObject.SetActive(true);
                fireFX.Play();
                interactableText = "Rest";
            }
            else
            {
                interactableText = "Light Bonfire";
            }

            audioSource = GetComponent<AudioSource>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            Debug.Log("Bonfire Interacted with");

            if(hasBeenActivated)
            {

            }
            else
            {
                playerManager.playerAnimatorManager.PlayTargetAnimation("Bonfire_Activate", true);
                playerManager.UIManager.ActivateBonfirePopUp();
                hasBeenActivated = true;
                interactableText = "Rest";
                activationFX.gameObject.SetActive(true);
                activationFX.Play();
                fireFX.gameObject.SetActive(true);
                fireFX.Play();
                audioSource.PlayOneShot(bonfireActivationSoundFX);
            }
        }
    }
}

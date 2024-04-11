using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class AICharacterAnimatorManager : CharacterAnimatorManager
    {
        AICharacterManager aiCharacter;

        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
        }

        public void AwardSoulsOnDeath()
        {
            //Scan for the player in the scene and award him
            PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
            SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();

            if (playerStats != null)
            {
                playerStats.AddSouls(aiCharacter.aiCharacterStatsManager.souldAwardedOnDeath);

                if (soulCountBar != null)
                {
                    soulCountBar.SetSoulCountText(playerStats.currentSoulCount);
                }
            }
        }

        public void DropItemOnDeath()
        {
            StartCoroutine(SpawnItemAfterDeath());
            WeaponPickUp weaponPickUp = aiCharacter.itemSpawner.GetComponent<WeaponPickUp>();

            if (weaponPickUp != null)
            {
                weaponPickUp.weapon = aiCharacter.weaponItemToDrop;
            }
        }

        private IEnumerator SpawnItemAfterDeath()
        {
            yield return new WaitForSeconds(1f);
            Instantiate(aiCharacter.itemSpawner, transform);
        }

        public void InstantiateBossParticleFX()
        {
            BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();

            GameObject phaseFX = Instantiate(aiCharacter.aiCharacterBossManager.particleFX, bossFXTransform.transform);
        }

        public void PlayWeaponTrailFX()
        {
            aiCharacter.aiCharacterEffectsManager.PlayWeaponFX(false);
        }

        public override void OnAnimatorMove()
        {
            Vector3 velocity = character.animator.deltaPosition;
            character.characterController.Move(velocity);

            if (aiCharacter.isRotatingWithRootMotion)
            {
                character.transform.rotation *= character.animator.deltaRotation;
            }
        }
    }
}

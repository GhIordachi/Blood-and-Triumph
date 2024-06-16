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
            SoulCountBar xpCountBar = FindObjectOfType<SoulCountBar>();

            if (playerStats != null)
            {
                playerStats.AddXP(aiCharacter.aiCharacterStatsManager.XPAwardedOnDeath);

                if (xpCountBar != null)
                {
                    xpCountBar.SetXPCountText(playerStats.currentXPCount);
                }
            }
        }

        public void ResetTheArenaBool()
        {
            ArenaManager arenaManager = FindObjectOfType<ArenaManager>();
            if (arenaManager != null)
            {
                Debug.Log("L-a gasit");
            }
            if (aiCharacter.isDead && arenaManager.level01 == true && aiCharacter.aiCharacterStatsManager.playerLevel == 1)
            {               
                arenaManager.isEnemyLevelOneDead = true;
            }
            if (aiCharacter.isDead && arenaManager.level02 == true && aiCharacter.aiCharacterStatsManager.playerLevel == 2)
            {
                arenaManager.isEnemyOneLevelTwoDead = true;
            }
            if (aiCharacter.isDead && arenaManager.level02 == true && aiCharacter.aiCharacterStatsManager.playerLevel == 3)
            {
                arenaManager.isEnemyTwoLevelTwoDead = true;
            }
            if (aiCharacter.isDead && arenaManager.level03 == true && aiCharacter.aiCharacterStatsManager.playerLevel == 3)
            {
                arenaManager.isEnemyOneLevelThreeDead = true;
            }
            if (aiCharacter.isDead && arenaManager.level03 == true && aiCharacter.aiCharacterStatsManager.playerLevel == 4)
            {
                arenaManager.isEnemyTwoLevelThreeDead = true;
            }
            if (aiCharacter.isDead && arenaManager.level03 == true && aiCharacter.aiCharacterStatsManager.playerLevel == 5)
            {
                arenaManager.isEnemyThreeLevelThreeDead = true;
            }
            if (aiCharacter.isDead && arenaManager.level03 == true && aiCharacter.aiCharacterStatsManager.playerLevel == 6)
            {
                arenaManager.isEnemyFourLevelThreeDead = true;
            }
            if(aiCharacter.isDead && arenaManager.level04 == true && aiCharacter.aiCharacterStatsManager.isBoss)
            {
                arenaManager.isBossDead = true;
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
            yield return new WaitForSeconds(3f);
            if(aiCharacter.itemSpawner != null)
                Instantiate(aiCharacter.itemSpawner, transform);
            if(aiCharacter.isDead)
                Destroy(aiCharacter.gameObject);
        }

        public void PlayWeaponTrailFX()
        {
            aiCharacter.aiCharacterEffectsManager.PlayWeaponFX(false);
        }

        public override void OnAnimatorMove()
        {
            // Check if root motion is enabled
            if (aiCharacter.isAnimal == false)
            {
                // Use root motion to move the character
                Vector3 velocity = character.animator.deltaPosition;
                character.characterController.Move(velocity);

                if (aiCharacter.isRotatingWithRootMotion)
                {
                    character.transform.rotation *= character.animator.deltaRotation;
                }
            }
            else
            {
                return;           
            }
        }
    }
}

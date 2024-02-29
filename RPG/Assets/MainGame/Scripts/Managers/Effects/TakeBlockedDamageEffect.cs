using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace GI
{
    [CreateAssetMenu(menuName = "Character Effects/Take Blocked Damage")]
    public class TakeBlockedDamageEffect : CharacterEffect
    {
        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage; //If the damage is caused by a character, they are listed here

        [Header("Damage type")]
        public float physicalDamage = 0;
        public float fireDamage = 0;
        public float staminaDamage = 0;
        public float poiseDamage = 0;

        [Header("Animation")]
        public string blockAnimation;

        public override void ProcessEffect(CharacterManager character)
        {
            // If the character is dead, return without running any logic
            if (character.isDead)
                return;

            // If the character is invulnerable, no damage is taken
            if (character.isInvulnerable)
                return;

            CalculateDamage(character);
            ClaculateStaminaDamage(character);
            DecideBlockAnimationBasedOnPoiseDamage(character);
            PlayBlockSoundFX(character);
            AssignNewAITarget(character);

            if (character.isDead)
            {
                character.characterAnimatorManager.PlayTargetAnimation("Death_01", true);
            }
            else
            {
                if(character.characterStatsManager.currentStamina <= 0)
                {
                    character.characterAnimatorManager.PlayTargetAnimation("Break_Guard", true);
                    character.canBeRiposted = true;
                    //Play guard break sound
                    character.isBlocking = false;
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetAnimation(blockAnimation, true);
                    character.isAttacking = false;
                }
            }
        }

        private void CalculateDamage(CharacterManager character)
        {
            if(characterCausingDamage != null)
            {
                physicalDamage = Mathf.RoundToInt(physicalDamage * (characterCausingDamage.characterStatsManager.physicalDamagePercentageModifier / 100));
                fireDamage = Mathf.RoundToInt(fireDamage * (characterCausingDamage.characterStatsManager.fireDamagePercentageModifier / 100));
            }

            character.characterAnimatorManager.EraseHandIKForWeapon();

            float totalPhysicalDamageAbsorption = 1 -
                (1 - character.characterStatsManager.physicalDamageAbsorptionHead / 100) *
                (1 - character.characterStatsManager.physicalDamageAbsorptionBody / 100) *
                (1 - character.characterStatsManager.physicalDamageAbsorptionHands / 100) *
                (1 - character.characterStatsManager.physicalDamageAbsorptionLegs / 100);

            physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

            float totalFireDamageAbsorption = 1 -
                (1 - character.characterStatsManager.fireDamageAbsorptionHead / 100) *
                (1 - character.characterStatsManager.fireDamageAbsorptionBody / 100) *
                (1 - character.characterStatsManager.fireDamageAbsorptionHands / 100) *
                (1 - character.characterStatsManager.fireDamageAbsorptionLegs / 100);

            fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

            physicalDamage = physicalDamage - Mathf.RoundToInt(physicalDamage * (character.characterStatsManager.physicalAbsorptionPercentageModifier / 100));
            fireDamage = fireDamage - Mathf.RoundToInt(fireDamage * (character.characterStatsManager.fireAbsorptionPercentageModifier / 100));

            float finalDamage = physicalDamage + fireDamage; // + magicDamage etc.

            character.characterStatsManager.currentHealth = Mathf.RoundToInt(character.characterStatsManager.currentHealth - finalDamage);

            //Updates the player UI if it's damaged
            PlayerManager player = character as PlayerManager;
            if (player != null)
            {
                player.playerStatsManager.healthBar.SetCurrentHealth(player.playerStatsManager.currentHealth);
            }

            //Updates the aiCharacter's UI if it's damaged
            AICharacterManager aICharacter = character as AICharacterManager;
            if (aICharacter != null)
            {
                if (!aICharacter.aiCharacterStatsManager.isBoss)
                {
                    aICharacter.aiCharacterStatsManager.aiCharacterHealthBar.SetHealth(aICharacter.aiCharacterStatsManager.currentHealth);
                }
                else if (aICharacter.aiCharacterStatsManager.isBoss && aICharacter.aiCharacterBossManager != null)
                {
                    aICharacter.aiCharacterBossManager.UpdateBossHealthBar(aICharacter.aiCharacterStatsManager.currentHealth, aICharacter.aiCharacterStatsManager.maxHealth);
                }
            }

            if (character.characterStatsManager.currentHealth <= 0)
            {
                character.characterStatsManager.currentHealth = 0;
                character.isDead = true;
            }
        }

        private void ClaculateStaminaDamage(CharacterManager character)
        {
            float staminaDamageAbsorption = staminaDamage * (character.characterStatsManager.blockingStabilityRating / 100);
            float staminaDamageAfterAbsorption = staminaDamage - staminaDamageAbsorption;
            character.characterStatsManager.currentStamina -= staminaDamageAfterAbsorption;
        }

        private void DecideBlockAnimationBasedOnPoiseDamage(CharacterManager character)
        {
            // One handed block animation
            if(!character.isTwoHandingWeapon)
            {
                if (poiseDamage <= 24 && poiseDamage >= 0)
                {
                    blockAnimation = "OH_Block_Small_01";
                    return;
                }
                else if (poiseDamage <= 49 && poiseDamage >= 25)
                {
                    blockAnimation = "OH_Block_Medium_01";
                    return;
                }
                else if (poiseDamage <= 74 && poiseDamage >= 50)
                {
                    blockAnimation = "OH_Block_Heavy_01";
                    return;
                }
                else if (poiseDamage >= 100)
                {
                    blockAnimation = "OH_Block_Colossal_01";
                    return;
                }
            }
            // Two handed block animation
            else
            {
                if (poiseDamage <= 24 && poiseDamage >= 0)
                {
                    blockAnimation = "TH_Block_Small_01";
                    return;
                }
                else if (poiseDamage <= 49 && poiseDamage >= 25)
                {
                    blockAnimation = "TH_Block_Medium_01";
                    return;
                }
                else if (poiseDamage <= 74 && poiseDamage >= 50)
                {
                    blockAnimation = "TH_Block_Heavy_01";
                    return;
                }
                else if (poiseDamage >= 100)
                {
                    blockAnimation = "TH_Block_Colossal_01";
                    return;
                }
            }
        }

        private void PlayBlockSoundFX(CharacterManager character)
        {
            // We are blocking with our right handed weapon
            if (character.isTwoHandingWeapon)
            {
                character.characterSoundFXManager.PlayRandomSoundFXFromArray(character.characterInventoryManager.rightWeapon.blockingNoises);
            }
            // We are blocking with our left hand
            else
            {
                character.characterSoundFXManager.PlayRandomSoundFXFromArray(character.characterInventoryManager.leftWeapon.blockingNoises);
            }
        }

        private void AssignNewAITarget(CharacterManager character)
        {
            AICharacterManager aiCharacter = character as AICharacterManager;
            if (aiCharacter != null && characterCausingDamage != null)
            {
                aiCharacter.currentTarget = characterCausingDamage;
            }
        }
    }
}

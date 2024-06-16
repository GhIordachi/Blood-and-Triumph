using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace GI
{
    [CreateAssetMenu(menuName = "Character Effects/Take Damage")]
    public class TakeDamageEffect : CharacterEffect
    {        
        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage; //If the damage is caused by a character, they are listed here

        [Header("Damage type")]
        public float physicalDamage = 0;
        public float fireDamage = 0;
        public float magicDamage = 0;

        [Header("Poise")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;

        [Header("Animation")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("SFX")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSoundSFX; //Extra sfx that is played when there is elemental damage(fire, magic etc.)

        [Header("Direction Damage Taken From")]
        public float angleHitFrom;
        public Vector3 contactPoint; //Where the damage strikes the player on their body

        public override void ProcessEffect(CharacterManager character)
        {
            // If the character is dead, return without running any logic
            if(character.isDead) 
                return;

            // If the character is invulnerable, no damage is taken
            if (character.isInvulnerable)
                return;

            CalculateDamage(character);
            CheckWhichDirectionDamageCameFrom(character);
            PlayDamageAnimation(character);
            PlayDamageSoundFX(character);
            PlayBloodSplatter(character);
            AssignNewAITarget(character);
        }

        private void CalculateDamage(CharacterManager character)
        { 
            //Before calculating damage defense, we check the attacking character damage modifiers
            if(characterCausingDamage != null)
            {
                physicalDamage = Mathf.RoundToInt(physicalDamage * (characterCausingDamage.characterStatsManager.physicalDamagePercentageModifier / 100));
                fireDamage = Mathf.RoundToInt(fireDamage * (characterCausingDamage.characterStatsManager.fireDamagePercentageModifier / 100));
                magicDamage = Mathf.RoundToInt(magicDamage * (characterCausingDamage.characterStatsManager.magicDamagePercentageModifier / 100));
            }

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

            float totalMagicDamageAbsorption = 1 -
                (1 - character.characterStatsManager.magicDamageAbsorptionHead / 100) *
                (1 - character.characterStatsManager.magicDamageAbsorptionBody / 100) *
                (1 - character.characterStatsManager.magicDamageAbsorptionHands / 100) *
                (1 - character.characterStatsManager.magicDamageAbsorptionLegs / 100);

            magicDamage = Mathf.RoundToInt(magicDamage - (magicDamage * totalMagicDamageAbsorption));

            physicalDamage = physicalDamage - Mathf.RoundToInt(physicalDamage * (character.characterStatsManager.physicalAbsorptionPercentageModifier / 100));
            fireDamage = fireDamage - Mathf.RoundToInt(fireDamage * (character.characterStatsManager.fireAbsorptionPercentageModifier / 100));
            magicDamage = magicDamage - Mathf.RoundToInt(magicDamage * (character.characterStatsManager.magicAbsorptionPercentageModifier / 100));

            float finalDamage = physicalDamage + fireDamage + magicDamage; // + magicDamage etc.

            character.characterStatsManager.currentHealth = Mathf.RoundToInt(character.characterStatsManager.currentHealth - finalDamage);

            //Updates the player UI if it's damaged
            PlayerManager player = character as PlayerManager;
            if (player != null)
            {
                player.playerStatsManager.healthBar.SetCurrentHealth(player.playerStatsManager.currentHealth);
            }

            //Updates the aiCharacter's UI if it's damaged
            AICharacterManager aICharacter = character as AICharacterManager;
            if(aICharacter != null)
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

            if (character.characterStatsManager.totalPoiseDefence < poiseDamage)
            {
                poiseIsBroken = true;
            }

            Debug.Log("Total damage dealt is " + finalDamage);

            if (character.characterStatsManager.currentHealth <= 0)
            {
                character.characterStatsManager.currentHealth = 0;
                character.isDead = true;
            }
        }

        private void CheckWhichDirectionDamageCameFrom(CharacterManager character)
        {
            if (manuallySelectDamageAnimation)
                return;

            if (angleHitFrom >= 145 && angleHitFrom <= 180)
            {
                ChooseDamageAnimationForward(character);
            }
            else if (angleHitFrom <= -145 && angleHitFrom >= -180)
            {
                ChooseDamageAnimationForward(character);
            }
            else if (angleHitFrom >= -45 && angleHitFrom <= 45)
            {
                ChooseDamageAnimationBackward(character);
            }
            else if (angleHitFrom >= -144 && angleHitFrom <= -45)
            {
                ChooseDamageAnimationLeft(character);
            }
            else if (angleHitFrom >= 45 && angleHitFrom <= 144)
            {
                ChooseDamageAnimationRight(character);
            }
        }

        private void ChooseDamageAnimationForward(CharacterManager character)
        {
            // Poise bracket < 25 small
            // Poise bracket > 25 <50 medium
            // Poise bracket > 50 <75 large
            // Poise bracket > 75 <100 colosall
            Debug.Log("Forward");
            if(poiseDamage <=24 && poiseDamage >= 0)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Medium_Forward);
                return;
            }
            else if(poiseDamage <= 49 && poiseDamage >= 25)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Medium_Forward);
                return;
            }
            else if (poiseDamage <= 74 && poiseDamage >= 50)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Heavy_Forward);
                return;
            }
            else if (poiseDamage >= 100)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Colossal_Forward);
                return;
            }
        }

        private void ChooseDamageAnimationBackward(CharacterManager character)
        {
            Debug.Log("Backward");
            if (poiseDamage <= 24 && poiseDamage >= 0)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Medium_Backward);
                return;
            }
            else if (poiseDamage <= 49 && poiseDamage >= 25)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Medium_Backward);
                return;
            }
            else if (poiseDamage <= 74 && poiseDamage >= 50)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Heavy_Backward);
                return;
            }
            else if (poiseDamage >= 100)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Colossal_Backward);
                return;
            }
        }

        private void ChooseDamageAnimationLeft(CharacterManager character)
        {
            Debug.Log("Left");
            if (poiseDamage <= 24 && poiseDamage >= 0)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Medium_Left);
                return;
            }
            else if (poiseDamage <= 49 && poiseDamage >= 25)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Medium_Left);
                return;
            }
            else if (poiseDamage <= 74 && poiseDamage >= 50)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Heavy_Left);
                return;
            }
            else if (poiseDamage >= 100)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Colossal_Left);
                return;
            }
        }

        private void ChooseDamageAnimationRight(CharacterManager character)
        {
            Debug.Log("Right");
            if (poiseDamage <= 24 && poiseDamage >= 0)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Medium_Right);
                return;
            }
            else if (poiseDamage <= 49 && poiseDamage >= 25)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Medium_Right);
                return;
            }
            else if (poiseDamage <= 74 && poiseDamage >= 50)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Heavy_Right);
                return;
            }
            else if (poiseDamage >= 100)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Colossal_Right);
                return;
            }
        }

        private void PlayDamageSoundFX(CharacterManager character)
        {
            character.characterSoundFXManager.PlayRandomDamageSoundFX();

            if(fireDamage > 0)
            {
                character.characterSoundFXManager.PlaySoundFX(elementalDamageSoundSFX);
            }
        }

        private void PlayDamageAnimation(CharacterManager character)
        {
            // If we are currently playing a damage animation that is heavy and a light attack hits us
            // we do not want to play the light damage animation, we want to finish the heavy attack
            if(character.isInteracting && character.characterCombatManager.previousPoiseDamageTaken > poiseDamage)
            {
                // If the character is interacting && the previous poise damage is above 0, they must be in a damage animation
                // If the previous poise is above the current poise, return
                return;
            }

            if(character.isDead)
            {
                character.characterWeaponSlotManager.CloseDamageCollider();
                character.characterAnimatorManager.PlayTargetAnimation("Death_01", true);
                return;
            }

            // If the characters poise is not broken, no damage animation is played
            if(!poiseIsBroken)
            {
                return;
            }
            else
            {
                //Enable/Disable stun lock
                if (playDamageAnimation)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(damageAnimation, true);
                }
            }
        }

        private void PlayBloodSplatter(CharacterManager character)
        {
            character.characterEffectsManager.PlayBloodSplatterFX(contactPoint);
        }

        private void AssignNewAITarget(CharacterManager character)
        {
            AICharacterManager aiCharacter = character as AICharacterManager;
            if(aiCharacter != null && characterCausingDamage != null)
            {
                aiCharacter.currentTarget = characterCausingDamage;
            }
        }
    }
}

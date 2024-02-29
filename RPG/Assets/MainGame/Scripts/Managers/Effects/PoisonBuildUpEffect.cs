using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [CreateAssetMenu(menuName = "Character Effects/Poison Build Up")]
    public class PoisonBuildUpEffect : CharacterEffect
    {
        // The amount of poison build up given before resistances are calculated, per game tick
        [SerializeField] float basePoisonBuildUpAmount = 7;
        // The amount of poison time the character receives if they are poisoned, per game tick
        [SerializeField] float poisonAmount = 100;
        // The amount of damage taken from the poison per tick if it is built up to 100%
        [SerializeField] int poisonDamagePerTick = 5;

        public override void ProcessEffect(CharacterManager character)
        {
            PlayerManager player = character as PlayerManager;
            // Poison build up after we factor our player's resistances
            float finalPoisonBuildUp = 0;

            if (character.characterStatsManager.poisonResistance > 0)
            {
                if(character.characterStatsManager.poisonResistance >= 100)
                {
                    finalPoisonBuildUp = 0;
                }
                else
                {
                    float resistancePercentage = character.characterStatsManager.poisonResistance / 100;

                    finalPoisonBuildUp = basePoisonBuildUpAmount - (basePoisonBuildUpAmount * resistancePercentage);
                }
            }

            //Each tick we add the build up amount to the character overall build up
            character.characterStatsManager.poisonBuildup += finalPoisonBuildUp;

            // If the character is already poisoned, remove all poison build up effects
            if(character.characterStatsManager.isPoisoned)
            {
                character.characterEffectsManager.timedEffects.Remove(this);
            }

            //If our build up is 100 or more, poison the character
            if(character.characterStatsManager.poisonBuildup >= 100)
            {
                character.characterStatsManager.isPoisoned = true;
                character.characterStatsManager.poisonAmount = poisonAmount;
                character.characterStatsManager.poisonBuildup = 0;

                if(player != null)
                {
                    player.playerEffectsManager.poisonAmountBar.SetPoisonAmount(Mathf.RoundToInt(poisonAmount));
                }

                // We always want to instantiate a copy of a scriptable, so the original is never edited
                // If the original is edited, and every character uses an original, they will all share the same values
                PoisonedEffect poisonedEffect = Instantiate(WorldCharacterEffectsManager.instance.poisonedEffect);
                poisonedEffect.poisonDamage = poisonDamagePerTick;
                character.characterEffectsManager.timedEffects.Add(poisonedEffect);

                character.characterEffectsManager.timedEffects.Remove(this);

                character.characterEffectsManager.AddTimedEffectParticle(Instantiate(WorldCharacterEffectsManager.instance.poisonFX));
            }
            character.characterEffectsManager.timedEffects.Remove(this);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [CreateAssetMenu(menuName = "Spells/Healing Spell")]
    public class HealingSpell : SpellItem
    {
        public int healAmount;

        public override void AttemptToCastSpell(CharacterManager character)
        {
            base.AttemptToCastSpell(character);
            GameObject instatiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, character.transform);
            character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true, false, character.isUsingLeftHand);
        }

        public override void SuccessfullyCastSpell(CharacterManager character)
        {
            base.SuccessfullyCastSpell(character);
            GameObject instantiatedSpellFX = Instantiate(spellCastFX, character.transform);
            character.characterStatsManager.HealCharacter(healAmount);
        }
    }
}

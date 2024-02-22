using RPGCharacterAnims;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [CreateAssetMenu(menuName = "Item Actions/Magic Spell Action")]
    public class MagicSpellAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting)
                return;

            if (character.characterInventoryManager.currentSpell != null && character.characterInventoryManager.currentSpell.isMagicSpell)
            {
                if (character.characterStatsManager.currentFocusPoints >= character.characterInventoryManager.currentSpell.focusPointCost)
                {
                    character.characterInventoryManager.currentSpell.AttemptToCastSpell(character);
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetAnimation("No", true);
                }
            }
        }
    }
}

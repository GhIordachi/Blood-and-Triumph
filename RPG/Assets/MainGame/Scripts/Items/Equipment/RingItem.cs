using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [CreateAssetMenu(menuName = "Items/Ring")]
    public class RingItem : Item
    {
        [SerializeField] StaticCharacterEffect effect;
        private StaticCharacterEffect effectClone;

        [Header("Item Effect Description")]
        [TextArea] public string itemEffectInformation;

        //Called when equipping a ring, adds the ring's effect to our character
        public void EquipRing(CharacterManager character)
        {
            //We create a clone so the base scriptable object is not effected
            effectClone = Instantiate(effect);

            character.characterEffectsManager.AddStaticEffect(effectClone);
        }

        //Called when unequipping a ring, removes effect from our character
        public void UnEquipRing(CharacterManager character)
        {
            character.characterEffectsManager.RemoveStaticEffect(effect.effectID);
        }
    }
}

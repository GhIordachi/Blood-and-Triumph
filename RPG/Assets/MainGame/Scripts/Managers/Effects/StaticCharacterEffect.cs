using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class StaticCharacterEffect : ScriptableObject
    {
        public int effectID;

        //Static effects are used to add an effect to a player when equipping an item, and removing the effect after the item has been removed
        public virtual void AddStaticEffect(CharacterManager character)
        {

        }

        public virtual void RemoveStaticEffect(CharacterManager character)
        {

        }
    }
}

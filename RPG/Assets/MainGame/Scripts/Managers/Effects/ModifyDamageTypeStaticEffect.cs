using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [CreateAssetMenu(menuName = "Character Effects/Static Effects/Modify Damage Types")]
    public class ModifyDamageTypeStaticEffect : StaticCharacterEffect
    {
        [Header("Damage Type Effected")]
        [SerializeField] DamageType damageType;
        [SerializeField] int modifiedValue = 0;

        //When adding the effect we add the modified value amount to our respective damage type modifier
        public override void AddStaticEffect(CharacterManager character)
        {
            base.AddStaticEffect(character);

            switch (damageType)
            {
                case DamageType.Physical: 
                    character.characterStatsManager.physicalDamagePercentageModifier += modifiedValue;
                    break;
                case DamageType.Fire:
                    character.characterStatsManager.fireDamagePercentageModifier += modifiedValue;
                    break;
                case DamageType.Magic:
                    character.characterStatsManager.magicDamagePercentageModifier += modifiedValue;
                    break;
                default:
                    break;
            }
        }

        //When removing the effect, we subtract the amount we added
        public override void RemoveStaticEffect(CharacterManager character)
        {
            base.RemoveStaticEffect(character);

            switch (damageType)
            {
                case DamageType.Physical:
                    character.characterStatsManager.physicalDamagePercentageModifier -= modifiedValue;
                    break;
                case DamageType.Fire:
                    character.characterStatsManager.fireDamagePercentageModifier -= modifiedValue;
                    break;
                case DamageType.Magic:
                    character.characterStatsManager.magicDamagePercentageModifier -= modifiedValue;
                    break;
                default:
                    break;
            }
        }
    }
}

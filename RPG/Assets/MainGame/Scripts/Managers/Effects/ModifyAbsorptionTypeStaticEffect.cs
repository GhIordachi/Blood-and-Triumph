using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [CreateAssetMenu(menuName = "Character Effects/Static Effects/Modify Absorption Types")]
    public class ModifyAbsorptionTypeStaticEffect : StaticCharacterEffect
    {
        [Header("Damage Type Effected")]
        [SerializeField] AbsorptionType absorptionType;
        [SerializeField] int modifiedValue = 0;

        public override void AddStaticEffect(CharacterManager character)
        {
            base.AddStaticEffect(character);

            switch (absorptionType)
            {
                case AbsorptionType.physicalAbsorption:
                    character.characterStatsManager.physicalAbsorptionPercentageModifier += modifiedValue;
                    break;
                case AbsorptionType.fireAbsorption:
                    character.characterStatsManager.fireAbsorptionPercentageModifier += modifiedValue;
                    break;
                case AbsorptionType.magicAbsorption:
                    character.characterStatsManager.magicAbsorptionPercentageModifier += modifiedValue;
                    break;
                default:
                    break;
            }
        }

        public override void RemoveStaticEffect(CharacterManager character)
        {
            base.RemoveStaticEffect(character);

            switch (absorptionType)
            {
                case AbsorptionType.physicalAbsorption:
                    character.characterStatsManager.physicalAbsorptionPercentageModifier -= modifiedValue;
                    break;
                case AbsorptionType.fireAbsorption:
                    character.characterStatsManager.fireAbsorptionPercentageModifier -= modifiedValue;
                    break;
                case AbsorptionType.magicAbsorption:
                    character.characterStatsManager.magicAbsorptionPercentageModifier -= modifiedValue;
                    break;
                default:
                    break;
            }
        }
    }
}

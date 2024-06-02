using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [CreateAssetMenu(menuName = "Item Actions/Charge Attack Action")]
    public class ChargeAttackAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.characterStatsManager.currentStamina <= 0)
                return;

            character.isAttacking = true;
            character.characterEffectsManager.PlayWeaponFX(false);

            if (character.canDoCombo)
            {
                HandleChargeWeaponCombo(character);
            }
            else
            {
                if (character.isInteracting)
                    return;
                if (character.canDoCombo)
                    return;

                HandleChargeAttack(character);
            }
        }

        void HandleChargeAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_charge_attack_01, true, false, true);
                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charge_attack_01;
            }
            else if (character.isUsingRightHand)
            {
                if (character.isTwoHandingWeapon)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_charge_attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.th_charge_attack_01;
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_charge_attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charge_attack_01;
                }
            }
        }

        void HandleChargeWeaponCombo(CharacterManager character)
        {
            if (character.canDoCombo)
            {
                character.animator.SetBool("canDoCombo", false);

                if (character.isUsingLeftHand)
                {
                    if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_charge_attack_01)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_charge_attack_02, true, false, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charge_attack_02;
                    }
                    else
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_charge_attack_01, true, false, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charge_attack_01;
                    }
                }
                else if (character.isUsingRightHand)
                {
                    if (character.isTwoHandingWeapon)
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_charge_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_charge_attack_02, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_charge_attack_02;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_charge_attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_charge_attack_01;
                        }
                    }
                    else
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_charge_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_charge_attack_02, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charge_attack_02;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_charge_attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charge_attack_01;
                        }
                    }
                }
            }
        }
    }
}

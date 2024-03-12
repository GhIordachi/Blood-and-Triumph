using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class AttackStateHumanoid : State
    {
        public CombatStanceStateHumanoid combatStanceState;
        public PursueTargetStateHumanoid pursueTargetState;
        public RotateTowardsTargetStateHumanoid rotateTowardsTargetState;
        public ItemBasedAttackAction currentAttack;

        bool willDoComboNextAttack = false;
        public bool hasPerformedAttack = false;

        private void Awake()
        {
            rotateTowardsTargetState = GetComponent<RotateTowardsTargetStateHumanoid>();
            combatStanceState = GetComponent<CombatStanceStateHumanoid>();
            pursueTargetState = GetComponent <PursueTargetStateHumanoid>();
        }

        public override State Tick(AICharacterManager enemy)
        {
            if(enemy.combatStyle == AICombatStyle.swordAndShield)
            {
                return ProcessSwordAndShieldCombatStyle(enemy);
            }
            else if (enemy.combatStyle == AICombatStyle.archer)
            {
                return ProcessArcherCombatStyle(enemy);
            }
            else
            {
                return this;
            }
        }

        private State ProcessSwordAndShieldCombatStyle(AICharacterManager enemy)
        {
            RotateTowardsTargetWhilstAttacking(enemy);

            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if (willDoComboNextAttack && enemy.canDoCombo)
            {
                AttackTargetWithCombo(enemy);
            }

            if (!hasPerformedAttack)
            {
                AttackTarget(enemy);
                RollForComboChance(enemy);
            }

            if (willDoComboNextAttack && hasPerformedAttack)
            {
                return this;
            }

            ResetStateFlags();
            return rotateTowardsTargetState;
        }

        private State ProcessArcherCombatStyle(AICharacterManager enemy)
        {
            RotateTowardsTargetWhilstAttacking(enemy);

            if (enemy.isInteracting)
                return this;

            if(!enemy.isHoldingArrow)
            {
                ResetStateFlags();
                return combatStanceState;
            }

            if(enemy.currentTarget.isDead)
            {
                ResetStateFlags();
                enemy.currentTarget = null;
                return this;
            }

            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                ResetStateFlags();
                return pursueTargetState;
            }

            if (!hasPerformedAttack && enemy.isHoldingArrow)
            {
                FireAmmo(enemy);
            }

            ResetStateFlags();
            return rotateTowardsTargetState;
        }

        private void AttackTarget(AICharacterManager enemy)
        {
            currentAttack.PerformAttackAction(enemy);
            enemy.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformedAttack = true;
        }

        private void AttackTargetWithCombo(AICharacterManager enemy)
        {
            currentAttack.PerformAttackAction(enemy);
            willDoComboNextAttack = false;
            enemy.currentRecoveryTime = currentAttack.recoveryTime;
            currentAttack = null;
        }

        private void RotateTowardsTargetWhilstAttacking(AICharacterManager enemy)
        {
            //Rotate manually
            if (enemy.canRotate && enemy.isInteracting)
            {
                Vector3 direction = enemy.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemy.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemy.rotationSpeed / Time.deltaTime);
            }

        }

        private void RollForComboChance(AICharacterManager enemy)
        {
            float comboChance = Random.Range(0, 100);

            if (enemy.allowAIToPerformCombos && comboChance <= enemy.comboLikelyHood)
            {
                if (currentAttack.actionCanCombo)
                {
                    willDoComboNextAttack = true;
                }
                else
                {
                    willDoComboNextAttack = false;
                    currentAttack = null;
                }
            }
        }

        private void FireAmmo(AICharacterManager enemy)
        {
            if(enemy.isHoldingArrow)
            {
                hasPerformedAttack = true;
                enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.rightWeapon;
                enemy.characterInventoryManager.rightWeapon.th_tap_Left_Click.PerformAction(enemy);
            }
        }

        private void ResetStateFlags()
        {
            willDoComboNextAttack = false;
            hasPerformedAttack = false;
        }
    }
}
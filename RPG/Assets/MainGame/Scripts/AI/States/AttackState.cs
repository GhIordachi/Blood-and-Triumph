using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI {
    public class AttackState : State
    {
        public CombatStanceState combatStanceState;
        public PursueTargetState pursueTargetState;
        public RotateTowardsTargetState rotateTowardsTargetState;
        public EnemyAttackAction currentAttack;

        bool willDoComboNextAttack = false;
        public bool hasPerformedAttack = false;

        public override State Tick(EnemyManager enemy)
        {
            float distanceFromTarget = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);
            RotateTowardsTargetWhilstAttacking(enemy);

            if(distanceFromTarget > enemy.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if(willDoComboNextAttack && enemy.canDoCombo)
            {
                AttackTargetWithCombo(enemy);
            }

            if (!hasPerformedAttack)
            {
                AttackTarget(enemy);
                RollForComboChance(enemy);
            }

            if(willDoComboNextAttack && hasPerformedAttack)
            {
                return this;
            }

            return rotateTowardsTargetState;
        }

        private void AttackTarget(EnemyManager enemy)
        {
            enemy.animator.SetBool("isUsingRightHand", currentAttack.isRightHandedAction);
            enemy.animator.SetBool("isUsingLeftHand", !currentAttack.isRightHandedAction);
            enemy.enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemy.enemyAnimatorManager.PlayWeaponTrailFX();
            enemy.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformedAttack = true;
        }

        private void AttackTargetWithCombo(EnemyManager enemy)
        {
            enemy.animator.SetBool("isUsingRightHand", currentAttack.isRightHandedAction);
            enemy.animator.SetBool("isUsingLeftHand", !currentAttack.isRightHandedAction);
            willDoComboNextAttack = false;
            enemy.enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemy.enemyAnimatorManager.PlayWeaponTrailFX();
            enemy.currentRecoveryTime = currentAttack.recoveryTime;
            currentAttack = null;
        }

        private void RotateTowardsTargetWhilstAttacking(EnemyManager enemy)
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

        private void RollForComboChance(EnemyManager enemy)
        {
            float comboChance = Random.Range(0, 100);

            if(enemy.allowAIToPerformCombos && comboChance <= enemy.comboLikelyHood)
            {
                if(currentAttack.comboAction != null)
                {
                    willDoComboNextAttack = true;
                    currentAttack = currentAttack.comboAction;
                }
                else
                {
                    willDoComboNextAttack = false;
                    currentAttack = null;
                }
            }
        }
    }
}

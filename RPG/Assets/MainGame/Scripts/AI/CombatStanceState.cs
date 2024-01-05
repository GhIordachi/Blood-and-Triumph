using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public PursueTargetState pursueTargetState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            //Check for attack range
            //Potentially circle the player or walk around them
            //If in attack range return attack State
            //If we in a cooldown after attacking, return this state and continue circling player
            //if the player runs out of range, return to the pursue target state
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            if (enemyManager.isPerformingAction)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            }

            if(enemyManager.currentRecoveryTime <=0 && distanceFromTarget <= enemyManager.maximumAttackRange)
            {
                return attackState;
            }
            else if(distanceFromTarget > enemyManager.maximumAttackRange)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }

            return this;
        }
    }
}

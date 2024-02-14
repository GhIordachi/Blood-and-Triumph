using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GI
{
    public class PursueTargetState : State
    {
        public CombatStanceState combatStanceState;
        public RotateTowardsTargetState rotateTowardsTargetState;
        public override State Tick(EnemyManager enemy)
        {
            Vector3 targetDirection = enemy.currentTarget.transform.position - enemy.transform.position;
            float distanceFromTarget = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);
            float viewableAngle = Vector3.SignedAngle(targetDirection, enemy.transform.forward, Vector3.up);

            HandleRotateTowardsTarget(enemy);

            if (enemy.isInteracting)
                return this;

            if (enemy.isPerformingAction)
            {
                enemy.animator.SetFloat("Vertical", 0, 0.1f,Time.deltaTime);
                return this;
            }

            if (distanceFromTarget > enemy.maximumAggroRadius)
            {
                enemy.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            if(distanceFromTarget <= enemy.maximumAggroRadius)
            {
                return combatStanceState;
            }
            else
            {
                return this;
            }
        }

        private void HandleRotateTowardsTarget(EnemyManager enemy)
        {
            //Rotate manually
            if (enemy.isPerformingAction)
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
            //Rotate with pathfinding (navmesh)
            else
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(enemy.navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemy.enemyRigidBody.velocity;

                enemy.navMeshAgent.enabled = true;
                enemy.navMeshAgent.SetDestination(enemy.currentTarget.transform.position);
                enemy.enemyRigidBody.velocity = targetVelocity;
                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, enemy.navMeshAgent.transform.rotation, enemy.rotationSpeed / Time.deltaTime);
            }
        }
    }
}

using UnityEngine;

namespace GI
{
    public class CompanionStatePursueTarget : State
    {
        CompanionStateCombatStance combatStanceStateHumanoid;
        CompanionStateFollowHost companionStateFollowHost;

        private void Awake()
        {
            combatStanceStateHumanoid = GetComponent<CompanionStateCombatStance>();
            companionStateFollowHost = GetComponent<CompanionStateFollowHost>();
        }

        public override State Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.distanceFromCompanion > aiCharacter.maxDistanceFromCompanion)
            {
                return companionStateFollowHost;
            }

            if (aiCharacter.combatStyle == AICombatStyle.swordAndShield)
            {
                return ProcessSwordAndShieldPursueStyle(aiCharacter);
            }
            else if (aiCharacter.combatStyle == AICombatStyle.archer)
            {
                return ProcessArcherPursueStyle(aiCharacter);
            }
            else
            {
                return this;
            }
        }

        private State ProcessArcherPursueStyle(AICharacterManager aiCharacter)
        {
            HandleRotateTowardsTarget(aiCharacter);

            if (aiCharacter.isInteracting)
                return this;

            if (aiCharacter.isPerformingAction)
            {
                aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            if (aiCharacter.distanceFromTarget > aiCharacter.maximumAggroRadius)
            {
                if (!aiCharacter.isStationaryArcher)
                {
                    aiCharacter.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
                }
            }

            if (aiCharacter.distanceFromTarget <= aiCharacter.maximumAggroRadius)
            {
                return combatStanceStateHumanoid;
            }
            else
            {
                return this;
            }
        }

        private State ProcessSwordAndShieldPursueStyle(AICharacterManager aiCharacter)
        {
            HandleRotateTowardsTarget(aiCharacter);

            if (aiCharacter.isInteracting)
                return this;

            if (aiCharacter.isPerformingAction)
            {
                aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            if (aiCharacter.distanceFromTarget > aiCharacter.maximumAggroRadius)
            {
                aiCharacter.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            if (aiCharacter.distanceFromTarget <= aiCharacter.maximumAggroRadius)
            {
                return combatStanceStateHumanoid;
            }
            else
            {
                return this;
            }
        }

        private void HandleRotateTowardsTarget(AICharacterManager aiCharacter)
        {
            //Rotate manually
            if (aiCharacter.isPerformingAction)
            {
                Vector3 direction = aiCharacter.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                aiCharacter.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aiCharacter.rotationSpeed / Time.deltaTime);
            }
            //Rotate with pathfinding (navmesh)
            else
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(aiCharacter.navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = aiCharacter.aiCharacterRigidBody.velocity;

                aiCharacter.navMeshAgent.enabled = true;
                aiCharacter.navMeshAgent.SetDestination(aiCharacter.currentTarget.transform.position);
                aiCharacter.aiCharacterRigidBody.velocity = targetVelocity;
                aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, aiCharacter.navMeshAgent.transform.rotation, aiCharacter.rotationSpeed / Time.deltaTime);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class CompanionStateFollowHost : State
    {
        CompanionStateIdle companionStateIdle;

        private void Awake()
        {
            companionStateIdle = GetComponent<CompanionStateIdle>();
        }

        public override State Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isInteracting)
                return this;

            if (aiCharacter.isPerformingAction)
            {
                aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            HandleRotateTowardsTarget(aiCharacter);

            if (aiCharacter.distanceFromCompanion > aiCharacter.maxDistanceFromCompanion)
            {
                aiCharacter.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            if (aiCharacter.distanceFromCompanion <= aiCharacter.returnDistanceFromCompanion)
            {
                return companionStateIdle;
            }
            else
            {
                return this;
            }
        }

        private void HandleRotateTowardsTarget(AICharacterManager aiCharacter)
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(aiCharacter.navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = aiCharacter.aiCharacterRigidBody.velocity;

            aiCharacter.navMeshAgent.enabled = true;
            aiCharacter.navMeshAgent.SetDestination(aiCharacter.companion.transform.position);
            aiCharacter.aiCharacterRigidBody.velocity = targetVelocity;
            aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, aiCharacter.navMeshAgent.transform.rotation, aiCharacter.rotationSpeed / Time.deltaTime);
        }
    }
}

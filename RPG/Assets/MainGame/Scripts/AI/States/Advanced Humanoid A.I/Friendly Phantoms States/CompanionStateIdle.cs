using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class CompanionStateIdle : State
    {
        CompanionStatePursueTarget companionStatePursueTarget;
        CompanionStateFollowHost followHostState;

        public LayerMask detectionLayer;
        public LayerMask layersThatBlockLineOfSight;

        private void Awake()
        {
            followHostState = GetComponent<CompanionStateFollowHost>();
            companionStatePursueTarget = GetComponent<CompanionStatePursueTarget>();
        }

        public override State Tick(AICharacterManager aiCharacter)
        {
            aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);

            if(aiCharacter.distanceFromCompanion > aiCharacter.maxDistanceFromCompanion)
            {
                return followHostState;
            }

            //Searches for a potential target within the detection radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, aiCharacter.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                //if a potential target is found, that is not on the same team as the A.I we proceed to the next step
                if (targetCharacter != null)
                {
                    if (targetCharacter.characterStatsManager.teamIDNumber != aiCharacter.aiCharacterStatsManager.teamIDNumber)
                    {
                        Vector3 targetDetection = targetCharacter.transform.position - transform.position;
                        float viewableAngle = Vector3.Angle(targetDetection, transform.forward);

                        //if a potential target is found, it has to be standing infront of the A.I;s field of view
                        if (viewableAngle > aiCharacter.minimumDetectionAngle && viewableAngle < aiCharacter.maximumDetectionAngle)
                        {
                            //If the A.I's potential target has an obstacle in between of itself and the A.I, we do not add it as our current target
                            if (Physics.Linecast(aiCharacter.lockOnTransform.position, targetCharacter.lockOnTransform.position, layersThatBlockLineOfSight))
                            {
                                return this;
                            }
                            else
                            {
                                aiCharacter.currentTarget = targetCharacter;
                            }
                        }
                    }
                }
            }

            if (aiCharacter.currentTarget != null)
            {
                return companionStatePursueTarget;
            }
            else
            {
                return this;
            }

            return this;
        }
    }
}

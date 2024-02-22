using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class IdleStateHumanoid : State
    {
        public PursueTargetStateHumanoid pursueTargetStateHumanoid;

        public LayerMask detectionLayer;
        public LayerMask layersThatBlockLineOfSight;

        private void Awake()
        {
            pursueTargetStateHumanoid = GetComponent<PursueTargetStateHumanoid>();
        }

        public override State Tick(EnemyManager aiCharacter)
        {
            //Searches for a potential target within the detection radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, aiCharacter.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                //if a potential target is found, that is not on the same team as the A.I we proceed to the next step
                if (targetCharacter != null)
                {
                    if (targetCharacter.characterStatsManager.teamIDNumber != aiCharacter.enemyStatsManager.teamIDNumber)
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
                return pursueTargetStateHumanoid;
            }
            else
            {
                return this;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI {
    public class IdleState : State
    {
        public PursueTargetState pursueTargetState;

        public LayerMask detectionLayer;
        public LayerMask layersToIgnoreForLineOfSight;

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
                            if (Physics.Linecast(aiCharacter.lockOnTransform.position, targetCharacter.lockOnTransform.position, layersToIgnoreForLineOfSight))
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
                return pursueTargetState;
            }
            else
            {
                return this;
            }
        }
    }
}

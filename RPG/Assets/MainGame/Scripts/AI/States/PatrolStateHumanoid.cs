using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace GI
{
    public class PatrolStateHumanoid : State
    {
        [SerializeField]
        PursueTargetStateHumanoid pursueTargetState;

        [SerializeField] LayerMask detectionLayer;
        [SerializeField] LayerMask layersThatBlockLineOfSight;

        [SerializeField] bool patrolComplete;
        [SerializeField] bool repeatPatrol;

        //How long before next patrol
        [Header("Patrol Reset Time")]
        [SerializeField] float endOfPatrolRestTime;
        [SerializeField] float endOfPatrolTimer;

        [Header("Patrol Position")]
        public int patrolDestinationIndex;
        public bool hasPatrolDestination;
        public Transform currentPatrolDestination;
        public float distanceFromCurrentPatrolPoint;
        public List<Transform> listOfPatrolDestinations = new List<Transform>();

        public override State Tick(AICharacterManager aiCharacter)
        {
            SearchForTargetWhilstPatrolling(aiCharacter);

            if(aiCharacter.isInteracting)
            {
                aiCharacter.animator.SetFloat("Vertical", 0);
                aiCharacter.animator.SetFloat("Horizontal", 0);
                return this;
            }

            if(aiCharacter.currentTarget != null)
            {
                return pursueTargetState;
            }

            //If we've completed our patrol and we want to repeat it
            if(patrolComplete && repeatPatrol)
            {
                //We count down our timer and reset all our flags
                if(endOfPatrolRestTime > endOfPatrolTimer)
                {
                    aiCharacter.animator.SetFloat("Vertical", 0f, 0.2f, Time.deltaTime);
                    endOfPatrolTimer = endOfPatrolTimer + Time.deltaTime;
                    return this;
                }
                else if(endOfPatrolTimer >= endOfPatrolRestTime)
                {
                    patrolDestinationIndex = -1;
                    hasPatrolDestination = false;
                    currentPatrolDestination = null;
                    patrolComplete = false;
                    endOfPatrolTimer = 0;
                }
            }
            else if(patrolComplete && !repeatPatrol)
            {
                aiCharacter.navMeshAgent.enabled = false;
                aiCharacter.animator.SetFloat("Vertical", 0f, 0.2f, Time.deltaTime);
                return this;
            }

            if(hasPatrolDestination)
            {
                if(currentPatrolDestination != null)
                {
                    distanceFromCurrentPatrolPoint = Vector3.Distance(aiCharacter.transform.position, currentPatrolDestination.transform.position);

                    if(distanceFromCurrentPatrolPoint > 1)
                    {
                        aiCharacter.navMeshAgent.enabled = true;
                        aiCharacter.navMeshAgent.destination = currentPatrolDestination.transform.position;
                        Quaternion targetRotation = Quaternion.Lerp(aiCharacter.transform.rotation, aiCharacter.navMeshAgent.transform.rotation, 0.5f);
                        aiCharacter.transform.rotation = targetRotation;
                        aiCharacter.animator.SetFloat("Vertical", 0.5f, 0.2f, Time.deltaTime);
                    }
                    else
                    {
                        currentPatrolDestination = null;
                        hasPatrolDestination = false;
                    }
                }
            }

            if (!hasPatrolDestination)
            {
                patrolDestinationIndex = patrolDestinationIndex + 1;

                if(patrolDestinationIndex > listOfPatrolDestinations.Count - 1)
                {
                    patrolComplete = true;
                    return this;
                }

                currentPatrolDestination = listOfPatrolDestinations[patrolDestinationIndex];
                hasPatrolDestination = true;
            }

            return this;
        }

        private void SearchForTargetWhilstPatrolling(AICharacterManager aiCharacter)
        {
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
                                return;
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
                return;
            }
            else
            {
                return;
            }
        }
    }
}

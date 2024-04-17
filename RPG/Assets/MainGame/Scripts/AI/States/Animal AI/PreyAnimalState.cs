using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace GI
{
    public class PreyAnimalState : AnimalStates
    {
        [Header("Prey Variables")]
        [SerializeField] float detectionRange = 10f;
        [SerializeField] float escapeMaxDistance = 80f;
        private bool isRunningFromPredator = false;

        public LayerMask detectionLayer;

        protected override void CheckChaseConditions()
        {
            Collider[] numColliders = new Collider[3];
            int colliders = Physics.OverlapSphereNonAlloc(transform.position, detectionRange, numColliders, detectionLayer);

            for (int i = 0; i < colliders; i++)
            {
                CharacterManager targetCharacter = numColliders[i].GetComponent<CharacterManager>();

                //if a potential target is found, that is not on the same team as the A.I we proceed to the next step
                if (targetCharacter != null)
                {
                    if (targetCharacter.characterStatsManager.teamIDNumber != animal.aiCharacterStatsManager.teamIDNumber)
                    {
                        animal.currentTarget = targetCharacter;
                    }
                }
            }

            if(animal.currentTarget)
                HandleChaseState();
        }

        protected override void HandleChaseState()
        {            
            if(animal.currentTarget != null && !isRunningFromPredator)
            {
                StopAllCoroutines();
                SetState(AnimalState.Chase);
                StartCoroutine(RunFromPredator());
            }
            else
            {
                animal.currentTarget = null;
            }
        }

        private IEnumerator RunFromPredator()
        {
            isRunningFromPredator = true;

            while (animal.currentTarget == null || Vector3.Distance(transform.position, animal.currentTarget.transform.position) > detectionRange)
            {
                yield return null;
            }

            while (animal.currentTarget != null && Vector3.Distance(transform.position, animal.currentTarget.transform.position) <= detectionRange)
            {
                RunAwayFromPredator();

                yield return null;
            }

            if(!navMeshAgent.pathPending && navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {                
                yield return null;
            }

            SetState(AnimalState.Idle);
            isRunningFromPredator = false;
        }

        private void RunAwayFromPredator()
        {
            if(navMeshAgent != null && navMeshAgent.isActiveAndEnabled)
            {
                if(!navMeshAgent.pathPending && navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                {
                    Vector3 runDirection = transform.position - animal.currentTarget.transform.position;
                    Vector3 escapeDestination = transform.position + runDirection.normalized * (escapeMaxDistance * 2);
                    navMeshAgent.SetDestination(GetRandomNavMeshPosition(escapeDestination, escapeMaxDistance));
                }
            }
        }

        protected override void Die()
        {
            StopAllCoroutines();
            base.Die();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
}

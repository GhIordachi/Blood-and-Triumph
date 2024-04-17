using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class PredatorAnimalState : AnimalStates
    {
        [Header("Predator Variables")]
        [SerializeField] float detectionRange = 15f;
        [SerializeField] float maxChaseTime = 10f;
        [SerializeField] int biteDamage = 3;
        [SerializeField] float biteCooldown = 1f;

        private PreyAnimalState currentChaseTarget;

        protected override void CheckChaseConditions()
        {
            if (currentChaseTarget)
                return;

            Collider[] colliders = new Collider[10];
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position,detectionRange, colliders);

            for(int i = 0; i < numColliders; i++)
            {
                PreyAnimalState prey = colliders[i].GetComponent<PreyAnimalState>();

                if(prey != null)
                {
                    StartChase(prey);
                    return;
                }
            }

            currentChaseTarget = null;
        }

        private void StartChase(PreyAnimalState prey)
        {
            currentChaseTarget = prey;
            SetState(AnimalState.Chase);
        }

        protected override void HandleChaseState()
        {
            if(currentChaseTarget != null)
            {
                //currentChaseTarget.AlertPrey(this);
                StartCoroutine(ChasePrey());
            }
            else
            {
                SetState(AnimalState.Idle);
            }
        }

        private IEnumerator ChasePrey()
        {
            float startTime = Time.deltaTime;

            while(currentChaseTarget != null && Vector3.Distance(transform.position, currentChaseTarget.transform.position) > navMeshAgent.stoppingDistance)
            {
                if(Time.deltaTime - startTime >= maxChaseTime || currentChaseTarget == null)
                {
                    StopChase();
                    yield break;
                }

                SetState(AnimalState.Chase);
                navMeshAgent.SetDestination(currentChaseTarget.transform.position);

                yield return null;
            }

            if (currentChaseTarget)
                currentChaseTarget.animal.aiCharacterStatsManager.TakeDamageNoAnimation(biteDamage,0,0);

            yield return new WaitForSeconds(biteCooldown);

            currentChaseTarget = null;
            HandleChaseState();

            CheckChaseConditions();
        }

        private void StopChase()
        {
            navMeshAgent.ResetPath();
            currentChaseTarget = null;
            SetState(AnimalState.Idle);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
}

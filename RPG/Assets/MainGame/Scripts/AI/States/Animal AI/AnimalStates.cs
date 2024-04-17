using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AnimalStates : MonoBehaviour
    {
        public AICharacterManager animal;

        [Header("Wander")]
        [SerializeField] float wanderDistance = 50f;
        [SerializeField] float walkSpeed = 5f;
        [SerializeField] float maxWalkTime = 6f;

        [Header("Idle")]
        [SerializeField] float idleTime = 5f;

        [Header("Chase")]
        [SerializeField] float runSpeed = 8f;

        protected NavMeshAgent navMeshAgent;
        [SerializeField] protected AnimalState currentState = AnimalState.Idle;

        private void Start()
        {
            InitialiseAnimal();
        }

        protected virtual void InitialiseAnimal()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.speed = walkSpeed;
            animal = GetComponent<AICharacterManager>();
            animal.navMeshAgent = navMeshAgent;

            currentState = AnimalState.Idle;
            UpdateState();
        }

        protected virtual void UpdateState()
        {
            if(animal.isDead) 
                return;

            switch(currentState)
            {
                case AnimalState.Idle:
                    HandleIdleState();
                    break;
                case AnimalState.Moving:
                    HandleMovingState();
                    break;
                case AnimalState.Chase:
                    HandleChaseState();
                    break;
            }
        }

        protected Vector3 GetRandomNavMeshPosition(Vector3 origin, float distance)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere * distance;
                randomDirection += origin;
                NavMeshHit navMeshHit;

                if (NavMesh.SamplePosition(randomDirection, out navMeshHit, distance, NavMesh.AllAreas))
                {
                    return navMeshHit.position;
                }
            }

            return origin;
        }

        protected virtual void CheckChaseConditions()
        {

        }

        protected virtual void HandleChaseState()
        {
            StopAllCoroutines();
        }

        protected virtual void HandleIdleState()
        {
            StartCoroutine(WaitToMove());
        }

        private IEnumerator WaitToMove()
        {
            CheckChaseConditions();
            float waitTime = Random.Range(idleTime / 2, idleTime * 2);
            yield return new WaitForSeconds(waitTime);

            Vector3 randomDestination = GetRandomNavMeshPosition(transform.position, wanderDistance);

            navMeshAgent.SetDestination(randomDestination);
            SetState(AnimalState.Moving);
        }

        protected virtual void HandleMovingState()
        {
            StartCoroutine(WaitToReachDestination());
        }

        private IEnumerator WaitToReachDestination()
        {
            float startTime = Time.deltaTime;

            while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && navMeshAgent.isActiveAndEnabled)
            {
                if (Time.deltaTime - startTime >= maxWalkTime) 
                {
                    navMeshAgent.ResetPath();
                    SetState(AnimalState.Idle);
                    yield break;
                }

                CheckChaseConditions();

                yield return null;
            }

            SetState(AnimalState.Idle);
        }

        protected void SetState(AnimalState newState)
        {
            if(currentState == newState) 
                return;

            currentState = newState;
            OnStateChange(newState);
        }

        protected virtual void OnStateChange(AnimalState newState)
        {
            if(newState == AnimalState.Moving)
            {
                navMeshAgent.speed = walkSpeed;
            }
            else if(newState == AnimalState.Chase)
            {
                navMeshAgent.speed = runSpeed;
            }

            UpdateState();            
        }

        protected virtual void Die()
        {
            StopAllCoroutines();
            animal.isDead = true;
        }
    }
}

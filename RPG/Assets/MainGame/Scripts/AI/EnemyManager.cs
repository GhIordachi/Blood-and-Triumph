using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;   

namespace GI {
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotionManager enemyLocomotionManager;
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyStats enemyStats;

        public State currentState;
        public CharacterStats currentTarget;
        public NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigidBody;

        public bool isPerformingAction;
        public bool isInteracting;
        public float rotationSpeed = 15;
        public float maximumAttackRange = 1.5f;

        [Header("AI Settings")]
        public float detectionRadius = 20;
        //The higher, and lower, respectively these angles are, the greater detection Field of view(eye sight of the enemy)
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;

        public float currentRecoveryTime = 0;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyStats = GetComponent<EnemyStats>();
            enemyRigidBody = GetComponent<Rigidbody>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            navMeshAgent.enabled = false;
        }

        private void Start()
        {
            enemyRigidBody.isKinematic = false;
        }

        private void Update()
        {
            HandleRecoveryTimer();

            isInteracting = enemyAnimatorManager.anim.GetBool("isInteracting");
            enemyAnimatorManager.anim.SetBool("isDead", enemyStats.isDead);
        }

        private void FixedUpdate()
        {
            HandleStateMachine();
        }

        private void HandleStateMachine()
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

                if (nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(State state)
        {
            currentState = state;
        }

        private void HandleRecoveryTimer()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
            }
        }
    }
}

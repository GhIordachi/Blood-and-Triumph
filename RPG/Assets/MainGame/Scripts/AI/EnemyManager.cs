using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;   

namespace GI {
    public class EnemyManager : CharacterManager
    {
        public EnemyBossManager enemyBossManager;
        public EnemyLocomotionManager enemyLocomotionManager;
        public EnemyAnimatorManager enemyAnimatorManager;
        public EnemyStatsManager enemyStatsManager;
        public EnemyEffectsManager enemyEffectsManager;
        public EnemyInventoryManager enemyInventoryManager;

        public State currentState;
        public CharacterManager currentTarget;
        public NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigidBody;

        public bool isPerformingAction;
        public float rotationSpeed = 15;
        public float maximumAggroRadius = 1.5f;

        [Header("AI Settings")]
        public float detectionRadius = 20;
        //The higher, and lower, respectively these angles are, the greater detection Field of view(eye sight of the enemy)
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
        public float currentRecoveryTime = 0;

        [Header("AI Combat Settings")]
        public bool allowAIToPerformCombos;
        public bool isPhaseShifting;
        public float comboLikelyHood;
        public AICombatStyle combatStyle;

        //These settings only affect A.I with advanced states
        [Header("Advanced A.I Settings")]
        public bool allowAIToPerformBlock;
        public int blockLikelyHood = 50;   //Number 0-100. 100 will generate a block every time, 0 will generate a block 0% of the time.
        public bool allowAIToPerformDodge;
        public int dodgeLikelyHood = 50;
        public bool allowAIToPerformParry;
        public int parryLikelyHood = 50;

        [Header("A.I Archery Settings")]
        public float minimumTimeToAimAtTarget = 3;
        public float maximumTimeToAimAtTarget = 6;

        [Header("A.I Target Information")]
        public float distanceFromTarget;
        public Vector3 targetsDirection;
        public float viewableAngle;

        protected override void Awake()
        {
            base.Awake();
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyBossManager = GetComponent<EnemyBossManager>();
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            enemyStatsManager = GetComponent<EnemyStatsManager>();
            enemyEffectsManager = GetComponent<EnemyEffectsManager>();
            enemyInventoryManager = GetComponent<EnemyInventoryManager>();
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
            HandleStateMachine();

            isRotatingWithRootMotion = animator.GetBool("isRotatingWithRootMotion");
            isInteracting = animator.GetBool("isInteracting");
            isInvulnerable = animator.GetBool("isInvulnerable");
            isHoldingArrow = animator.GetBool("isHoldingArrow");
            isPhaseShifting = animator.GetBool("isPhaseShifting");
            canDoCombo = animator.GetBool("canDoCombo");
            canRotate = animator.GetBool("canRotate");
            animator.SetBool("isDead", isDead);
            animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
            animator.SetBool("isBlocking", isBlocking);

            if(currentTarget != null)
            {
                distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
                targetsDirection = currentTarget.transform.position - transform.position;
                viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            enemyEffectsManager.HandleAllBuildUpEffects();
        }

        private void LateUpdate()
        {
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleStateMachine()
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this);

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;   

namespace GI {
    public class AICharacterManager : CharacterManager
    {
        public AICharacterBossManager aiCharacterBossManager;
        public AICharacterLocomotionManager aiCharacterLocomotionManager;
        public AICharacterAnimatorManager aiCharacterAnimatorManager;
        public AICharacterStatsManager aiCharacterStatsManager;
        public AICharacterEffectsManager aiCharacterEffectsManager;
        public AICharacterInventoryManager aiCharacterInventoryManager;

        public State currentState;
        public CharacterManager currentTarget;
        public NavMeshAgent navMeshAgent;
        public Rigidbody aiCharacterRigidBody;

        public bool isPerformingAction;
        public float rotationSpeed = 15;
        public float maximumAggroRadius = 1.5f;

        [Header("AI Drop Item")]
        public GameObject itemSpawner;
        public WeaponItem weaponItemToDrop;

        [Header("AI Settings")]
        public float detectionRadius = 20;
        //The higher, and lower, respectively these angles are, the greater detection Field of view(eye sight of the enemy)
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
        public float currentRecoveryTime = 0;
        public float stoppingDistance = 1.2f; //How close we get to our target before stopping infront of them

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
        public bool isStationaryArcher;
        public float minimumTimeToAimAtTarget = 3;
        public float maximumTimeToAimAtTarget = 6;

        [Header("A.I Companion Settings")]
        public float maxDistanceFromCompanion;  //Max distance we can go from our companion
        public float minDistanceFromCompanion;  //Min distance we have to be from our companion
        public float returnDistanceFromCompanion = 2f;  //How close we get to our companion when we return to them
        public float distanceFromCompanion;     
        public CharacterManager companion;

        [Header("A.I Target Information")]
        public float distanceFromTarget;
        public Vector3 targetsDirection;
        public float viewableAngle;

        [Header("Animal A.I Info")]
        public bool isAnimal = false;
        public float movementSpeed = 1;
        public PreyAnimalState preyAnimalState;

        protected override void Awake()
        {
            base.Awake();
            aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
            aiCharacterBossManager = GetComponent<AICharacterBossManager>();
            aiCharacterAnimatorManager = GetComponent<AICharacterAnimatorManager>();
            aiCharacterStatsManager = GetComponent<AICharacterStatsManager>();
            aiCharacterEffectsManager = GetComponent<AICharacterEffectsManager>();
            aiCharacterInventoryManager = GetComponent<AICharacterInventoryManager>();
            aiCharacterRigidBody = GetComponent<Rigidbody>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            if(!isAnimal)
                navMeshAgent.enabled = false;
        }

        protected override void Start()
        {
            base.Start();

            aiCharacterRigidBody.isKinematic = false;
        }

        protected override void Update()
        {
            base.Update();

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

            if(companion != null)
            {
                distanceFromCompanion = Vector3.Distance(companion.transform.position, transform.position);
            }
        }

        private void LateUpdate()
        {
            if (!isAnimal)
            {
                navMeshAgent.transform.localPosition = Vector3.zero;
                navMeshAgent.transform.localRotation = Quaternion.identity;
            }
        }

        private void HandleStateMachine()
        {
            if (isDead)
                return;
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

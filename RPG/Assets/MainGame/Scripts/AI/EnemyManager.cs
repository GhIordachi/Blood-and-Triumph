using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI {
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotionManager enemyLocomotionManager;
        public bool isPerformingAction;

        [Header("AI Settings")]
        public float detectionRadius = 20;
        //The higher, and lower, respectively these angles are, the greater detection Field of view(eye sight of the enemy)
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        }

        private void Update()
        {
            
        }

        private void FixedUpdate()
        {
            HandleCurrentAction();
        }

        private void HandleCurrentAction()
        {
            if(enemyLocomotionManager.currentTarget == null)
            {
                //Debug.Log("nu l-a gasit");
                enemyLocomotionManager.HandleDetection();
            }
            else
            {
                //Debug.Log("l-a gasit");
                enemyLocomotionManager.HandleMoveToTarget();
            }
        }
    }
}

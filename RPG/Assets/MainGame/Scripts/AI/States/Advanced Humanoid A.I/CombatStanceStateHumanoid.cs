using RPGCharacterAnims.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class CombatStanceStateHumanoid : State
    {
        public AttackStateHumanoid attackState;
        public ItemBasedAttackAction[] enemyAttacks;
        public PursueTargetStateHumanoid pursueTargetState;

        protected bool randomDestinationSet = false;
        protected float verticalMovementValue = 0;
        protected float horizontalMovementValue = 0;

        [Header("State Flags")]
        bool willPerformBlock = false;
        bool willPerformDodge = false;
        bool willPerformParry = false;

        bool hasPerformedDodge = false;
        bool hasPerformedParry = false;
        bool hasRandomDodgeDirection = false;
        bool hasAmmoLoaded = false;

        Quaternion targetDodgeDirection;

        private void Awake()
        {
            pursueTargetState = GetComponent<PursueTargetStateHumanoid>();
            attackState = GetComponent<AttackStateHumanoid>();
        }

        public override State Tick(EnemyManager enemy)
        {
            if(enemy.combatStyle == AICombatStyle.swordAndShield)
            {
                return ProcessSwordAndShieldCombatStyle(enemy);
            }
            else if(enemy.combatStyle == AICombatStyle.archer)
            {
                return ProcessArcherCombatStyle(enemy);
            }
            else
            {
                return this;
            }
        }

        private State ProcessSwordAndShieldCombatStyle(EnemyManager enemy)
        {
            enemy.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);

            //If the A.I is falling or is performing some sort of action Stop all movement
            if(!enemy.isGrounded || enemy.isInteracting)
            {
                enemy.animator.SetFloat("Vertical", 0);
                enemy.animator.SetFloat("Horizontal", 0);
                return this;
            }

            //If the A.I has gotten too far from it's target, return the A.I to it's pursue target state
            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            //Randomises the walking pattern of our A.I so they circle the player
            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                //Decide Circling Action
                DecideCirclingAction(enemy.enemyAnimatorManager);
            }

            if (enemy.allowAIToPerformParry)
            {
                if(enemy.currentTarget.canBeRiposted)
                {
                    CheckForRiposte(enemy);
                    return this;
                }
            }

            if (enemy.allowAIToPerformBlock)
            {
                RollForBlockChance(enemy);
            }

            if (enemy.allowAIToPerformDodge)
            {
                RollForDodgeChance(enemy);
            }

            if (enemy.allowAIToPerformParry)
            {
                RollForParryChance(enemy);
            }

            if (enemy.currentTarget.isAttacking)
            {
                if (willPerformParry)
                {
                    ParryCurrentTarget(enemy);
                    return this;
                }
            }

            if (willPerformBlock)
            {
                BlockUsingOffHand(enemy);
            }

            if (willPerformDodge && enemy.currentTarget)
            {
                DodgeAttacks(enemy);
            }

            HandleRotateTowardsTarget(enemy);

            if (enemy.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                ResetStateFlags();
                return attackState;
            }
            else
            {
                GetNewAttack(enemy);
            }

            return this;
        }

        private State ProcessArcherCombatStyle(EnemyManager enemy) 
        {
            enemy.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);

            //If the A.I is falling or is performing some sort of action Stop all movement
            if (!enemy.isGrounded || enemy.isInteracting)
            {
                enemy.animator.SetFloat("Vertical", 0);
                enemy.animator.SetFloat("Horizontal", 0);
                return this;
            }

            //If the A.I has gotten too far from it's target, return the A.I to it's pursue target state
            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                ResetStateFlags();
                return pursueTargetState;
            }

            //Randomises the walking pattern of our A.I so they circle the player
            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                //Decide Circling Action
                DecideCirclingAction(enemy.enemyAnimatorManager);
            }

            if (enemy.allowAIToPerformDodge)
            {
                RollForDodgeChance(enemy);
            }

            if (willPerformDodge && enemy.currentTarget)
            {
                DodgeAttacks(enemy);
            }

            HandleRotateTowardsTarget(enemy);

            if(!hasAmmoLoaded)
            {
                DrawArrow(enemy);
                AimAtTargetBeforeFiring(enemy);
            }

            if (enemy.currentRecoveryTime <= 0 && hasAmmoLoaded)
            {
                ResetStateFlags();
                return attackState;
            }

            return this;
        }

        protected void HandleRotateTowardsTarget(EnemyManager enemy)
        {
            //Rotate manually
            if (enemy.isPerformingAction)
            {
                Vector3 direction = enemy.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemy.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemy.rotationSpeed / Time.deltaTime);
            }
            //Rotate with pathfinding (navmesh)
            else
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(enemy.navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemy.enemyRigidBody.velocity;

                enemy.navMeshAgent.enabled = true;
                enemy.navMeshAgent.SetDestination(enemy.currentTarget.transform.position);
                enemy.enemyRigidBody.velocity = targetVelocity;
                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, enemy.navMeshAgent.transform.rotation, enemy.rotationSpeed / Time.deltaTime);
            }
        }

        protected void DecideCirclingAction(EnemyAnimatorManager enemyAnimatorManager)
        {
            WalkAroundTarget(enemyAnimatorManager);
        }

        protected void WalkAroundTarget(EnemyAnimatorManager enemyAnimatorManager)
        {
            verticalMovementValue = 0.5f;

            horizontalMovementValue = Random.Range(-1, 1);

            if (horizontalMovementValue <= 1 && horizontalMovementValue >= 0)
            {
                horizontalMovementValue = 0.5f;
            }
            else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0)
            {
                horizontalMovementValue = -0.5f;
            }
        }

        protected virtual void GetNewAttack(EnemyManager enemy)
        {

            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                ItemBasedAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemy.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && enemy.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (enemy.viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && enemy.viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                ItemBasedAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemy.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && enemy.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (enemy.viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && enemy.viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (attackState.currentAttack != null)
                            return;

                        temporaryScore += enemyAttackAction.attackScore;

                        if (temporaryScore > randomValue)
                        {
                            attackState.currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }

        //A.I Rolls
        private void RollForBlockChance(EnemyManager enemy)
        {
            int blockChance = Random.Range(0, 100);

            if(blockChance <= enemy.blockLikelyHood)
            {
                willPerformBlock = true;
            }
            else
            {
                willPerformBlock = false;
            }
        }

        private void RollForDodgeChance(EnemyManager enemy)
        {
            int dodgeChance = Random.Range(0, 100);

            if (dodgeChance <= enemy.dodgeLikelyHood)
            {
                willPerformDodge = true;
            }
            else
            {
                willPerformDodge = false;
            }
        }

        private void RollForParryChance(EnemyManager enemy)
        {
            int parryChance = Random.Range(0, 100);

            if (parryChance <= enemy.parryLikelyHood)
            {
                willPerformParry = true;
            }
            else
            {
                willPerformParry = false;
            }
        }

        //A.I Actions
        private void BlockUsingOffHand(EnemyManager enemy)
        {
            if(enemy.isBlocking == false)
            {
                if(enemy.allowAIToPerformBlock)
                {
                    enemy.isBlocking = true;
                    enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.leftWeapon;
                    enemy.characterCombatManager.SetBlockingAbsorptionsFromBlockingWeapon();
                }
            }
        }

        private void DodgeAttacks(EnemyManager enemy)
        {
            if (!hasPerformedDodge)
            {
                if (!randomDestinationSet)
                {
                    float randomDodgeDirection;

                    hasRandomDodgeDirection = true;
                    randomDodgeDirection = Random.Range(0, 360);
                    targetDodgeDirection = Quaternion.Euler(enemy.transform.eulerAngles.x, randomDodgeDirection, enemy.transform.eulerAngles.z);
                }

                if(enemy.transform.rotation != targetDodgeDirection)
                {
                    Quaternion targetRotation = Quaternion.Slerp(enemy.transform.rotation, targetDodgeDirection, 1f);
                    enemy.transform.rotation = targetRotation;

                    float targetYRotation = targetDodgeDirection.eulerAngles.y;
                    float currentYRotation = enemy.transform.eulerAngles.y;
                    float rotationDifference = Mathf.Abs(targetYRotation - currentYRotation);

                    if(rotationDifference <= 5)
                    {
                        hasPerformedDodge = true;
                        enemy.transform.rotation = targetDodgeDirection;
                        enemy.enemyAnimatorManager.PlayTargetAnimation("Rolling", true);
                    }
                }
            }
        }

        private void DrawArrow(EnemyManager enemy)
        {
            if(!enemy.isTwoHandingWeapon)
            {
                enemy.isTwoHandingWeapon = true;
                enemy.characterWeaponSlotManager.LoadBothWeaponsOnSlots();
            }
            else
            {
                hasAmmoLoaded = true;
                enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.rightWeapon;
                enemy.characterInventoryManager.rightWeapon.th_hold_RB_Action.PerformAction(enemy);
            }
        }

        private void AimAtTargetBeforeFiring(EnemyManager enemy)
        {
            float timeUntilAmmoIsShotAtTarget = Random.Range(enemy.minimumTimeToAimAtTarget, enemy.maximumTimeToAimAtTarget);
            enemy.currentRecoveryTime = timeUntilAmmoIsShotAtTarget;
        }

        private void ParryCurrentTarget(EnemyManager enemy)
        {
            if(enemy.currentTarget.canBeParried)
            {
                if(enemy.distanceFromTarget <= 2)
                {
                    hasPerformedParry = true;
                    enemy.isParrying = true;
                    enemy.enemyAnimatorManager.PlayTargetAnimation("Parry_01", true);
                }
            }
        }

        private void CheckForRiposte(EnemyManager enemy)
        {
            if(enemy.isInteracting)
            {
                enemy.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
                enemy.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                return;
            }
            if(enemy.distanceFromTarget >= 1.0)
            {
                HandleRotateTowardsTarget(enemy);
                enemy.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
                enemy.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
            }
            else
            {
                enemy.isBlocking = false;

                if(!enemy.isInteracting && !enemy.currentTarget.isBeingRiposted && !enemy.currentTarget.isBeingBackStabbed){
                    enemy.enemyRigidBody.velocity = Vector3.zero;
                    enemy.animator.SetFloat("Vertical", 0);
                    enemy.characterCombatManager.AttemptBackStabOrRiposte();
                }
            }
        }

        //Called when ever we exit this state, resets all the state flags
        private void ResetStateFlags()
        {
            hasRandomDodgeDirection = false;
            hasPerformedDodge = false;
            hasAmmoLoaded = false;
            hasPerformedParry = false;
            randomDestinationSet = false;
            willPerformBlock = false;
            willPerformDodge = false;
            willPerformParry = false;
        }
    }
}

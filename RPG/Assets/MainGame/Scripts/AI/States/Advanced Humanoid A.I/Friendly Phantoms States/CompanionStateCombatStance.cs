using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace GI
{
    public class CompanionStateCombatStance : State
    {
        CompanionStateAttackTarget attackState;
        public ItemBasedAttackAction[] enemyAttacks;
        CompanionStatePursueTarget pursueTargetState;
        CompanionStateFollowHost followHostState;

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
            pursueTargetState = GetComponent<CompanionStatePursueTarget>();
            attackState = GetComponent<CompanionStateAttackTarget>();
            followHostState = GetComponent<CompanionStateFollowHost>();
        }

        public override State Tick(AICharacterManager aiCharacter)
        {
            //If we are too far away from our host , we return to them
            if (aiCharacter.distanceFromCompanion > aiCharacter.maxDistanceFromCompanion)
            {
                return followHostState;
            }

            if (aiCharacter.combatStyle == AICombatStyle.swordAndShield)
            {
                return ProcessSwordAndShieldCombatStyle(aiCharacter);
            }
            else if (aiCharacter.combatStyle == AICombatStyle.archer)
            {
                return ProcessArcherCombatStyle(aiCharacter);
            }
            else
            {
                return this;
            }
        }

        private State ProcessSwordAndShieldCombatStyle(AICharacterManager aiCharacter)
        {
            //If the A.I is falling or is performing some sort of action Stop all movement
            if (!aiCharacter.isGrounded || aiCharacter.isInteracting)
            {
                aiCharacter.animator.SetFloat("Vertical", 0);
                aiCharacter.animator.SetFloat("Horizontal", 0);
                return this;
            }

            //If the A.I has gotten too far from it's target, return the A.I to it's pursue target state
            if (aiCharacter.distanceFromTarget > aiCharacter.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            //Randomises the walking pattern of our A.I so they circle the player
            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                //Decide Circling Action
                DecideCirclingAction(aiCharacter.aiCharacterAnimatorManager);
            }

            if (aiCharacter.allowAIToPerformParry)
            {
                if (aiCharacter.currentTarget.canBeRiposted)
                {
                    CheckForRiposte(aiCharacter);
                    return this;
                }
            }

            if (aiCharacter.allowAIToPerformBlock)
            {
                RollForBlockChance(aiCharacter);
            }

            if (aiCharacter.allowAIToPerformDodge)
            {
                RollForDodgeChance(aiCharacter);
            }

            if (aiCharacter.allowAIToPerformParry)
            {
                RollForParryChance(aiCharacter);
            }

            if (aiCharacter.currentTarget.isAttacking)
            {
                if (willPerformParry)
                {
                    ParryCurrentTarget(aiCharacter);
                    return this;
                }
            }

            if (willPerformBlock)
            {
                BlockUsingOffHand(aiCharacter);
            }

            if (willPerformDodge && aiCharacter.currentTarget)
            {
                DodgeAttacks(aiCharacter);
            }

            HandleRotateTowardsTarget(aiCharacter);

            if (aiCharacter.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                ResetStateFlags();
                return attackState;
            }
            else
            {
                GetNewAttack(aiCharacter);
            }

            HandleMovement(aiCharacter);

            return this;
        }

        private State ProcessArcherCombatStyle(AICharacterManager aiCharacter)
        {
            //If the A.I is falling or is performing some sort of action Stop all movement
            if (!aiCharacter.isGrounded || aiCharacter.isInteracting)
            {
                aiCharacter.animator.SetFloat("Vertical", 0);
                aiCharacter.animator.SetFloat("Horizontal", 0);
                return this;
            }

            //If the A.I has gotten too far from it's target, return the A.I to it's pursue target state
            if (aiCharacter.distanceFromTarget > aiCharacter.maximumAggroRadius)
            {
                ResetStateFlags();
                return pursueTargetState;
            }

            //Randomises the walking pattern of our A.I so they circle the player
            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                //Decide Circling Action
                DecideCirclingAction(aiCharacter.aiCharacterAnimatorManager);
            }

            if (aiCharacter.allowAIToPerformDodge)
            {
                RollForDodgeChance(aiCharacter);
            }

            if (willPerformDodge && aiCharacter.currentTarget)
            {
                DodgeAttacks(aiCharacter);
            }

            HandleRotateTowardsTarget(aiCharacter);

            if (!hasAmmoLoaded)
            {
                DrawArrow(aiCharacter);
                AimAtTargetBeforeFiring(aiCharacter);
            }

            if (aiCharacter.currentRecoveryTime <= 0 && hasAmmoLoaded)
            {
                ResetStateFlags();
                return attackState;
            }

            if (aiCharacter.isStationaryArcher)
            {
                aiCharacter.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
            }
            else
            {
                HandleMovement(aiCharacter);
            }

            return this;
        }

        protected void HandleRotateTowardsTarget(AICharacterManager aiCharacter)
        {
            Vector3 direction = aiCharacter.currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            aiCharacter.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aiCharacter.rotationSpeed / Time.deltaTime);
        }

        protected void DecideCirclingAction(AICharacterAnimatorManager aiCharacterAnimatorManager)
        {
            WalkAroundTarget(aiCharacterAnimatorManager);
        }

        protected void WalkAroundTarget(AICharacterAnimatorManager aiCharacterAnimatorManager)
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

        protected virtual void GetNewAttack(AICharacterManager aiCharacter)
        {

            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                ItemBasedAttackAction enemyAttackAction = enemyAttacks[i];

                if (aiCharacter.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && aiCharacter.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (aiCharacter.viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && aiCharacter.viewableAngle >= enemyAttackAction.minimumAttackAngle)
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

                if (aiCharacter.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && aiCharacter.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (aiCharacter.viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && aiCharacter.viewableAngle >= enemyAttackAction.minimumAttackAngle)
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
        private void RollForBlockChance(AICharacterManager aiCharacter)
        {
            int blockChance = Random.Range(0, 100);

            if (blockChance <= aiCharacter.blockLikelyHood)
            {
                willPerformBlock = true;
            }
            else
            {
                willPerformBlock = false;
            }
        }

        private void RollForDodgeChance(AICharacterManager aiCharacter)
        {
            int dodgeChance = Random.Range(0, 100);

            if (dodgeChance <= aiCharacter.dodgeLikelyHood)
            {
                willPerformDodge = true;
            }
            else
            {
                willPerformDodge = false;
            }
        }

        private void RollForParryChance(AICharacterManager aiCharacter)
        {
            int parryChance = Random.Range(0, 100);

            if (parryChance <= aiCharacter.parryLikelyHood)
            {
                willPerformParry = true;
            }
            else
            {
                willPerformParry = false;
            }
        }

        //A.I Actions
        private void BlockUsingOffHand(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isBlocking == false)
            {
                if (aiCharacter.allowAIToPerformBlock)
                {
                    aiCharacter.isBlocking = true;
                    aiCharacter.characterInventoryManager.currentItemBeingUsed = aiCharacter.characterInventoryManager.leftWeapon;
                    aiCharacter.characterCombatManager.SetBlockingAbsorptionsFromBlockingWeapon();
                }
            }
        }

        private void DodgeAttacks(AICharacterManager aiCharacter)
        {
            if (!hasPerformedDodge)
            {
                if (!randomDestinationSet)
                {
                    float randomDodgeDirection;

                    hasRandomDodgeDirection = true;
                    randomDodgeDirection = Random.Range(0, 360);
                    targetDodgeDirection = Quaternion.Euler(aiCharacter.transform.eulerAngles.x, randomDodgeDirection, aiCharacter.transform.eulerAngles.z);
                }

                if (aiCharacter.transform.rotation != targetDodgeDirection)
                {
                    Quaternion targetRotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetDodgeDirection, 1f);
                    aiCharacter.transform.rotation = targetRotation;

                    float targetYRotation = targetDodgeDirection.eulerAngles.y;
                    float currentYRotation = aiCharacter.transform.eulerAngles.y;
                    float rotationDifference = Mathf.Abs(targetYRotation - currentYRotation);

                    if (rotationDifference <= 5)
                    {
                        hasPerformedDodge = true;
                        aiCharacter.transform.rotation = targetDodgeDirection;
                        aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation("Rolling", true);
                    }
                }
            }
        }

        private void DrawArrow(AICharacterManager aiCharacter)
        {
            if (!aiCharacter.isTwoHandingWeapon)
            {
                aiCharacter.isTwoHandingWeapon = true;
                aiCharacter.characterWeaponSlotManager.LoadBothWeaponsOnSlots();
            }
            else
            {
                hasAmmoLoaded = true;
                aiCharacter.characterInventoryManager.currentItemBeingUsed = aiCharacter.characterInventoryManager.rightWeapon;
                aiCharacter.characterInventoryManager.rightWeapon.th_hold_Left_Click.PerformAction(aiCharacter);
            }
        }

        private void AimAtTargetBeforeFiring(AICharacterManager aiCharacter)
        {
            float timeUntilAmmoIsShotAtTarget = Random.Range(aiCharacter.minimumTimeToAimAtTarget, aiCharacter.maximumTimeToAimAtTarget);
            aiCharacter.currentRecoveryTime = timeUntilAmmoIsShotAtTarget;
        }

        private void ParryCurrentTarget(AICharacterManager aiCharacter)
        {
            if (aiCharacter.currentTarget.canBeParried)
            {
                if (aiCharacter.distanceFromTarget <= 2)
                {
                    hasPerformedParry = true;
                    aiCharacter.isParrying = true;
                    aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation("Parry_01", true);
                }
            }
        }

        private void CheckForRiposte(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isInteracting)
            {
                aiCharacter.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                return;
            }
            if (aiCharacter.distanceFromTarget >= 1.0)
            {
                HandleRotateTowardsTarget(aiCharacter);
                aiCharacter.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
            }
            else
            {
                aiCharacter.isBlocking = false;

                if (!aiCharacter.isInteracting && !aiCharacter.currentTarget.isBeingRiposted && !aiCharacter.currentTarget.isBeingBackStabbed)
                {
                    aiCharacter.aiCharacterRigidBody.velocity = Vector3.zero;
                    aiCharacter.animator.SetFloat("Vertical", 0);
                    aiCharacter.characterCombatManager.AttemptBackStabOrRiposte();
                }
            }
        }

        private void HandleMovement(AICharacterManager aiCharacter)
        {
            if (aiCharacter.distanceFromTarget <= aiCharacter.stoppingDistance)
            {
                aiCharacter.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            }
            else
            {
                aiCharacter.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
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

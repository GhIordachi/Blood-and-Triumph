using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace GI
{
    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
        protected Collider damageCollider;
        public bool enabledDamageColliderOnStartUp = false;

        [Header("Team I.D")]
        public int teamIDNumber = 0;

        [Header("Poise")]
        public float poiseDamage;
        public float offensivePoiseBonus;

        [Header("Damage")]
        public int physicalDamage;
        public int fireDamage;
        public int magicDamage;
        public int lightningDamage;
        public int darkDamage;

        [Header("Guard Break Modifier")]
        public float guardBreakModifier = 1;

        protected bool shieldHasBeenHit;
        protected bool hasBeenParried;
        protected string currentDamageAnimation;

        protected Vector3 contactPoint;
        protected float angleHitFrom;

        protected List<CharacterManager> charactersDamagedDuringThisCalculation = new List<CharacterManager>();

        protected virtual void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = enabledDamageColliderOnStartUp;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            if(charactersDamagedDuringThisCalculation.Count > 0)
            {
                charactersDamagedDuringThisCalculation.Clear();
            }
            damageCollider.enabled = false;
        }

        protected virtual void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Damageable Character"))
            {
                shieldHasBeenHit = false;
                hasBeenParried = false;

                CharacterManager enemyManager = collision.GetComponentInParent<CharacterManager>();

                if (enemyManager != null)
                {
                    AICharacterManager aiCharacter = enemyManager as AICharacterManager;

                    if (charactersDamagedDuringThisCalculation.Contains(enemyManager))
                        return;

                    charactersDamagedDuringThisCalculation.Add(enemyManager);

                    if (enemyManager.characterStatsManager.teamIDNumber == teamIDNumber)
                        return;

                    CheckForParry(enemyManager);
                    CheckForBlock(enemyManager);

                    if (enemyManager.characterStatsManager.teamIDNumber == teamIDNumber)
                        return;

                    if (hasBeenParried)
                        return;

                    if (shieldHasBeenHit)
                        return;

                    enemyManager.characterStatsManager.poiseResetTimer = enemyManager.characterStatsManager.totalPoiseResetTime;
                    enemyManager.characterStatsManager.totalPoiseDefence = enemyManager.characterStatsManager.totalPoiseDefence - poiseDamage;

                    //Detects where the collider is hit by the weapon
                    contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    angleHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));

                    //Deals damage
                    DealDamage(enemyManager);

                    if (aiCharacter != null)
                    {
                        //If the target is A.I, the A.I receives a new target, the person dealind damage to it
                        aiCharacter.currentTarget = characterManager;
                    }
                }
            }
            if (collision.tag == "Illusionary Wall")
            {
                Debug.Log("am dat");
                IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();

                illusionaryWall.wallHasBeenHit = true;
            }
        }

        protected virtual void CheckForParry(CharacterManager enemyManager)
        {
            if (enemyManager.isParrying)
            {
                //Check here if you are parryable
                characterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Parried", true);
                hasBeenParried = true;
            }
        }

        protected virtual void CheckForBlock(CharacterManager enemyManager)
        {
            Vector3 directionFromPlayerToEnemy = (characterManager.transform.position - enemyManager.transform.position);
            float dotValueFromPlayerToEnemy = Vector3.Dot(directionFromPlayerToEnemy, enemyManager.transform.forward);

            if (enemyManager.isBlocking && dotValueFromPlayerToEnemy > 0.3f)
            {
                shieldHasBeenHit = true;

                TakeBlockedDamageEffect takeBlockedDamage = Instantiate(WorldCharacterEffectsManager.instance.takeBlockedDamageEffect);
                takeBlockedDamage.physicalDamage = physicalDamage;
                takeBlockedDamage.fireDamage = fireDamage;
                takeBlockedDamage.poiseDamage = poiseDamage;
                takeBlockedDamage.staminaDamage = poiseDamage;

                enemyManager.characterEffectsManager.ProcessEffectInstantly(takeBlockedDamage);
            }
        }

        protected virtual void DealDamage(CharacterManager enemy)
        {
            float finalPhysicalDamage = physicalDamage;
            float finalFireDamage = fireDamage;
            //If we are using the right weapon, we compare the right weapon modifiers
            if(characterManager.isUsingRightHand)
            {
                if (characterManager.characterCombatManager.currentAttackType == AttackType.light)
                {
                    finalPhysicalDamage = physicalDamage * characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
                    finalFireDamage = fireDamage * characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
                }
                else if (characterManager.characterCombatManager.currentAttackType == AttackType.heavy)
                {
                    finalPhysicalDamage = physicalDamage * characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
                    finalFireDamage = fireDamage * characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
                }
            }
            //otherwise we compare the left weapon modifiers
            else if(characterManager.isUsingLeftHand)
            {
                if (characterManager.characterCombatManager.currentAttackType == AttackType.light)
                {
                    finalPhysicalDamage = physicalDamage * characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
                    finalFireDamage = fireDamage * characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
                }
                else if (characterManager.characterCombatManager.currentAttackType == AttackType.heavy)
                {
                    finalPhysicalDamage = physicalDamage * characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
                    finalFireDamage = fireDamage * characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
                }
            }

            TakeDamageEffect takeDamageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            takeDamageEffect.physicalDamage = finalPhysicalDamage;
            takeDamageEffect.fireDamage = finalFireDamage;
            takeDamageEffect.poiseDamage = poiseDamage;
            takeDamageEffect.contactPoint = contactPoint;
            takeDamageEffect.angleHitFrom = angleHitFrom;
            enemy.characterEffectsManager.ProcessEffectInstantly(takeDamageEffect);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class CharacterCombatManager : MonoBehaviour
    {
        protected CharacterManager character;

        [Header("Combat Transforms")]
        public Transform backStabReceiverTransform;
        public Transform riposteReceiverTransform;

        public LayerMask characterLayer;
        public float criticalAttackRange = 0.7f;

        [Header("Attack Type")]
        public AttackType currentAttackType;

        [Header("Attack Animations")]
        public string oh_light_attack_01 = "OH_Light_Attack_01";
        public string oh_light_attack_02 = "OH_Light_Attack_02";
        public string oh_heavy_attack_01 = "OH_Heavy_Attack_01";
        public string oh_heavy_attack_02 = "OH_Heavy_Attack_02";
        public string oh_running_attack_01 = "OH_Running_Attack_01";
        public string oh_jumping_attack_01 = "OH_Jumping_Attack_01";

        public string oh_charge_attack_01 = "OH_Charging_Attack_Charge_01";
        public string oh_charge_attack_02 = "OH_Charging_Attack_Charge_02";

        public string th_light_attack_01 = "TH_Light_Attack_01";
        public string th_light_attack_02 = "TH_Light_Attack_02";
        public string th_heavy_attack_01 = "TH_Heavy_Attack_01";
        public string th_heavy_attack_02 = "TH_Heavy_Attack_02";
        public string th_running_attack_01 = "TH_Running_Attack_01";
        public string th_jumping_attack_01 = "TH_Jumping_Attack_01";

        public string th_charge_attack_01 = "TH_Charging_Attack_Charge_01";
        public string th_charge_attack_02 = "TH_Charging_Attack_Charge_02";

        public string weapon_art = "Weapon_Art";

        public int pendingCriticalDamage;

        public string lastAttack;


        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void SetBlockingAbsorptionsFromBlockingWeapon()
        {
            if(character.isUsingRightHand)
            {
                character.characterStatsManager.blockingPhysicalDamageAbsorption = character.characterInventoryManager.rightWeapon.physicalBlockingDamageAbsorption;
                character.characterStatsManager.blockingFireDamageAbsorption = character.characterInventoryManager.rightWeapon.fireBlockingDamageAbsorption;
                character.characterStatsManager.blockingStabilityRating = character.characterInventoryManager.rightWeapon.stability;
            }
            else if (character.isUsingLeftHand)
            {
                character.characterStatsManager.blockingPhysicalDamageAbsorption = character.characterInventoryManager.leftWeapon.physicalBlockingDamageAbsorption;
                character.characterStatsManager.blockingFireDamageAbsorption = character.characterInventoryManager.leftWeapon.fireBlockingDamageAbsorption;
                character.characterStatsManager.blockingStabilityRating = character.characterInventoryManager.leftWeapon.stability;
            }
        }

        public virtual void DrainStaminaBasedOnAttack()
        {
            
        }

        public virtual void AttemptBlock(DamageCollider attackingWeapon, float physicalDamage, float fireDamage, string blockAnimation)
        {
            float staminaDamageAbsorption = ((physicalDamage + fireDamage) * attackingWeapon.guardBreakModifier)
                * (character.characterStatsManager.blockingStabilityRating / 100);

            float staminaDamage = ((physicalDamage + fireDamage) * attackingWeapon.guardBreakModifier) - staminaDamageAbsorption;

            character.characterStatsManager.currentStamina = character.characterStatsManager.currentStamina - staminaDamage;

            if (character.characterStatsManager.currentStamina <= 0)
            {
                character.isBlocking = false;
                character.characterAnimatorManager.PlayTargetAnimation("Guard Break", true);
            }
            else
            {
                character.characterAnimatorManager.PlayTargetAnimation(blockAnimation, true);
            }
        }

        private void SuccessfullyCastSpell()
        {
            character.characterInventoryManager.currentSpell.SuccessfullyCastSpell(character);
        }

        IEnumerator ForceMoveCharacterToEnemyBackStabPosition(CharacterManager characterPerformingBackStab)
        {
            for(float timer = 0.05f; timer < 0.5f; timer = timer + 0.5f)
            {
                Quaternion backStabRotation = Quaternion.LookRotation(characterPerformingBackStab.transform.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, backStabRotation, 1);
                transform.parent = characterPerformingBackStab.characterCombatManager.backStabReceiverTransform;
                transform.localPosition = characterPerformingBackStab.characterCombatManager.backStabReceiverTransform.localPosition;
                transform.parent = null;
                yield return new WaitForSeconds(0.05f);
            }
        }

        IEnumerator ForceMoveCharacterToEnemyRipostePosition(CharacterManager characterPerformingRiposte)
        {
            for (float timer = 0.05f; timer < 0.5f; timer = timer + 0.5f)
            {
                Quaternion riposteRotation = Quaternion.LookRotation(-characterPerformingRiposte.transform.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, riposteRotation, 1);
                transform.parent = characterPerformingRiposte.characterCombatManager.riposteReceiverTransform;
                transform.localPosition = characterPerformingRiposte.characterCombatManager.riposteReceiverTransform.localPosition;
                transform.parent = null;
                yield return new WaitForSeconds(0.05f);
            }
        }

        public void GetBackStabbed(CharacterManager characterPerformingTheBackStab)
        {
            //Play sound fx
            character.isBeingBackStabbed = true;

            //Force lock position
            StartCoroutine(ForceMoveCharacterToEnemyBackStabPosition(characterPerformingTheBackStab));

            character.characterAnimatorManager.PlayTargetAnimation("Back Stabbed", true);
        }

        public void GetRiposted(CharacterManager characterPerformingTheRiposte)
        {
            //Play sound fx
            character.isBeingRiposted = true;

            //Force lock position
            StartCoroutine(ForceMoveCharacterToEnemyRipostePosition(characterPerformingTheRiposte));

            character.characterAnimatorManager.PlayTargetAnimation("Riposted", true);
        }

        public void AttemptBackStabOrRiposte()
        {
            if (character.isInteracting)
                return;

            if (character.characterStatsManager.currentStamina <= 0)
                return;

            RaycastHit hit;

            if(Physics.Raycast(character.criticalAttackRayCastStartPoint.transform.position, 
                character.transform.TransformDirection(Vector3.forward), out hit, criticalAttackRange, characterLayer))
            {
                CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();
                Vector3 directionFromCharacterToEnemy = transform.position - enemyCharacter.transform.position;
                float dotValue = Vector3.Dot(directionFromCharacterToEnemy, enemyCharacter.transform.forward);

                Debug.Log("Current dot value is " + dotValue);

                if(enemyCharacter.canBeRiposted)
                {
                    if(dotValue <= 1.2f && dotValue >= 0.6f)
                    {
                        AttemptRiposte(hit);
                        return;
                    }
                }

                if(dotValue >= -0.8f && dotValue <= -0.5f)
                {
                    AttemptBackStab(hit);
                }
            }
        }

        private void AttemptBackStab(RaycastHit hit)
        {
            CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();
            
            if (enemyCharacter != null)
            {
                if (!enemyCharacter.isBeingBackStabbed || !enemyCharacter.isBeingRiposted)
                {
                    //The enemy cannot be damaged whilst being critically damaged
                    EnableIsInvulnerable();
                    character.isPerformingBackStab = true;
                    character.characterAnimatorManager.EraseHandIKForWeapon();

                    character.characterAnimatorManager.PlayTargetAnimation("Back Stab", true);

                    float criticalDamage = (character.characterInventoryManager.rightWeapon.criticalDamageMultiplier *
                        (character.characterInventoryManager.rightWeapon.physicalDamage + character.characterInventoryManager.rightWeapon.fireDamage));

                    int roundedCriticalDamage = Mathf.RoundToInt(criticalDamage);
                    enemyCharacter.characterCombatManager.pendingCriticalDamage = roundedCriticalDamage;
                    enemyCharacter.characterCombatManager.GetBackStabbed(character);
                }
            }
        }

        private void AttemptRiposte(RaycastHit hit)
        {
            CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();

            if (enemyCharacter != null)
            {
                if (!enemyCharacter.isBeingBackStabbed || !enemyCharacter.isBeingRiposted)
                {
                    //The enemy cannot be damaged whilst being critically damaged
                    EnableIsInvulnerable();
                    character.isPerformingRiposte = true;
                    character.characterAnimatorManager.EraseHandIKForWeapon();

                    character.characterAnimatorManager.PlayTargetAnimation("Riposte", true);

                    float criticalDamage = (character.characterInventoryManager.rightWeapon.criticalDamageMultiplier *
                        (character.characterInventoryManager.rightWeapon.physicalDamage + character.characterInventoryManager.rightWeapon.fireDamage));

                    int roundedCriticalDamage = Mathf.RoundToInt(criticalDamage);
                    enemyCharacter.characterCombatManager.pendingCriticalDamage = roundedCriticalDamage;
                    enemyCharacter.characterCombatManager.GetRiposted(character);
                }
            }
        }

        private void EnableIsInvulnerable()
        {
            character.animator.SetBool("isInvulnerable", true);
        }

        public void ApplyPendingDamage()
        {
            character.characterStatsManager.TakeDamageNoAnimation(pendingCriticalDamage, 0);
        }

        public void EnableCanBeParried()
        {
            character.canBeParried = true;
        }

        public void DisableCanBeParried()
        {
            character.canBeParried = false;
        }
    }
}

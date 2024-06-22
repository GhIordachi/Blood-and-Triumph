using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

namespace GI {
    public class AICharacterStatsManager : CharacterStatsManager
    {
        AICharacterManager aiCharacter;
        public UIEnemyHealthBar aiCharacterHealthBar;

        public bool isBoss; 

        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
            maxHealth = SetMaxHealthFromHealthLevel();
            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentHealth = maxHealth;
            currentStamina = maxStamina;
        }

        protected override void Start()
        {
            base.Start();
            if (!isBoss)
            {
                aiCharacterHealthBar.SetMaxHealth(maxHealth);
            }
        }

        public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage, int magicDamage)
        {
            if (aiCharacter.isDead)
                return;

            base.TakeDamageNoAnimation(physicalDamage,fireDamage,magicDamage);

            if (!isBoss)
            {
                aiCharacterHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && aiCharacter.aiCharacterBossManager != null)
            {
                aiCharacter.aiCharacterBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }
        }

        public override void TakePoisonDamage(int damage)
        {
            if (aiCharacter.isDead)
                return;

            base.TakePoisonDamage(damage);
            if (!isBoss)
            {
                aiCharacterHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && aiCharacter.aiCharacterBossManager != null)
            {
                aiCharacter.aiCharacterBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                aiCharacter.isDead = true;
                aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation("Death_01", true);
            }
        }

        public void BreakGuard()
        {
            aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation("Break Guard", true);
        }

        private void HandleDeath()
        {
            currentHealth = 0;
            aiCharacter.isDead = true;
            aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation("Death_01", true);
            aiCharacter.characterController.enabled = false;
        }
    }
}

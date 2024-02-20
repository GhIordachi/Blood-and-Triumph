using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI {
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager player;

        public HealthBar healthBar;
        public StaminaBar staminaBar;
        public FocusPointBar focusPointBar;

        public float staminaRegenerationAmount = 1;
        public float staminaRegenerationAmountWhilstBlocking = 0.1f;
        public float staminaRegenTimer = 0;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusPointBar = FindObjectOfType<FocusPointBar>();
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetCurrentHealth(currentHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
            staminaBar.SetCurrentStamina(currentStamina);

            maxFocusPoints = SetMaxFocusPointsFromFocusLevel();
            currentFocusPoints = maxFocusPoints;
            focusPointBar.SetMaxFocusPoint(maxFocusPoints);
            focusPointBar.SetCurrentFocusPoint(currentFocusPoints);
        }

        public override void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else if (poiseResetTimer <= 0 && !player.isInteracting) 
            {
                totalPoiseDefence = armorPoiseBonus;
            }
        }

        public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation, CharacterManager enemyCharacterDamagingMe)
        {
            if (player.isInvulnerable)
                return;

            if(player.isDead) 
                return;

            base.TakeDamage(physicalDamage, fireDamage, damageAnimation, enemyCharacterDamagingMe);
            healthBar.SetCurrentHealth(currentHealth);
            player.playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                player.isDead = true;
                player.playerAnimatorManager.PlayTargetAnimation("Death_01", true);
            }
        }

        public override void TakePoisonDamage(int damage)
        {
            if (player.isDead)
                return;

            base.TakePoisonDamage(damage);
            healthBar.SetCurrentHealth(currentHealth);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                player.isDead = true;
                player.playerAnimatorManager.PlayTargetAnimation("Death_01", true);
            }
        }

        public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
        {
            if (player.isDead)
                return;

            base.TakeDamageNoAnimation(physicalDamage, fireDamage);
            healthBar.SetCurrentHealth(currentHealth);
        }

        public override void DeductStamina(float staminaToDeduct)
        {
            base.DeductStamina(staminaToDeduct);
            staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
        }

        public void RegenerateStamina()
        {
            if(player.isInteracting)
            {
                staminaRegenTimer = 0;
            }
            else
            {
                staminaRegenTimer += Time.deltaTime;

                if (currentStamina < maxStamina && staminaRegenTimer > 1f)
                {
                    if(player.isBlocking)
                    {
                        currentStamina += staminaRegenerationAmountWhilstBlocking * Time.deltaTime;
                        staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                    }
                    else
                    {
                        currentStamina += staminaRegenerationAmount * Time.deltaTime;
                        staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                    }
                }
            }
        }

        public void HealPlayer(int healAmount)
        {
            currentHealth = currentHealth + healAmount;

            if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            healthBar.SetCurrentHealth(currentHealth);
        }

        public void DeductFocusPoints(int focusPoints)
        {
            currentFocusPoints = currentFocusPoints - focusPoints;

            if(currentFocusPoints < 0)
            {
                currentFocusPoints = 0;
            }

            focusPointBar.SetCurrentFocusPoint(currentFocusPoints);
        }

        public void AddSouls(int souls)
        {
            currentSoulCount = currentSoulCount + souls;
        }
    }
}

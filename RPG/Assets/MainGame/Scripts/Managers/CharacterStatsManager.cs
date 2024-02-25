using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI {
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Name:")]
        public string characterName = "";

        [Header("Team I.D")]
        public int teamIDNumber = 0;

        public int maxHealth;
        public int currentHealth;

        public float maxStamina;
        public float currentStamina;

        public float maxFocusPoints;
        public float currentFocusPoints;

        public int currentSoulCount = 0;
        public int souldAwardedOnDeath = 50;

        [Header("Character Level")]
        public int playerLevel = 1;

        [Header("Stat Levels")]
        public int healthLevel = 10;
        public int staminaLevel = 10;
        public int focusLevel = 10;
        public int poiseLevel = 10;
        public int strengthLevel = 10;
        public int dexterityLevel = 10;
        public int intelligenceLevel = 10;
        public int faithLevel = 10;

        [Header("Poise")]
        public float totalPoiseDefence; //The total poise during damage calculation
        public float offensivePoiseBonus; //The poise you Gain during an attack with a weapon
        public float armorPoiseBonus; //The poise you Gain from wearing what ever you have equipped
        public float totalPoiseResetTime = 15;
        public float poiseResetTimer = 0;

        [Header("Armor Absorptions")]
        public float physicalDamageAbsorptionHead;
        public float physicalDamageAbsorptionBody;
        public float physicalDamageAbsorptionLegs;
        public float physicalDamageAbsorptionHands;

        public float fireDamageAbsorptionHead;
        public float fireDamageAbsorptionBody;
        public float fireDamageAbsorptionLegs;
        public float fireDamageAbsorptionHands;

        [Header("Blocking Absorptions")]
        public float blockingPhysicalDamageAbsorption;
        public float blockingFireDamageAbsorption;
        public float blockingStabilityRating;

        //Lightning absorption
        //Magic absorption
        //Dark absorption

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {
            HandlePoiseResetTimer();
        }

        private void Start()
        {
            totalPoiseDefence = armorPoiseBonus;
        }

        public virtual void TakeDamage(int physicalDamage,int fireDamage, string damageAnimation, CharacterManager enemyCharacterDamagingMe)
        {
            if (character.isDead) 
                return;

            character.characterAnimatorManager.EraseHandIKForWeapon();

            float totalPhysicalDamageAbsorption = 1 - 
                (1 - physicalDamageAbsorptionHead / 100) * 
                (1 - physicalDamageAbsorptionBody / 100) *
                (1 - physicalDamageAbsorptionHands / 100) *
                (1 - physicalDamageAbsorptionLegs / 100);

            physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

            float totalFireDamageAbsorption = 1 -
                (1 - fireDamageAbsorptionHead / 100) *
                (1 - fireDamageAbsorptionBody / 100) *
                (1 - fireDamageAbsorptionHands / 100) *
                (1 - fireDamageAbsorptionLegs / 100);

            fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

            Debug.Log("Total damage absorption is " + totalPhysicalDamageAbsorption + "%");

            float finalDamage = physicalDamage + fireDamage; // + magicDamage etc.

            if (enemyCharacterDamagingMe.isPerformingFullyChargedAttack)
            {
                finalDamage = finalDamage * 1.5f;
            }

            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

            Debug.Log("Total damage dealt is " + finalDamage);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                character.isDead = true;
            }

            character.characterSoundFXManager.PlayRandomDamageSoundFX();
        }

        public virtual void TakeDamageAfterBlock(int physicalDamage, int fireDamage, CharacterManager enemyCharacterDamagingMe)
        {
            if (character.isDead)
                return;

            character.characterAnimatorManager.EraseHandIKForWeapon();

            float totalPhysicalDamageAbsorption = 1 -
                (1 - physicalDamageAbsorptionHead / 100) *
                (1 - physicalDamageAbsorptionBody / 100) *
                (1 - physicalDamageAbsorptionHands / 100) *
                (1 - physicalDamageAbsorptionLegs / 100);

            physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

            float totalFireDamageAbsorption = 1 -
                (1 - fireDamageAbsorptionHead / 100) *
                (1 - fireDamageAbsorptionBody / 100) *
                (1 - fireDamageAbsorptionHands / 100) *
                (1 - fireDamageAbsorptionLegs / 100);

            fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

            Debug.Log("Total damage absorption is " + totalPhysicalDamageAbsorption + "%");

            float finalDamage = physicalDamage + fireDamage; // + magicDamage etc.

            if (enemyCharacterDamagingMe.isPerformingFullyChargedAttack)
            {
                finalDamage = finalDamage * 1.5f;
            }

            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

            Debug.Log("Total damage dealt is " + finalDamage);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                character.isDead = true;
            }

            //Play blocking sound
            //character.characterSoundFXManager.PlayRandomDamageSoundFX();
        }

        public virtual void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
        {
            if (character.isDead)
                return;

            float totalPhysicalDamageAbsorption = 1 -
                (1 - physicalDamageAbsorptionHead / 100) *
                (1 - physicalDamageAbsorptionBody / 100) *
                (1 - physicalDamageAbsorptionHands / 100) *
                (1 - physicalDamageAbsorptionLegs / 100);

            physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

            float totalFireDamageAbsorption = 1 -
                (1 - fireDamageAbsorptionHead / 100) *
                (1 - fireDamageAbsorptionBody / 100) *
                (1 - fireDamageAbsorptionHands / 100) *
                (1 - fireDamageAbsorptionLegs / 100);

            fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

            Debug.Log("Total damage absorption is " + totalPhysicalDamageAbsorption + "%");

            float finalDamage = physicalDamage + fireDamage; // + magicDamage etc.

            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                character.isDead = true;
            }
        }

        public virtual void TakePoisonDamage(int damage)
        {
            currentHealth = currentHealth - damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                character.isDead = true;
            }
        }

        public virtual void HandlePoiseResetTimer()
        {
            if(poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else
            {
                totalPoiseDefence = armorPoiseBonus;
            }
        }

        public virtual void DeductStamina(float staminaToDeduct)
        {
            currentStamina = currentStamina -staminaToDeduct;
        }

        public int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        public float SetMaxFocusPointsFromFocusLevel()
        {
            maxFocusPoints = focusLevel * 10;
            return maxFocusPoints;
        }

        public virtual void HealCharacter(int healAmount)
        {
            currentHealth = currentHealth + healAmount;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }
}

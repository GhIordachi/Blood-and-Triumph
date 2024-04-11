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

        public int currentXPCount = 0;
        public int XPAwardedOnDeath = 50;

        [Header("Character Level")]
        public int playerLevel = 1;

        [Header("Stat Levels")]
        public int healthLevel = 10;
        public int staminaLevel = 10;
        public int focusLevel = 10;
        public int poiseLevel = 10;
        public int strengthLevel = 10;
        public int faithLevel = 10;

        [Header("Equipment Load")]
        public float currentEquipLoad = 0;
        public float maxEquipLoad = 0;
        public EncumbranceLevel encumbraceLevel;

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

        public float magicDamageAbsorptionHead;
        public float magicDamageAbsorptionBody;
        public float magicDamageAbsorptionLegs;
        public float magicDamageAbsorptionHands;

        [Header("Resistances")]
        public float poisonResistance;

        [Header("Blocking Absorptions")]
        public float blockingPhysicalDamageAbsorption;
        public float blockingFireDamageAbsorption;
        public float blockingMagicDamageAbsorption;
        public float blockingStabilityRating;

        //Any damage dealt by this player is modified bythese amounts
        [Header("Damage Type Modifiers")]
        public float physicalDamagePercentageModifier = 100;
        public float fireDamagePercentageModifier = 100;
        public float magicDamagePercentageModifier = 100;

        //Incoming damage after armor calculation is modified by this values
        [Header("Damage Absorptions Modifier")]
        public float physicalAbsorptionPercentageModifier = 0;
        public float fireAbsorptionPercentageModifier = 0;
        public float magicAbsorptionPercentageModifier = 0;

        [Header("Poison")]
        public bool isPoisoned;
        public float poisonBuildup = 0; //The build up over time that poisons the player after reaching 100
        public float poisonAmount = 100; //The amount of poison the player has to process before becoming unpoisoned

        //Lightning absorption
        //Magic absorption
        //Dark absorption

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            CalculateAndSetMaxEquipLoad();
        }

        protected virtual void Update()
        {
            HandlePoiseResetTimer();
        }

        protected virtual void Start()
        {
            totalPoiseDefence = armorPoiseBonus;
        }

        public virtual void TakeDamageNoAnimation(int physicalDamage, int fireDamage, int magicDamage)
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

            float totalMagicDamageAbsorption = 1 -
                (1 - magicDamageAbsorptionHead / 100) *
                (1 - magicDamageAbsorptionBody / 100) *
                (1 - magicDamageAbsorptionHands / 100) *
                (1 - magicDamageAbsorptionLegs / 100);

            magicDamage = Mathf.RoundToInt(magicDamage - (magicDamage * totalMagicDamageAbsorption));

            Debug.Log("Total damage absorption is " + totalPhysicalDamageAbsorption + "%");

            float finalDamage = physicalDamage + fireDamage + magicDamage; // + magicDamage etc.

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

        public void CalculateAndSetMaxEquipLoad()
        {
            float totalEquipLoad = 40;
            
            for(int i = 0; i < staminaLevel; i++)
            {
                if(i < 25)
                {
                    totalEquipLoad = totalEquipLoad + 1.2f;
                }
                if(i >= 25 && i <= 50)
                {
                    totalEquipLoad = totalEquipLoad + 1.4f;
                }
                if(i > 50)
                {
                    totalEquipLoad = totalEquipLoad + 1f;
                }
            }

            maxEquipLoad = totalEquipLoad;
        }

        public void CalculateAndSetCurrentEquipLoad(float equipLoad)
        {
            currentEquipLoad = equipLoad;

            encumbraceLevel = EncumbranceLevel.Light;

            if (currentEquipLoad > (maxEquipLoad * 0.3f))
            {
                encumbraceLevel = EncumbranceLevel.Medium;
            }
            if (currentEquipLoad > (maxEquipLoad * 0.7f))
            {
                encumbraceLevel = EncumbranceLevel.Heavy;
            }
            if (currentEquipLoad > maxEquipLoad)
            {
                encumbraceLevel = EncumbranceLevel.Overloaded;
            }
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

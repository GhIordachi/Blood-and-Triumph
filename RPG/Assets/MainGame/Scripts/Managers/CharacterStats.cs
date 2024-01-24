using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI {
    public class CharacterStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public int staminaLevel = 10;
        public float maxStamina;
        public float currentStamina;

        public int focusLevel = 10;
        public float maxFocusPoints;
        public float currentFocusPoints;

        public int soulCount = 0;

        public bool isDead;

        [Header("Armor Absorptions")]
        public float physicalDamageAbsorptionHead;
        public float physicalDamageAbsorptionBody;
        public float physicalDamageAbsorptionLegs;
        public float physicalDamageAbsorptionHands;

        //Fire absorption
        //Lightning absorption
        //Magic absorption
        //Dark absorption

        public virtual void TakeDamage(int physicalDamage, string damageAnimation = "Damage_01")
        {
            if (isDead) 
                return;

            float totalPhysicalDamageAbsorption = 1 - 
                (1 - physicalDamageAbsorptionHead / 100) * 
                (1 - physicalDamageAbsorptionBody / 100) *
                (1 - physicalDamageAbsorptionHands / 100) *
                (1 - physicalDamageAbsorptionLegs / 100);

            physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

            Debug.Log("Total damage absorption is " + totalPhysicalDamageAbsorption + "%");

            float finalDamage = physicalDamage; //+ fireDamage + magicDamage etc.

            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

            Debug.Log("Total damage dealt is " + finalDamage);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }
    }
}

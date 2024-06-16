using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class LevelUpUI : MonoBehaviour
    {
        public PlayerManager playerManager;
        public Button confirmLevelUpButton;

        [Header("Player Level")]
        public int currentPlayerLevel;    //The current level we are before leveling up
        public int projectedPlayerLevel;  //The possible level we will be if we accept leveling up
        public Text currentPlayerLevelText;   //The UI text for the number of the current player level
        public Text projectedPlayerLevelText; //The UI text for the projected player level number

        [Header("Experience")]
        public Text currentXPText;
        public Text XPRequiredToLevelUpText;
        private int XPRequiredToLevelUp;
        public int baseLevelUpCost = 5;

        [Header("Health")]
        public Slider healthSlider;
        public Text currentHealthLevelText;
        public Text projectedHealthLevelText;

        [Header("Stamina")]
        public Slider staminaSlider;
        public Text currentStaminaLevelText;
        public Text projectedStaminaLevelText;

        [Header("Focus")]
        public Slider focusSlider;
        public Text currentFocusLevelText;
        public Text projectedFocusLevelText;

        [Header("Poise")]
        public Slider poiseSlider;
        public Text currentPoiseLevelText;
        public Text projectedPoiseLevelText;

        [Header("Strength")]
        public Slider strengthSlider;
        public Text currentStrengthLevelText;
        public Text projectedStrengthLevelText;

        [Header("Faith")]
        public Slider faithSlider;
        public Text currentFaithLevelText;
        public Text projectedFaithLevelText;

        //Update all of the stats on the UI to the player's current Status
        private void OnEnable()
        {
            currentPlayerLevel = playerManager.playerStatsManager.playerLevel;
            currentPlayerLevelText.text = currentPlayerLevel.ToString();

            projectedPlayerLevel = playerManager.playerStatsManager.playerLevel;
            projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

            healthSlider.value = playerManager.playerStatsManager.healthLevel;
            healthSlider.minValue = playerManager.playerStatsManager.healthLevel;
            healthSlider.maxValue = 99;
            currentHealthLevelText.text = playerManager.playerStatsManager.healthLevel.ToString();
            projectedHealthLevelText.text = playerManager.playerStatsManager.healthLevel.ToString();

            staminaSlider.value = playerManager.playerStatsManager.staminaLevel;
            staminaSlider.minValue = playerManager.playerStatsManager.staminaLevel;
            staminaSlider.maxValue = 99;
            currentStaminaLevelText.text = playerManager.playerStatsManager.staminaLevel.ToString();
            projectedStaminaLevelText.text = playerManager.playerStatsManager.staminaLevel.ToString();

            focusSlider.value = playerManager.playerStatsManager.focusLevel;
            focusSlider.minValue = playerManager.playerStatsManager.focusLevel;
            focusSlider.maxValue = 99;
            currentFocusLevelText.text = playerManager.playerStatsManager.focusLevel.ToString();
            projectedFocusLevelText.text = playerManager.playerStatsManager.focusLevel.ToString();

            poiseSlider.value = playerManager.playerStatsManager.poiseLevel;
            poiseSlider.minValue = playerManager.playerStatsManager.poiseLevel;
            poiseSlider.maxValue = 99;
            currentPoiseLevelText.text = playerManager.playerStatsManager.poiseLevel.ToString();
            projectedPoiseLevelText.text = playerManager.playerStatsManager.poiseLevel.ToString();

            strengthSlider.value = playerManager.playerStatsManager.strengthLevel;
            strengthSlider.minValue = playerManager.playerStatsManager.strengthLevel;
            strengthSlider.maxValue = 99;
            currentStrengthLevelText.text = playerManager.playerStatsManager.strengthLevel.ToString();
            projectedStrengthLevelText.text = playerManager.playerStatsManager.strengthLevel.ToString();

            faithSlider.value = playerManager.playerStatsManager.faithLevel;
            faithSlider.minValue = playerManager.playerStatsManager.faithLevel;
            faithSlider.maxValue = 99;
            currentFaithLevelText.text = playerManager.playerStatsManager.faithLevel.ToString();
            projectedFaithLevelText.text = playerManager.playerStatsManager.faithLevel.ToString();

            currentXPText.text = playerManager.playerStatsManager.currentXPCount.ToString();

            UpdateProjectedPlayerLevel();
        }

        //Updates the player's stats to the projected stats, providing they have enough souls
        public void ConfirmPlayerLevelUpStats()
        {
            playerManager.playerStatsManager.playerLevel = projectedPlayerLevel;
            playerManager.playerStatsManager.healthLevel = Mathf.RoundToInt(healthSlider.value);
            playerManager.playerStatsManager.staminaLevel = Mathf.RoundToInt(staminaSlider.value);
            playerManager.playerStatsManager.focusLevel = Mathf.RoundToInt(focusSlider.value);
            playerManager.playerStatsManager.poiseLevel = Mathf.RoundToInt(poiseSlider.value);
            playerManager.playerStatsManager.strengthLevel = Mathf.RoundToInt(strengthSlider.value);
            playerManager.playerStatsManager.faithLevel = Mathf.RoundToInt(faithSlider.value);

            playerManager.playerStatsManager.maxHealth = playerManager.playerStatsManager.SetMaxHealthFromHealthLevel();
            playerManager.playerStatsManager.maxStamina = playerManager.playerStatsManager.SetMaxStaminaFromStaminaLevel();
            playerManager.playerStatsManager.maxFocusPoints = playerManager.playerStatsManager.SetMaxFocusPointsFromFocusLevel();

            playerManager.playerStatsManager.currentXPCount = playerManager.playerStatsManager.currentXPCount - XPRequiredToLevelUp;
            playerManager.UIManager.xpCount.text = playerManager.playerStatsManager.currentXPCount.ToString();

            playerManager.playerStatsManager.CalculateAndSetMaxEquipLoad();

            gameObject.SetActive(false);
        }

        private void CalculateSoulCostToLevelUp()
        {
            for (int i = 0; i < projectedPlayerLevel; i++)
            {
                XPRequiredToLevelUp = XPRequiredToLevelUp + Mathf.RoundToInt((projectedPlayerLevel * baseLevelUpCost) * 1.5f);
            }
        }

        //Updates the projected player's total level, by adding up all the projected level up stats
        private void UpdateProjectedPlayerLevel()
        {
            XPRequiredToLevelUp = 0;

            projectedPlayerLevel = currentPlayerLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(healthSlider.value) - playerManager.playerStatsManager.healthLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(staminaSlider.value) - playerManager.playerStatsManager.staminaLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(focusSlider.value) - playerManager.playerStatsManager.focusLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(poiseSlider.value) - playerManager.playerStatsManager.poiseLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(strengthSlider.value) - playerManager.playerStatsManager.strengthLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(faithSlider.value) - playerManager.playerStatsManager.faithLevel;
            projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

            CalculateSoulCostToLevelUp();
            XPRequiredToLevelUpText.text = XPRequiredToLevelUp.ToString();

            if (playerManager.playerStatsManager.currentXPCount < XPRequiredToLevelUp)
            {
                confirmLevelUpButton.interactable = false;
            }
            else
            {
                confirmLevelUpButton.interactable = true;
            }
        }

        public void UpdateHealthLevelSlider()
        {
            projectedHealthLevelText.text = healthSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateStaminaLevelSlider()
        {
            projectedStaminaLevelText.text = staminaSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateFocusLevelSlider()
        {
            projectedFocusLevelText.text = focusSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdatePoiseLevelSlider()
        {
            projectedPoiseLevelText.text = poiseSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateStrengthLevelSlider()
        {
            projectedStrengthLevelText.text = strengthSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateFaithLevelSlider()
        {
            projectedFaithLevelText.text = faithSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }
    }
}

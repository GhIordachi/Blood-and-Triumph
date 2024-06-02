using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace GI
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        PlayerManager player;

        public PoisonBuildUpBar poisonBuildUpBar;
        public PoisonAmountBar poisonAmountBar;

        public GameObject currentParticleFX; //The particles that will play of the current effect that is effecting the player (drinking estus, poison etc)
        public int amountToBeHealed;
        public int amountOfFocusPointsToBeHealed;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
            poisonBuildUpBar = FindObjectOfType<PoisonBuildUpBar>();
            poisonAmountBar = FindObjectOfType<PoisonAmountBar>();
        }

        public void HealPlayerFromEffect()
        {
            player.playerStatsManager.HealCharacter(amountToBeHealed);
            player.playerStatsManager.HealFocusPointsCharacter(amountOfFocusPointsToBeHealed);
            GameObject healParticles = Instantiate(currentParticleFX, player.playerStatsManager.transform);
            Destroy(instantiatedFXModel.gameObject);
            player.playerWeaponSlotManager.LoadBothWeaponsOnSlots();
        }

        protected override void ProcessBuildUpDecay()
        {
            if (player.characterStatsManager.poisonBuildup >= 0)
            {
                player.characterStatsManager.poisonBuildup -= 1;

                poisonBuildUpBar.gameObject.SetActive(true);
                poisonBuildUpBar.SetPoisonBuildUpAmount(Mathf.RoundToInt(player.characterStatsManager.poisonBuildup));
            }
        }
    }
}

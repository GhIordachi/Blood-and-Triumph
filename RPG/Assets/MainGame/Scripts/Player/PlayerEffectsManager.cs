using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class PlayerEffectsManager : MonoBehaviour
    {
        PlayerStatsManager playerStatsManager;
        PlayerWeaponSlotManager playerWeaponSlotManager;
        public GameObject currentParticleFX; //The particles that will play of the current effect that is effecting the player (drinking estus, poison etc)
        public GameObject instantiatedFXModel;
        public int amountToBeHealed;

        private void Awake()
        {
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        }

        public void HealPlayerFromEffect()
        {
            playerStatsManager.HealPlayer(amountToBeHealed);
            GameObject healParticles = Instantiate(currentParticleFX, playerStatsManager.transform);
            Destroy(instantiatedFXModel.gameObject);
            playerWeaponSlotManager.LoadBothWeaponsOnSlots();
        }
    }
}

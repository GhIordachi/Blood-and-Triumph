using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class ClassSelector : MonoBehaviour
    {
        PlayerManager player;

        [Header("Class Info UI")]
        public Text healthStat;
        public Text staminaStat;
        public Text focusStat;
        public Text poiseStat;
        public Text strengthStat;
        public Text dexterityStat;
        public Text intelligenceStat;
        public Text faithStat;
        public Text classDescription;

        [Header("Class Starting Stats")]
        public ClassStats[] classStats;

        [Header("Class Starting Gear")]
        public ClassGear[] classGears;

        //Set the stats and gear for each class

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }

        private void AssignClassStats(int classChosen)
        {
            player.playerStatsManager.healthLevel = classStats[classChosen].healthLevel;
            player.playerStatsManager.staminaLevel = classStats[classChosen].staminaLevel;
            player.playerStatsManager.focusLevel = classStats[classChosen].focusLevel;
            player.playerStatsManager.poiseLevel = classStats[classChosen].poiseLevel;
            player.playerStatsManager.strengthLevel = classStats[classChosen].strengthLevel;
            player.playerStatsManager.faithLevel = classStats[classChosen].faithLevel;

            classDescription.text = classStats[classChosen].classDescription;
        }

        public void AssignKnightClass()
        {
            AssignClassStats(0);
            player.playerInventoryManager.currentHelmetEquipment = classGears[0].headEquipment;
            player.playerInventoryManager.currentBodyEquipment = classGears[0].bodyEquipment;
            player.playerInventoryManager.currentLegEquipment = classGears[0].legEquipment;
            player.playerInventoryManager.currentHandEquipment = classGears[0].handEquipment;

            player.playerInventoryManager.weaponsInRightHandSlots[0] = classGears[0].primaryWeapon;
            player.playerInventoryManager.weaponsInLeftHandSlots[0] = classGears[0].offHandWeapon;
            player.playerInventoryManager.currentAmmo = classGears[0].ammoItem;

            player.playerEquipmentManager.EquipAllArmor();
            player.playerWeaponSlotManager.LoadBothWeaponsOnSlots();

            healthStat.text = player.playerStatsManager.healthLevel.ToString();
            staminaStat.text = player.playerStatsManager.staminaLevel.ToString();
            focusStat.text = player.playerStatsManager.focusLevel.ToString();
            poiseStat.text = player.playerStatsManager.poiseLevel.ToString();
            strengthStat.text = player.playerStatsManager.strengthLevel.ToString();
            faithStat.text = player.playerStatsManager.faithLevel.ToString();
        }

        public void AssignHunterClass()
        {
            AssignClassStats(1);
            player.playerInventoryManager.currentHelmetEquipment = classGears[1].headEquipment;
            player.playerInventoryManager.currentBodyEquipment = classGears[1].bodyEquipment;
            player.playerInventoryManager.currentLegEquipment = classGears[1].legEquipment;
            player.playerInventoryManager.currentHandEquipment = classGears[1].handEquipment;

            player.playerInventoryManager.weaponsInRightHandSlots[0] = classGears[1].primaryWeapon;
            player.playerInventoryManager.weaponsInLeftHandSlots[0] = classGears[1].offHandWeapon;
            player.playerInventoryManager.currentAmmo = classGears[1].ammoItem;

            player.playerEquipmentManager.EquipAllArmor();
            player.playerWeaponSlotManager.LoadBothWeaponsOnSlots();

            healthStat.text = player.playerStatsManager.healthLevel.ToString();
            staminaStat.text = player.playerStatsManager.staminaLevel.ToString();
            focusStat.text = player.playerStatsManager.focusLevel.ToString();
            poiseStat.text = player.playerStatsManager.poiseLevel.ToString();
            strengthStat.text = player.playerStatsManager.strengthLevel.ToString();
            faithStat.text = player.playerStatsManager.faithLevel.ToString();
        }

        public void AssignMageClass()
        {
            AssignClassStats(2);
            player.playerInventoryManager.currentHelmetEquipment = classGears[2].headEquipment;
            player.playerInventoryManager.currentBodyEquipment = classGears[2].bodyEquipment;
            player.playerInventoryManager.currentLegEquipment = classGears[2].legEquipment;
            player.playerInventoryManager.currentHandEquipment = classGears[2].handEquipment;

            player.playerInventoryManager.weaponsInRightHandSlots[0] = classGears[2].primaryWeapon;
            player.playerInventoryManager.weaponsInLeftHandSlots[0] = classGears[2].offHandWeapon;
            player.playerInventoryManager.currentAmmo = classGears[2].ammoItem;

            player.playerEquipmentManager.EquipAllArmor();
            player.playerWeaponSlotManager.LoadBothWeaponsOnSlots();

            healthStat.text = player.playerStatsManager.healthLevel.ToString();
            staminaStat.text = player.playerStatsManager.staminaLevel.ToString();
            focusStat.text = player.playerStatsManager.focusLevel.ToString();
            poiseStat.text = player.playerStatsManager.poiseLevel.ToString();
            strengthStat.text = player.playerStatsManager.strengthLevel.ToString();
            faithStat.text = player.playerStatsManager.faithLevel.ToString();
        }

        public void AssignGuardianClass()
        {
            AssignClassStats(3);
            player.playerInventoryManager.currentHelmetEquipment = classGears[3].headEquipment;
            player.playerInventoryManager.currentBodyEquipment = classGears[3].bodyEquipment;
            player.playerInventoryManager.currentLegEquipment = classGears[3].legEquipment;
            player.playerInventoryManager.currentHandEquipment = classGears[3].handEquipment;

            player.playerInventoryManager.weaponsInRightHandSlots[0] = classGears[3].primaryWeapon;
            player.playerInventoryManager.weaponsInLeftHandSlots[0] = classGears[3].offHandWeapon;
            player.playerInventoryManager.currentAmmo = classGears[3].ammoItem;

            player.playerEquipmentManager.EquipAllArmor();
            player.playerWeaponSlotManager.LoadBothWeaponsOnSlots();

            healthStat.text = player.playerStatsManager.healthLevel.ToString();
            staminaStat.text = player.playerStatsManager.staminaLevel.ToString();
            focusStat.text = player.playerStatsManager.focusLevel.ToString();
            poiseStat.text = player.playerStatsManager.poiseLevel.ToString();
            strengthStat.text = player.playerStatsManager.strengthLevel.ToString();
            faithStat.text = player.playerStatsManager.faithLevel.ToString();
        }
    }
}

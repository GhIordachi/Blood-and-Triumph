using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        PlayerManager player;

        [Header("Equipment Model Changers")]
        //Head Equipment
        HelmetModelChanger helmetModelChanger;
        //Torso Equipment
        TorsoModelChanger torsoModelChanger;
        UpperRightArmModelChanger upperRightArmModelChanger;
        UpperLeftArmModelChanger upperLeftArmModelChanger;
        //Leg Equipment
        HipModelChanger hipModelChanger;
        LeftLegModelChanger leftLegModelChanger;
        RightLegModelChanger rightLegModelChanger;
        //Arm Equipment
        LowerRightArmModelChanger lowerRightArmModelChanger;
        LowerLeftArmModelChanger lowerLeftArmModelChanger;
        LeftHandModelChanger leftHandModelChanger;
        RightHandModelChanger rightHandModelChanger;

        [Header("Facial Features")]
        public GameObject[] hairs;
        public GameObject[] eyebrows;
        public GameObject[] beards;

        [Header("Default Naked Models")]
        public GameObject nakedHeadModel;
        public string nakedTorsoModel;
        public string nakedUpperLeftArm;
        public string nakedUpperRightArm;
        public string nakedLowerLeftArm;
        public string nakedLowerRightArm;
        public string nakedLeftArm;
        public string nakedRightArm;
        public string nakedHipModel;
        public string nakedLeftLeg;
        public string nakedRightLeg;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();

            helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
            torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
            hipModelChanger = GetComponentInChildren<HipModelChanger>();
            leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
            rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();
            upperRightArmModelChanger = GetComponentInChildren<UpperRightArmModelChanger>();
            upperLeftArmModelChanger = GetComponentInChildren<UpperLeftArmModelChanger>();
            lowerRightArmModelChanger = GetComponentInChildren<LowerRightArmModelChanger>();
            lowerLeftArmModelChanger = GetComponentInChildren<LowerLeftArmModelChanger>();
            leftHandModelChanger = GetComponentInChildren<LeftHandModelChanger>();
            rightHandModelChanger = GetComponentInChildren<RightHandModelChanger>();
        }

        private void Start()
        {
            EquipAllArmor();
        }

        public void EquipAllArmor()
        {
            float poisonResistance = 0;
            float totalEquipmentLoad = 0;

            //Helmet Equipment
            if (helmetModelChanger != null)
            {                
                helmetModelChanger.UnEquipAllHelmetModels();       
                
                if(player.playerInventoryManager.currentHelmetEquipment != null)
                {
                    if(player.playerInventoryManager.currentHelmetEquipment.hideHair)
                    {
                        foreach (var feature in hairs)
                        {
                            feature.SetActive(false);
                        }
                    }

                    if (player.playerInventoryManager.currentHelmetEquipment.hideEyebrows)
                    {
                        foreach (var feature in eyebrows)
                        {
                            feature.SetActive(false);
                        }
                    }

                    if (player.playerInventoryManager.currentHelmetEquipment.hideBeard)
                    {
                        foreach (var feature in beards)
                        {
                            feature.SetActive(false);
                        }
                    }

                    nakedHeadModel.SetActive(false);
                    helmetModelChanger.EquipHelmetModelByName(player.playerInventoryManager.currentHelmetEquipment.helmetModelName);
                    player.playerStatsManager.physicalDamageAbsorptionHead = player.playerInventoryManager.currentHelmetEquipment.physicalDefense;
                    player.playerStatsManager.fireDamageAbsorptionHead = player.playerInventoryManager.currentHelmetEquipment.fireDefense;
                    poisonResistance += player.playerInventoryManager.currentHelmetEquipment.poisonResistance;
                    totalEquipmentLoad += player.playerInventoryManager.currentHelmetEquipment.weight;
                }
                else
                {
                    nakedHeadModel.SetActive(true);
                    player.playerStatsManager.physicalDamageAbsorptionHead = 0;

                    foreach (var feature in hairs)
                    {
                        feature.SetActive(true);
                    }

                    foreach (var feature in eyebrows)
                    {
                        feature.SetActive(true);
                    }

                    foreach (var feature in beards)
                    {
                        feature.SetActive(true);
                    }
                }
            }
            //Torso Equipment
            if (torsoModelChanger != null)
            {
                torsoModelChanger.UnEquipAllTorsoModels();
                upperLeftArmModelChanger.UnEquipAllArmModels();
                upperRightArmModelChanger.UnEquipAllArmModels();

                if (player.playerInventoryManager.currentBodyEquipment != null)
                {
                    torsoModelChanger.EquipTorsoModelByName(player.playerInventoryManager.currentBodyEquipment.torsoModelName);
                    upperLeftArmModelChanger.EquipArmModelByName(player.playerInventoryManager.currentBodyEquipment.upperLeftArmModelName);
                    upperRightArmModelChanger.EquipArmModelByName(player.playerInventoryManager.currentBodyEquipment.upperRightArmModelName);
                    player.playerStatsManager.physicalDamageAbsorptionBody = player.playerInventoryManager.currentBodyEquipment.physicalDefense;
                    player.playerStatsManager.fireDamageAbsorptionBody = player.playerInventoryManager.currentBodyEquipment.fireDefense;
                    poisonResistance += player.playerInventoryManager.currentBodyEquipment.poisonResistance;
                    totalEquipmentLoad += player.playerInventoryManager.currentBodyEquipment.weight;
                }
                else
                {
                    torsoModelChanger.EquipTorsoModelByName(nakedTorsoModel);
                    upperLeftArmModelChanger.EquipArmModelByName(nakedUpperLeftArm);
                    upperRightArmModelChanger.EquipArmModelByName(nakedUpperRightArm);
                    player.playerStatsManager.physicalDamageAbsorptionBody = 0;
                }
            }
            //Leg Equipment
            if(hipModelChanger != null)
            {
                hipModelChanger.UnEquipAllHipModels();
                leftLegModelChanger.UnEquipAllLegModels();
                rightLegModelChanger.UnEquipAllLegModels();

                if (player.playerInventoryManager.currentLegEquipment != null)
                {
                    hipModelChanger.EquipHipModelByName(player.playerInventoryManager.currentLegEquipment.hipModelName);
                    leftLegModelChanger.EquipLegModelByName(player.playerInventoryManager.currentLegEquipment.leftLegName);
                    rightLegModelChanger.EquipLegModelByName(player.playerInventoryManager.currentLegEquipment.rightLegName);
                    player.playerStatsManager.physicalDamageAbsorptionLegs = player.playerInventoryManager.currentLegEquipment.physicalDefense;
                    player.playerStatsManager.fireDamageAbsorptionLegs = player.playerInventoryManager.currentLegEquipment.fireDefense;
                    poisonResistance += player.playerInventoryManager.currentLegEquipment.poisonResistance;
                    totalEquipmentLoad += player.playerInventoryManager.currentLegEquipment.weight;
                }
                else
                {
                    hipModelChanger.EquipHipModelByName(nakedHipModel);
                    leftLegModelChanger.EquipLegModelByName(nakedLeftLeg);
                    rightLegModelChanger.EquipLegModelByName(nakedRightLeg);
                    player.playerStatsManager.physicalDamageAbsorptionLegs = 0;
                }
            }
            //Hand Equipment
            if (leftHandModelChanger != null && rightHandModelChanger != null)
            {
                lowerLeftArmModelChanger.UnEquipAllArmModels();
                lowerRightArmModelChanger.UnEquipAllArmModels();
                leftHandModelChanger.UnEquipAllArmModels();
                rightHandModelChanger.UnEquipAllArmModels();

                if (player.playerInventoryManager.currentHandEquipment != null)
                {
                    lowerLeftArmModelChanger.EquipArmModelByName(player.playerInventoryManager.currentHandEquipment.lowerLeftArmModelName);
                    lowerRightArmModelChanger.EquipArmModelByName(player.playerInventoryManager.currentHandEquipment.lowerRightArmModelName);
                    leftHandModelChanger.EquipArmModelByName(player.playerInventoryManager.currentHandEquipment.leftHandModelName);
                    rightHandModelChanger.EquipArmModelByName(player.playerInventoryManager.currentHandEquipment.rightHandModelName);
                    player.playerStatsManager.physicalDamageAbsorptionHands = player.playerInventoryManager.currentHandEquipment.physicalDefense;
                    player.playerStatsManager.fireDamageAbsorptionHands = player.playerInventoryManager.currentHandEquipment.fireDefense;
                    poisonResistance += player.playerInventoryManager.currentHandEquipment.poisonResistance;
                    totalEquipmentLoad += player.playerInventoryManager.currentHandEquipment.weight;
                }
                else
                {
                    lowerLeftArmModelChanger.EquipArmModelByName(nakedLowerLeftArm);
                    lowerRightArmModelChanger.EquipArmModelByName(nakedLowerRightArm);
                    leftHandModelChanger.EquipArmModelByName(nakedLeftArm);
                    rightHandModelChanger.EquipArmModelByName(nakedRightArm);
                    player.playerStatsManager.physicalDamageAbsorptionHands = 0;
                }
            }

            player.playerStatsManager.poisonResistance = poisonResistance;
            player.playerStatsManager.CalculateAndSetCurrentEquipLoad(totalEquipmentLoad);
        }
    }
}

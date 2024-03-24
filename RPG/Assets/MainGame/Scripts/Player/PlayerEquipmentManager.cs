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
        HelmetAttachmentModelChanger helmetAttachmentModelChanger;
        //Torso Equipment
        TorsoModelChanger torsoModelChanger;
        UpperRightArmModelChanger upperRightArmModelChanger;
        UpperLeftArmModelChanger upperLeftArmModelChanger;
        BackAttachmentModelChanger backAttachmentModelChanger;
        RightShoulderModelChanger rightShoulderModelChanger;
        LeftShoulderModelChanger leftShoulderModelChanger;
        //Leg Equipment
        HipModelChanger hipModelChanger;
        LeftLegModelChanger leftLegModelChanger;
        RightLegModelChanger rightLegModelChanger;
        LeftKneePadModelChanger leftKneePadModelChanger;
        RightKneePadModelChanger rightKneePadModelChanger;
        //Arm Equipment
        LowerRightArmModelChanger lowerRightArmModelChanger;
        LowerLeftArmModelChanger lowerLeftArmModelChanger;
        LeftHandModelChanger leftHandModelChanger;
        RightHandModelChanger rightHandModelChanger;
        LeftElbowModelChanger leftElbowModelChanger;
        RightElbowModelChanger rightElbowModelChanger;

        [Header("Facial Features")]
        EyebrowSelector eyebrowSelector;
        HairSelector hairSelector;
        BeardSelector beardSelector;
        ColorFacialFeatures colorFacialFeatures;
        public GameObject hair;
        public GameObject eyebrows;
        public GameObject beard;
        public int hairID = -1;
        public int eyebrowID = -1;
        public int beardID = -1;
        public Color hairColor;

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
            backAttachmentModelChanger = GetComponentInChildren<BackAttachmentModelChanger>();
            rightShoulderModelChanger = GetComponentInChildren<RightShoulderModelChanger>();
            leftShoulderModelChanger = GetComponentInChildren<LeftShoulderModelChanger>();
            helmetAttachmentModelChanger = GetComponentInChildren<HelmetAttachmentModelChanger>();
            leftKneePadModelChanger = GetComponentInChildren<LeftKneePadModelChanger>();
            rightKneePadModelChanger = GetComponentInChildren<RightKneePadModelChanger>();
            leftElbowModelChanger = GetComponentInChildren<LeftElbowModelChanger>();
            rightElbowModelChanger = GetComponentInChildren<RightElbowModelChanger>();

            hairSelector = GetComponentInChildren<HairSelector>();
            eyebrowSelector = GetComponentInChildren<EyebrowSelector>();
            beardSelector = GetComponentInChildren<BeardSelector>();
            colorFacialFeatures = GetComponentInChildren<ColorFacialFeatures>();
        }

        private void Start()
        {
            EquipAllArmor();
        }

        public void EquipAllArmor()
        {
            float poisonResistance = 0;
            float totalEquipmentLoad = 0;

            //Initialises Facial Features
            hairSelector.GetHairByID(hairID);
            eyebrowSelector.GetEyebrowByID(eyebrowID);
            beardSelector.GetBeardByID(beardID);
            colorFacialFeatures.SetHairColor();

            //Helmet Equipment
            if (helmetModelChanger != null)
            {                
                helmetModelChanger.UnEquipAllHelmetModels();
                helmetAttachmentModelChanger.UnEquipAllHelmetAttachmentModels();
                
                if(player.playerInventoryManager.currentHelmetEquipment != null)
                {                    
                    if (player.playerInventoryManager.currentHelmetEquipment.hideHair)
                    {
                        if(hair != null)
                           hair.SetActive(false);
                    }
                    else
                    {
                        if (hair != null)
                            hair.SetActive(true);
                    }

                    if (player.playerInventoryManager.currentHelmetEquipment.hideEyebrows)
                    {
                        if (eyebrows != null)
                            eyebrows.SetActive(false);
                    }
                    else
                    {
                        if (eyebrows != null)
                            eyebrows.SetActive(true);
                    }

                    if (player.playerInventoryManager.currentHelmetEquipment.hideBeard)
                    {
                        if (beard != null)
                            beard.SetActive(false);
                    }
                    else
                    {
                        if (beard != null)
                            beard.SetActive(true);
                    }

                    if(!player.playerInventoryManager.currentHelmetEquipment.hideBeard ||
                        !player.playerInventoryManager.currentHelmetEquipment.hideEyebrows ||
                        !player.playerInventoryManager.currentHelmetEquipment.hideHair)
                    {
                        nakedHeadModel.SetActive(true);
                    }
                    else
                    {
                        nakedHeadModel.SetActive(false);
                    }
                   
                    helmetModelChanger.EquipHelmetModelByName(player.playerInventoryManager.currentHelmetEquipment.helmetModelName);
                    helmetAttachmentModelChanger.EquipHelmetAttachmentModelByName(player.playerInventoryManager.currentHelmetEquipment.helmetAttachmentModelName);
                    player.playerStatsManager.physicalDamageAbsorptionHead = player.playerInventoryManager.currentHelmetEquipment.physicalDefense;
                    player.playerStatsManager.fireDamageAbsorptionHead = player.playerInventoryManager.currentHelmetEquipment.fireDefense;
                    player.playerStatsManager.magicDamageAbsorptionHead = player.playerInventoryManager.currentHelmetEquipment.magicDefense;
                    poisonResistance += player.playerInventoryManager.currentHelmetEquipment.poisonResistance;
                    totalEquipmentLoad += player.playerInventoryManager.currentHelmetEquipment.weight;
                }
                else
                {                    
                    nakedHeadModel.SetActive(true);
                    player.playerStatsManager.physicalDamageAbsorptionHead = 0;
                    player.playerStatsManager.fireDamageAbsorptionHead = 0;
                    player.playerStatsManager.magicDamageAbsorptionHead = 0;

                    if (hair != null)
                        hair.SetActive(true);
                    if (eyebrows != null)
                        eyebrows.SetActive(true);
                    if (beard != null)
                        beard.SetActive(true);
                }
            }
            //Torso Equipment
            if (torsoModelChanger != null)
            {
                torsoModelChanger.UnEquipAllTorsoModels();
                upperLeftArmModelChanger.UnEquipAllArmModels();
                upperRightArmModelChanger.UnEquipAllArmModels();
                backAttachmentModelChanger.UnEquipAllBackAttachmentModels();
                rightShoulderModelChanger.UnEquipAllRightShoulderModels();
                leftShoulderModelChanger.UnEquipAllLeftShoulderModels();

                if (player.playerInventoryManager.currentBodyEquipment != null)
                {
                    torsoModelChanger.EquipTorsoModelByName(player.playerInventoryManager.currentBodyEquipment.torsoModelName);
                    upperLeftArmModelChanger.EquipArmModelByName(player.playerInventoryManager.currentBodyEquipment.upperLeftArmModelName);
                    upperRightArmModelChanger.EquipArmModelByName(player.playerInventoryManager.currentBodyEquipment.upperRightArmModelName);
                    backAttachmentModelChanger.EquipBackAttachmentModelByName(player.playerInventoryManager.currentBodyEquipment.backAttachmentModelName);
                    rightShoulderModelChanger.EquipRightShoulderModelByName(player.playerInventoryManager.currentBodyEquipment.shoulderRightAttachmentModelName);
                    leftShoulderModelChanger.EquipLeftShoulderModelByName(player.playerInventoryManager.currentBodyEquipment.shoulderLeftAttachmentModelName);
                    player.playerStatsManager.physicalDamageAbsorptionBody = player.playerInventoryManager.currentBodyEquipment.physicalDefense;
                    player.playerStatsManager.fireDamageAbsorptionBody = player.playerInventoryManager.currentBodyEquipment.fireDefense;
                    player.playerStatsManager.magicDamageAbsorptionBody = player.playerInventoryManager.currentBodyEquipment.magicDefense;
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
                leftKneePadModelChanger.UnEquipAllKneePadModels();
                rightKneePadModelChanger.UnEquipAllKneePadModels();

                if (player.playerInventoryManager.currentLegEquipment != null)
                {
                    hipModelChanger.EquipHipModelByName(player.playerInventoryManager.currentLegEquipment.hipModelName);
                    leftLegModelChanger.EquipLegModelByName(player.playerInventoryManager.currentLegEquipment.leftLegName);
                    rightLegModelChanger.EquipLegModelByName(player.playerInventoryManager.currentLegEquipment.rightLegName);
                    leftKneePadModelChanger.EquipKneePadModelByName(player.playerInventoryManager.currentLegEquipment.leftKneePadName);
                    rightKneePadModelChanger.EquipKneePadModelByName(player.playerInventoryManager.currentLegEquipment.rightKneePadName);
                    player.playerStatsManager.physicalDamageAbsorptionLegs = player.playerInventoryManager.currentLegEquipment.physicalDefense;
                    player.playerStatsManager.fireDamageAbsorptionLegs = player.playerInventoryManager.currentLegEquipment.fireDefense;
                    player.playerStatsManager.magicDamageAbsorptionLegs = player.playerInventoryManager.currentLegEquipment.magicDefense;
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
                leftElbowModelChanger.UnEquipAllElbowModels();
                rightElbowModelChanger.UnEquipAllElbowModels();

                if (player.playerInventoryManager.currentHandEquipment != null)
                {
                    lowerLeftArmModelChanger.EquipArmModelByName(player.playerInventoryManager.currentHandEquipment.lowerLeftArmModelName);
                    lowerRightArmModelChanger.EquipArmModelByName(player.playerInventoryManager.currentHandEquipment.lowerRightArmModelName);
                    leftHandModelChanger.EquipArmModelByName(player.playerInventoryManager.currentHandEquipment.leftHandModelName);
                    rightHandModelChanger.EquipArmModelByName(player.playerInventoryManager.currentHandEquipment.rightHandModelName);
                    leftElbowModelChanger.EquipElbowModelByName(player.playerInventoryManager.currentHandEquipment.leftElbowModelName);
                    rightElbowModelChanger.EquipElbowModelByName(player.playerInventoryManager.currentHandEquipment.rightElbowModelName);
                    player.playerStatsManager.physicalDamageAbsorptionHands = player.playerInventoryManager.currentHandEquipment.physicalDefense;
                    player.playerStatsManager.fireDamageAbsorptionHands = player.playerInventoryManager.currentHandEquipment.fireDefense;
                    player.playerStatsManager.magicDamageAbsorptionHands = player.playerInventoryManager.currentHandEquipment.magicDefense;
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

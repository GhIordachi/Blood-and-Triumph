using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerInventory playerInventory;
        PlayerStats playerStats;

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

        public BlockingCollider blockingCollider;

        private void Awake()
        {
            inputHandler = GetComponentInParent<InputHandler>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            playerStats = GetComponentInParent<PlayerStats>();
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
            EquipAllEquipmentModelsOnStart();
        }

        private void EquipAllEquipmentModelsOnStart()
        {
            //Helmet Equipment
            if (helmetModelChanger != null)
            {                
                helmetModelChanger.UnEquipAllHelmetModels();
                if(playerInventory.currentHelmetEquipment != null)
                {
                    nakedHeadModel.SetActive(false);
                    helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmetEquipment.helmetModelName);
                    playerStats.physicalDamageAbsorptionHead = playerInventory.currentHelmetEquipment.physicalDefense;
                    Debug.Log("Head absorption is " + playerStats.physicalDamageAbsorptionHead + "%");
                }
                else
                {
                    nakedHeadModel.SetActive(true);
                    playerStats.physicalDamageAbsorptionHead = 0;
                }
            }
            //Torso Equipment
            if (torsoModelChanger != null)
            {
                torsoModelChanger.UnEquipAllTorsoModels();
                upperLeftArmModelChanger.UnEquipAllArmModels();
                upperRightArmModelChanger.UnEquipAllArmModels();
                if(playerInventory.currentTorsoEquipment != null)
                {
                    torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelName);
                    upperLeftArmModelChanger.EquipArmModelByName(playerInventory.currentTorsoEquipment.upperLeftArmModelName);
                    upperRightArmModelChanger.EquipArmModelByName(playerInventory.currentTorsoEquipment.upperRightArmModelName);
                    playerStats.physicalDamageAbsorptionBody = playerInventory.currentTorsoEquipment.physicalDefense;
                    Debug.Log("Body absorption is " + playerStats.physicalDamageAbsorptionBody + "%");
                }
                else
                {
                    torsoModelChanger.EquipTorsoModelByName(nakedTorsoModel);
                    upperLeftArmModelChanger.EquipArmModelByName(nakedUpperLeftArm);
                    upperRightArmModelChanger.EquipArmModelByName(nakedUpperRightArm);
                    playerStats.physicalDamageAbsorptionBody = 0;
                }
            }
            //Leg Equipment
            if(hipModelChanger != null)
            {
                hipModelChanger.UnEquipAllHipModels();
                leftLegModelChanger.UnEquipAllLegModels();
                rightLegModelChanger.UnEquipAllLegModels();

                if(playerInventory.currentLegEquipment != null)
                {
                    hipModelChanger.EquipHipModelByName(playerInventory.currentLegEquipment.hipModelName);
                    leftLegModelChanger.EquipLegModelByName(playerInventory.currentLegEquipment.leftLegName);
                    rightLegModelChanger.EquipLegModelByName(playerInventory.currentLegEquipment.rightLegName);
                    playerStats.physicalDamageAbsorptionLegs = playerInventory.currentLegEquipment.physicalDefense;
                    Debug.Log("Legs absorption is " + playerStats.physicalDamageAbsorptionLegs + "%");
                }
                else
                {
                    hipModelChanger.EquipHipModelByName(nakedHipModel);
                    leftLegModelChanger.EquipLegModelByName(nakedLeftLeg);
                    rightLegModelChanger.EquipLegModelByName(nakedRightLeg);
                    playerStats.physicalDamageAbsorptionLegs = 0;
                }
            }
            //Hand Equipment
            if (leftHandModelChanger != null && rightHandModelChanger != null)
            {
                lowerLeftArmModelChanger.UnEquipAllArmModels();
                lowerRightArmModelChanger.UnEquipAllArmModels();
                leftHandModelChanger.UnEquipAllArmModels();
                rightHandModelChanger.UnEquipAllArmModels();

                if (playerInventory.currentHandEquipment != null)
                {
                    lowerLeftArmModelChanger.EquipArmModelByName(playerInventory.currentHandEquipment.lowerLeftArmModelName);
                    lowerRightArmModelChanger.EquipArmModelByName(playerInventory.currentHandEquipment.lowerRightArmModelName);
                    leftHandModelChanger.EquipArmModelByName(playerInventory.currentHandEquipment.leftHandModelName);
                    rightHandModelChanger.EquipArmModelByName(playerInventory.currentHandEquipment.rightHandModelName);
                    playerStats.physicalDamageAbsorptionHands = playerInventory.currentHandEquipment.physicalDefense;
                    Debug.Log("Hands absorption is " + playerStats.physicalDamageAbsorptionHands + "%");
                }
                else
                {
                    lowerLeftArmModelChanger.EquipArmModelByName(nakedLowerLeftArm);
                    lowerRightArmModelChanger.EquipArmModelByName(nakedLowerRightArm);
                    leftHandModelChanger.EquipArmModelByName(nakedLeftArm);
                    rightHandModelChanger.EquipArmModelByName(nakedRightArm);
                    playerStats.physicalDamageAbsorptionHands = 0;
                }
            }
        }

        public void OpenBlockingCollider()
        {
            if(inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventory.rightWeapon);
            }
            else
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventory.leftWeapon);
            }
            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}

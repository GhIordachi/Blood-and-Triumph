using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerInventoryManager playerInventoryManager;
        PlayerStatsManager playerStatsManager;

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
            inputHandler = GetComponent<InputHandler>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();

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
                if(playerInventoryManager.currentHelmetEquipment != null)
                {
                    nakedHeadModel.SetActive(false);
                    helmetModelChanger.EquipHelmetModelByName(playerInventoryManager.currentHelmetEquipment.helmetModelName);
                    playerStatsManager.physicalDamageAbsorptionHead = playerInventoryManager.currentHelmetEquipment.physicalDefense;
                    playerStatsManager.fireDamageAbsorptionHead = playerInventoryManager.currentHelmetEquipment.fireDefense;
                    Debug.Log("Head absorption is " + playerStatsManager.physicalDamageAbsorptionHead + "%");
                }
                else
                {
                    nakedHeadModel.SetActive(true);
                    playerStatsManager.physicalDamageAbsorptionHead = 0;
                }
            }
            //Torso Equipment
            if (torsoModelChanger != null)
            {
                torsoModelChanger.UnEquipAllTorsoModels();
                upperLeftArmModelChanger.UnEquipAllArmModels();
                upperRightArmModelChanger.UnEquipAllArmModels();
                if(playerInventoryManager.currentTorsoEquipment != null)
                {
                    torsoModelChanger.EquipTorsoModelByName(playerInventoryManager.currentTorsoEquipment.torsoModelName);
                    upperLeftArmModelChanger.EquipArmModelByName(playerInventoryManager.currentTorsoEquipment.upperLeftArmModelName);
                    upperRightArmModelChanger.EquipArmModelByName(playerInventoryManager.currentTorsoEquipment.upperRightArmModelName);
                    playerStatsManager.physicalDamageAbsorptionBody = playerInventoryManager.currentTorsoEquipment.physicalDefense;
                    playerStatsManager.fireDamageAbsorptionBody = playerInventoryManager.currentTorsoEquipment.fireDefense;
                    Debug.Log("Body absorption is " + playerStatsManager.physicalDamageAbsorptionBody + "%");
                }
                else
                {
                    torsoModelChanger.EquipTorsoModelByName(nakedTorsoModel);
                    upperLeftArmModelChanger.EquipArmModelByName(nakedUpperLeftArm);
                    upperRightArmModelChanger.EquipArmModelByName(nakedUpperRightArm);
                    playerStatsManager.physicalDamageAbsorptionBody = 0;
                }
            }
            //Leg Equipment
            if(hipModelChanger != null)
            {
                hipModelChanger.UnEquipAllHipModels();
                leftLegModelChanger.UnEquipAllLegModels();
                rightLegModelChanger.UnEquipAllLegModels();

                if(playerInventoryManager.currentLegEquipment != null)
                {
                    hipModelChanger.EquipHipModelByName(playerInventoryManager.currentLegEquipment.hipModelName);
                    leftLegModelChanger.EquipLegModelByName(playerInventoryManager.currentLegEquipment.leftLegName);
                    rightLegModelChanger.EquipLegModelByName(playerInventoryManager.currentLegEquipment.rightLegName);
                    playerStatsManager.physicalDamageAbsorptionLegs = playerInventoryManager.currentLegEquipment.physicalDefense;
                    playerStatsManager.fireDamageAbsorptionLegs = playerInventoryManager.currentLegEquipment.fireDefense;
                    Debug.Log("Legs absorption is " + playerStatsManager.physicalDamageAbsorptionLegs + "%");
                }
                else
                {
                    hipModelChanger.EquipHipModelByName(nakedHipModel);
                    leftLegModelChanger.EquipLegModelByName(nakedLeftLeg);
                    rightLegModelChanger.EquipLegModelByName(nakedRightLeg);
                    playerStatsManager.physicalDamageAbsorptionLegs = 0;
                }
            }
            //Hand Equipment
            if (leftHandModelChanger != null && rightHandModelChanger != null)
            {
                lowerLeftArmModelChanger.UnEquipAllArmModels();
                lowerRightArmModelChanger.UnEquipAllArmModels();
                leftHandModelChanger.UnEquipAllArmModels();
                rightHandModelChanger.UnEquipAllArmModels();

                if (playerInventoryManager.currentHandEquipment != null)
                {
                    lowerLeftArmModelChanger.EquipArmModelByName(playerInventoryManager.currentHandEquipment.lowerLeftArmModelName);
                    lowerRightArmModelChanger.EquipArmModelByName(playerInventoryManager.currentHandEquipment.lowerRightArmModelName);
                    leftHandModelChanger.EquipArmModelByName(playerInventoryManager.currentHandEquipment.leftHandModelName);
                    rightHandModelChanger.EquipArmModelByName(playerInventoryManager.currentHandEquipment.rightHandModelName);
                    playerStatsManager.physicalDamageAbsorptionHands = playerInventoryManager.currentHandEquipment.physicalDefense;
                    playerStatsManager.fireDamageAbsorptionHands = playerInventoryManager.currentHandEquipment.fireDefense;
                    Debug.Log("Hands absorption is " + playerStatsManager.physicalDamageAbsorptionHands + "%");
                }
                else
                {
                    lowerLeftArmModelChanger.EquipArmModelByName(nakedLowerLeftArm);
                    lowerRightArmModelChanger.EquipArmModelByName(nakedLowerRightArm);
                    leftHandModelChanger.EquipArmModelByName(nakedLeftArm);
                    rightHandModelChanger.EquipArmModelByName(nakedRightArm);
                    playerStatsManager.physicalDamageAbsorptionHands = 0;
                }
            }
        }

        public void OpenBlockingCollider()
        {
            if(inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.rightWeapon);
            }
            else
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.leftWeapon);
            }
            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class QuestManager : MonoBehaviour
    {
        PlayerManager playerManager;
        ArenaManager arenaManager;

        [Header("Quests Completed")]
        public bool quest1 = false;
        public bool quest2 = false;
        public bool quest3 = false;
        public bool quest4 = false;
        public bool quest5 = false;
        public bool quest6 = false;
        public bool quest7 = false;
        public int lastQuestFinished = 0;

        [Header("Quest Holders")]
        public QuestOneInteractable questHolder1;
        public QuestTwoInteractable questHolder2;
        public QuestThreeInteractable questHolder3;
        public QuestFourInteractable questHolder4;
        public QuestFiveInteractable questHolder5;
        public QuestSixInteractable questHolder6;
        public QuestSevenInteractable questHolder7;

        private void Awake()
        {
            playerManager = FindObjectOfType<PlayerManager>();
            arenaManager = FindObjectOfType<ArenaManager>();
        }

        void UpdateQuestsColliders()
        {
            if (quest1 && !quest2)
            {
                CapsuleCollider questScript = questHolder2.GetComponent<CapsuleCollider>();

                if (!questScript.enabled)
                {
                    questScript.enabled = true;
                }
            }
            if (quest2 && !quest3)
            {
                CapsuleCollider questScript = questHolder3.GetComponent<CapsuleCollider>();

                if (!questScript.enabled)
                {
                    questScript.enabled = true;
                }
            }
        }

        public void FinishQuestOne()
        {
            quest1 = true;
            lastQuestFinished = 1;
            Destroy(questHolder1);
            playerManager.playerStatsManager.AddXP(100);
            CapsuleCollider questScript = questHolder2.GetComponent<CapsuleCollider>();

            if(!questScript.enabled) 
            { 
                questScript.enabled = true;
            }
        }

        public void FinishQuestTwo()
        {
            quest2 = true;
            lastQuestFinished = 2;
            Destroy(questHolder2);
            arenaManager.StartLevelOne();
            CapsuleCollider questScript = questHolder3.GetComponent<CapsuleCollider>();

            if (!questScript.enabled)
            {
                questScript.enabled = true;
            }
        }

        public void FinishQuestThree()
        {
            quest3 = true;
            lastQuestFinished = 3;
            Destroy(questHolder3);
            playerManager.playerStatsManager.AddXP(100);

            CapsuleCollider questScript = questHolder4.GetComponent<CapsuleCollider>();

            if (!questScript.enabled)
            {
                questScript.enabled = true;
            }
        }

        public void FinishQuestFour()
        {
            quest4 = true;
            lastQuestFinished = 4;
            Destroy(questHolder4);
            arenaManager.StartLevelTwo();

            CapsuleCollider questScript = questHolder6.GetComponent<CapsuleCollider>();

            if (!questScript.enabled)
            {
                questScript.enabled = true;
            }

            CapsuleCollider artifactScript = questHolder5.GetComponent<CapsuleCollider>();

            if (!artifactScript.enabled)
            {
                artifactScript.enabled = true;
            }
        }

        public void FinishQuestFive()
        {
            quest5 = true;
            lastQuestFinished = 5;
            Destroy(questHolder5);
            playerManager.playerInventoryManager.weaponsInventory.Add(questHolder5.weaponArtifact);
            playerManager.playerInventoryManager.headEquipmentInventory.Add(questHolder5.helmetArtifact);
            playerManager.playerInventoryManager.bodyEquipmentInventory.Add(questHolder5.bodyArtifact);
            playerManager.playerInventoryManager.legEquipmentInventory.Add(questHolder5.legArtifact);
            playerManager.playerInventoryManager.handEquipmentInventory.Add(questHolder5.handArtifact);
            playerManager.playerStatsManager.AddXP(5000);
        }

        public void FinishQuestSix()
        {
            quest6 = true;
            lastQuestFinished = 6;
            Destroy(questHolder6);
            arenaManager.StartLevelThree();

            CapsuleCollider questScript = questHolder7.GetComponent<CapsuleCollider>();

            if (!questScript.enabled)
            {
                questScript.enabled = true;
            }
        }

        public void FinishQuestSeven()
        {
            quest7 = true;
            lastQuestFinished = 7;
            Destroy(questHolder7);
            arenaManager.StartBossLevel();
        }

        public void LoadAllQuests()
        {
            if (lastQuestFinished == 1)
            {
                CapsuleCollider questScript = questHolder2.GetComponent<CapsuleCollider>();

                if (!questScript.enabled)
                {
                    questScript.enabled = true;
                }

                CapsuleCollider questScript1 = questHolder1.GetComponent<CapsuleCollider>();

                if (questScript1.enabled)
                {
                    questScript1.enabled = false;
                }
            }
            else if (lastQuestFinished == 2)
            {
                CapsuleCollider questScript = questHolder3.GetComponent<CapsuleCollider>();

                if (!questScript.enabled)
                {
                    questScript.enabled = true;
                }
            }
            else if (lastQuestFinished == 3)
            {
                CapsuleCollider questScript = questHolder4.GetComponent<CapsuleCollider>();

                if (!questScript.enabled)
                {
                    questScript.enabled = true;
                }
            }
            else if (lastQuestFinished == 4)
            {
                CapsuleCollider questScript = questHolder6.GetComponent<CapsuleCollider>();

                if (!questScript.enabled)
                {
                    questScript.enabled = true;
                }

                CapsuleCollider artifactScript = questHolder5.GetComponent<CapsuleCollider>();

                if (!artifactScript.enabled)
                {
                    artifactScript.enabled = true;
                }
            }
            else if (lastQuestFinished == 6)
            {
                CapsuleCollider questScript = questHolder7.GetComponent<CapsuleCollider>();

                if (!questScript.enabled)
                {
                    questScript.enabled = true;
                }
            }
        }
    }
}

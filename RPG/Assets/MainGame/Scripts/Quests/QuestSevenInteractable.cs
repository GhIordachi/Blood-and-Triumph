using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class QuestSevenInteractable : Interactable
    {
        QuestManager questManager;

        protected override void Awake()
        {
            questManager = FindObjectOfType<QuestManager>();
            questManager.questHolder7 = FindObjectOfType<QuestSevenInteractable>();
            if (questManager.quest7 == true)
                Destroy(questManager.questHolder7);
        }

        public override void Interact(PlayerManager playerManager)
        {
            questManager.quest7 = true;
            playerManager.UIManager.hudWindow.SetActive(false);
            playerManager.UIManager.questHolder.SetActive(true);
            playerManager.UIManager.quest7.SetActive(true);
            playerManager.inputHandler.inventoryFlag = true;
        }
    }
}

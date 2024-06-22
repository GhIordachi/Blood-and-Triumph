using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class QuestFourInteractable : Interactable
    {
        QuestManager questManager;

        protected override void Awake()
        {
            questManager = FindObjectOfType<QuestManager>();
            questManager.questHolder4 = FindObjectOfType<QuestFourInteractable>();
            if (questManager.quest4 == true)
                Destroy(questManager.questHolder4);
        }

        public override void Interact(PlayerManager playerManager)
        {
            questManager.quest4 = true;
            playerManager.UIManager.hudWindow.SetActive(false);
            playerManager.UIManager.questHolder.SetActive(true);
            playerManager.UIManager.quest4.SetActive(true);
            playerManager.inputHandler.inventoryFlag = true;
        }
    }
}

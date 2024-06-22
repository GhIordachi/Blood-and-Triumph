using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class QuestSixInteractable : Interactable
    {
        QuestManager questManager;

        protected override void Awake()
        {
            questManager = FindObjectOfType<QuestManager>();
            questManager.questHolder6 = FindObjectOfType<QuestSixInteractable>();
            if (questManager.quest6 == true)
                Destroy(questManager.questHolder6);
        }

        public override void Interact(PlayerManager playerManager)
        {
            questManager.quest6 = true;
            playerManager.UIManager.hudWindow.SetActive(false);
            playerManager.UIManager.questHolder.SetActive(true);
            playerManager.UIManager.quest6.SetActive(true);
            playerManager.inputHandler.inventoryFlag = true;
        }
    }
}

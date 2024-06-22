using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class QuestThreeInteractable : Interactable
    {
        QuestManager questManager;

        protected override void Awake()
        {
            questManager = FindObjectOfType<QuestManager>();
            questManager.questHolder3 = FindObjectOfType<QuestThreeInteractable>();
            if (questManager.quest3 == true)
                Destroy(questManager.questHolder3);
        }

        public override void Interact(PlayerManager playerManager)
        {
            questManager.quest3 = true;
            playerManager.UIManager.hudWindow.SetActive(false);
            playerManager.UIManager.questHolder.SetActive(true);
            playerManager.UIManager.quest3.SetActive(true);
            playerManager.inputHandler.inventoryFlag = true;
        }
    }
}

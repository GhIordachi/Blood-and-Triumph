using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class QuestTwoInteractable : Interactable
    {
        QuestManager questManager;

        protected override void Awake()
        {
            questManager = FindObjectOfType<QuestManager>();
            questManager.questHolder2 = FindObjectOfType<QuestTwoInteractable>();
            if (questManager.quest2 == true)
                Destroy(questManager.questHolder2);
        }

        public override void Interact(PlayerManager playerManager)
        {
            questManager.quest2 = true;
            playerManager.UIManager.hudWindow.SetActive(false);
            playerManager.UIManager.questHolder.SetActive(true);
            playerManager.UIManager.quest2.SetActive(true);
            playerManager.inputHandler.inventoryFlag = true;
        }
    }
}

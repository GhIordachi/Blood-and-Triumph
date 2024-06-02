using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class QuestOneInteractable : Interactable
    {
        QuestManager questManager;

        protected override void Awake()
        {
            questManager = FindObjectOfType<QuestManager>();
            questManager.questHolder1 = FindObjectOfType<QuestOneInteractable>();
            if (questManager.quest1 == true)
                Destroy(questManager.questHolder1);
        }

        public override void Interact(PlayerManager playerManager)
        {
            questManager.quest1 = true;
            playerManager.UIManager.hudWindow.SetActive(false);
            playerManager.UIManager.questHolder.SetActive(true);
            playerManager.UIManager.quest1.SetActive(true);
        }
    }
}

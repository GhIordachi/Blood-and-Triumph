using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class QuestFiveInteractable : Interactable
    {
        QuestManager questManager;

        public WeaponItem weaponArtifact;
        public HelmetEquipment helmetArtifact;
        public BodyEquipment bodyArtifact;
        public LegEquipment legArtifact;
        public HandEquipment handArtifact;

        protected override void Awake()
        {
            questManager = FindObjectOfType<QuestManager>();
            questManager.questHolder5 = FindObjectOfType<QuestFiveInteractable>();
            if (questManager.quest5 == true)
                Destroy(questManager.questHolder5);
        }

        public override void Interact(PlayerManager playerManager)
        {
            questManager.quest4 = true;
            playerManager.UIManager.hudWindow.SetActive(false);
            playerManager.UIManager.questHolder.SetActive(true);
            playerManager.UIManager.quest5.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class QuestManager : MonoBehaviour
    {
        [Header("Quests Completed")]
        public bool quest1 = false;
        public bool quest2 = false;
        public bool quest3 = false;
        public bool quest4 = false;
        public bool quest5 = false;
        public bool quest6 = false;
        public bool quest7 = false;

        [Header("Quest Holders")]
        public QuestOneInteractable questHolder1;

        public void FinishQuestOne()
        {
            quest1 = true;            
            Destroy(questHolder1);
        }
    }
}

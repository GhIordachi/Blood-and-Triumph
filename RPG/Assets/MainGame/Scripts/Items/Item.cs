using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class Item : ScriptableObject
    {
        [Header("Item Information")]
        public Sprite itemIcon;
        public string itemName;
        public int itemID;
        public int value;

        [Header("Item Stats Requirement")]
        public int strengthLevelRequirement;
        public int faithLevelRequirement;
    }
}

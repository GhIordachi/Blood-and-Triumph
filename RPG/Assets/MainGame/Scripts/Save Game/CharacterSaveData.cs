using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [System.Serializable]
    public class CharacterSaveData
    {
        public string characterName;

        public int characterLevel;

        public int currentScene;

        [Header("Equipment")]
        public int characterRightHandWeaponID;
        public int characterLeftHandWeaponID;

        public int currentHeadGearItemID;
        public int currentChestGearItemID;
        public int currentLegGearItemID;
        public int currentHandGearItemID;

        [Header("Facial Features")]
        public int currentHairID = -1;
        public int currentEyebrowID = -1;
        public int currentBeardID = -1;
        public Color currentHairColor;

        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;

        [Header("Items Looted From World")]
        public SerializableDictionary<int, bool> itemsInWorld;   //The int is the world item ID, the bool is if the item has been looetd

        public CharacterSaveData()
        {
            itemsInWorld = new SerializableDictionary<int, bool>();
        }
    }
}

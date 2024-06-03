using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [System.Serializable]
    public class CharacterSaveData
    {
        public string characterName;

        public int currentScene;

        public int currentGold;

        [Header("Player Levels")]
        public int currentXPCount;
        public int characterLevel;
        public int healthLevel;
        public int staminaLevel;
        public int focusLevel;
        public int poiseLevel;
        public int strengthLevel;
        public int faithLevel;

        [Header("Items Equiped")]
        public int characterRightHandWeaponID;
        public int characterLeftHandWeaponID;
        public int characterCurrentRightWeaponIndex;
        public int characterCurrentLeftWeaponIndex;

        public int characterCurrentSpellID;
        public int characterCurrentConsumableID;
        public int characterCurrentAmmoID;

        public int currentHeadGearItemID;
        public int currentChestGearItemID;
        public int currentLegGearItemID;
        public int currentHandGearItemID;

        public int ringSlot01;
        public int ringSlot02;
        public int ringSlot03;
        public int ringSlot04;

        public List<int> weaponsInRightHandByID = new List<int>(4);
        public List<int> weaponsInLeftHandByID = new List<int>(4);

        [Header("Facial Features")]
        public int currentHairID = -1;
        public int currentEyebrowID = -1;
        public int currentBeardID = -1;
        public Color currentHairColor;

        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;

        [Header("Player's Inventory")]
        public List<int> weaponsInInventoryByID = new List<int>();
        public List<int> helmetsInInventoryByID = new List<int>();
        public List<int> bodyInInventoryByID = new List<int>();
        public List<int> legsInInventoryByID = new List<int>();
        public List<int> armsInInventoryByID = new List<int>();
        public List<int> ringsInInventoryByID = new List<int>();
        public List<int> spellsInInventoryByID = new List<int>();
        public List<int> consumablesInInventoryByID = new List<int>();
        public List<int> ammoInInventoryByID = new List<int>();

        [Header("Items Looted From World")]
        public SerializableDictionary<int, bool> itemsInWorld;   //The int is the world item ID, the bool is if the item has been looetd

        public CharacterSaveData()
        {
            itemsInWorld = new SerializableDictionary<int, bool>();
        }
    }
}

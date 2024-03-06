using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class NameCharacter : MonoBehaviour
    {
        public CharacterManager character;
        public InputField inputField;
        public Text nameButtonText;

        public void NameMyCharacter()
        {
            character.characterStatsManager.characterName = inputField.text;
            if(character.characterStatsManager.characterName == "")
            {
                character.characterStatsManager.characterName = "Nameless";
            }

            nameButtonText.text = character.characterStatsManager.characterName;
        }
    }
}

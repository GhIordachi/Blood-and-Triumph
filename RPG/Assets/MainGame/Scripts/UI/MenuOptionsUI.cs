using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class MenuOptionsUI : MonoBehaviour
    {
        WorldSaveGameManager saveGameManager;

        private void Awake()
        {
            saveGameManager = FindObjectOfType<WorldSaveGameManager>();
        }

        public void SaveGameUI()
        {
            saveGameManager.SaveGame();
        }

        public void LoadGameUI()
        {
            saveGameManager.LoadGame();
        }

        public void ExitGameUI()
        {
            //Exit the game
        }
    }
}

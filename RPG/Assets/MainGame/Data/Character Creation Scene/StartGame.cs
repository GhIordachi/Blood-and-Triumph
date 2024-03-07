using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class StartGame : MonoBehaviour
    {
        WorldSaveGameManager worldSaveGameManager;

        private void Awake()
        {
            worldSaveGameManager = FindObjectOfType<WorldSaveGameManager>();
        }

        public void StartTheGame()
        {
            if(worldSaveGameManager != null)
            {
                worldSaveGameManager.SaveGame();
                worldSaveGameManager.LoadGame();
            }
        }
    }
}

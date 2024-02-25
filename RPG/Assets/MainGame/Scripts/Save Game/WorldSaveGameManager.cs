using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GI
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;

        public PlayerManager player;

        [Header("Save Data Writer")]
        SaveGameDataWriter saveGameDataWriter;

        [Header("Current Character Data")]
        //Character slot #
        public CharacterSaveData currentCharacterSaveData;
        [SerializeField] private string fileName;

        [Header("Save/Load")]
        [SerializeField] bool saveGame;
        [SerializeField] bool loadGame;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            //Load all possible character profiles
        }

        private void Update()
        {
            if(saveGame)
            {
                saveGame = false;
                SaveGame();

            }
            else if(loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        // New Game

        // Save Game
        public void SaveGame()
        {
            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName = fileName;

            // Pass along our characters data to the current save file
            player.SaveCharacterDataToCurrentSaveData(ref currentCharacterSaveData);

            // Write the current character data to a json file and save it on this device
            saveGameDataWriter.WriteCharacterDataToSaveFile(currentCharacterSaveData);

            Debug.Log("Saving game...");
            Debug.Log("File saved as " + fileName);
        }

        // Load Game
        public void LoadGame()
        {
            //Decide load file based on character save slot

            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveGameDataWriter .dataSaveFileName = fileName;
            currentCharacterSaveData = saveGameDataWriter.LoadCharacterDataFromJson();

            StartCoroutine(LoadWorldSceneAsynchronously());
        }

        private IEnumerator LoadWorldSceneAsynchronously()
        {
            if(player == null)
            {
                player = FindObjectOfType<PlayerManager>();
            }

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(0);

            while(!loadOperation.isDone)
            {
                float loadingProgress = Mathf.Clamp01(loadOperation.progress / 0.9f);
                //Enable a loading screen & pass the loading progress to a slider/loading effect
                yield return null;
            }

            player.LoadCharacterDataFromCurrentCharacterSaveData(ref currentCharacterSaveData);
        }
    }
}

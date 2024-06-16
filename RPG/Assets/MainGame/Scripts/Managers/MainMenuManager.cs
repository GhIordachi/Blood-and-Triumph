using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GI
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]string newGameScene = "Character Creation Scene";
        [SerializeField] Button continueButton;
        [SerializeField] WorldSaveGameManager saveGameManager;

        private void Awake()
        {
            saveGameManager = FindObjectOfType<WorldSaveGameManager>();
        }

        void Start()
        {
            continueButton.interactable = saveGameManager.SaveFileExists();
            continueButton.onClick.AddListener(ContinueGame);
        }

        public void StartNewGame()
        {
            SceneManager.LoadSceneAsync(newGameScene);
        }

        public void ContinueGame()
        {
            if (saveGameManager.SaveFileExists())
            {
                saveGameManager.LoadGame();
            }
        }

        public void ExitGame()
        {
            Debug.Log("Exiting game...");
            Application.Quit();

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class ArenaManager : MonoBehaviour
    {
        PlayerManager player;

        public Transform teleportPoint;
        public Transform teleportOutsideArenaPoint;

        public bool level01 = false;
        public bool level02 = false;
        public bool level03 = false;
        public bool level04 = false;

        [Header("First Level")]
        public GameObject enemyPrefab01;
        [SerializeField] Vector3 spawnPosition01;
        public bool isEnemyLevelOneDead = false;

        [Header("Second Level")]
        public GameObject enemyPrefab02;
        public GameObject enemyPrefab03;
        [SerializeField] Vector3 spawnPosition02;
        public bool isEnemyOneLevelTwoDead = false;
        public bool isEnemyTwoLevelTwoDead = false;

        [Header("Third Level")]
        public GameObject enemyPrefab04;
        public GameObject enemyPrefab05;
        public GameObject enemyPrefab06;
        public GameObject companionPrefab01;
        [SerializeField] Vector3 spawnPosition03;
        [SerializeField] Vector3 spawnPosition04;
        [SerializeField] Vector3 spawnPosition05;
        public bool isEnemyOneLevelThreeDead = false;
        public bool isEnemyTwoLevelThreeDead = false;
        public bool isEnemyThreeLevelThreeDead = false;
        public bool isEnemyFourLevelThreeDead = false;

        [Header("Boss Level")]
        public GameObject bossPrefab;
        public UIBossHealthBar bossHealthBar;
        public bool isBossDead = false;

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
            bossHealthBar = FindObjectOfType<UIBossHealthBar>();
        }

        private void Update()
        {
            if(isEnemyLevelOneDead && level01)
            {
                StartCoroutine(WaitBeforeFirstTeleport());
                isEnemyLevelOneDead = false;
                level01 = false;                
            }

            if(isEnemyOneLevelTwoDead && isEnemyTwoLevelTwoDead && level02)
            {
                StartCoroutine(WaitBeforeSecondTeleport());
                isEnemyOneLevelTwoDead=false;
                isEnemyTwoLevelTwoDead=false;
                level02 = false;
            }

            if(isEnemyOneLevelThreeDead && isEnemyTwoLevelThreeDead && isEnemyThreeLevelThreeDead && isEnemyFourLevelThreeDead && level03)
            {
                StartCoroutine(WaitBeforeThirdTeleport());
                isEnemyOneLevelThreeDead=false;
                isEnemyTwoLevelThreeDead=false;
                isEnemyThreeLevelThreeDead=false;
                isEnemyFourLevelThreeDead=false;
                level03 = false;
            }

            if(isBossDead && level04)
            {
                StartCoroutine(WaitBeforeFourthTeleport());
                bossHealthBar.gameObject.SetActive(false);
                isBossDead = false;
                level04 = false;
            }
        }

        public void StartLevelOne()
        {
            level01 = true;
            Instantiate(enemyPrefab01, spawnPosition01, Quaternion.identity);

            if (teleportPoint != null && player != null)
            {
                player.playerLocomotionManager.moveDirection = Vector3.zero;
                player.transform.position = teleportPoint.position;
                player.characterController.enabled = false;
                StartCoroutine(ReEnableMovementScript());
            }
        }

        public void StartLevelTwo()
        {
            level02 = true;
            Instantiate(enemyPrefab02, spawnPosition01, Quaternion.identity);
            Instantiate(enemyPrefab03, spawnPosition02, Quaternion.identity);

            if (teleportPoint != null && player != null)
            {
                // Set the player's moveDirection to zero
                player.playerLocomotionManager.moveDirection = Vector3.zero;

                // Teleport the player to the desired position using the transform.position property
                player.transform.position = teleportPoint.position;

                // Temporarily disable the movement script
                player.characterController.enabled = false;

                // Wait for a short time before re-enabling the movement script
                StartCoroutine(ReEnableMovementScript());
            }
        }

        public void StartLevelThree()
        {
            level03 = true;
            Instantiate(enemyPrefab03, spawnPosition02, Quaternion.identity);
            Instantiate(enemyPrefab06, spawnPosition03, Quaternion.identity);
            Instantiate(enemyPrefab04, spawnPosition04, Quaternion.identity);
            Instantiate(enemyPrefab05, spawnPosition01, Quaternion.identity);
            //Instantiate(companionPrefab01, spawnPosition05, Quaternion.identity);

            if (teleportPoint != null && player != null)
            {
                // Set the player's moveDirection to zero
                player.playerLocomotionManager.moveDirection = Vector3.zero;

                // Teleport the player to the desired position using the transform.position property
                player.transform.position = teleportPoint.position;

                // Temporarily disable the movement script
                player.characterController.enabled = false;

                // Wait for a short time before re-enabling the movement script
                StartCoroutine(ReEnableMovementScript());
            }
        }

        public void StartBossLevel()
        {
            level04 = true;
            Instantiate(bossPrefab, spawnPosition01, Quaternion.identity);
            bossHealthBar.SetUIHealthBarToActive();

            if (teleportPoint != null && player != null)
            {
                // Set the player's moveDirection to zero
                player.playerLocomotionManager.moveDirection = Vector3.zero;

                // Teleport the player to the desired position using the transform.position property
                player.transform.position = teleportPoint.position;

                // Temporarily disable the movement script
                player.characterController.enabled = false;

                // Wait for a short time before re-enabling the movement script
                StartCoroutine(ReEnableMovementScript());
            }
        }

        void TeleportBack()
        {
            if (teleportOutsideArenaPoint != null && player != null)
            {
                // Set the player's moveDirection to zero
                player.playerLocomotionManager.moveDirection = Vector3.zero;

                // Teleport the player to the desired position using the transform.position property
                player.transform.position = teleportOutsideArenaPoint.position;

                // Temporarily disable the movement script
                player.characterController.enabled = false;

                // Wait for a short time before re-enabling the movement script
                StartCoroutine(ReEnableMovementScript());
            }
        }

        IEnumerator ReEnableMovementScript()
        {
            // Wait for a short time before re-enabling the movement script
            yield return new WaitForSeconds(0.1f);

            // Re-enable the movement script
            player.characterController.enabled = true;
        }

        IEnumerator WaitBeforeFirstTeleport()
        {
            yield return new WaitForSeconds(3f);
            player.playerStatsManager.AddXP(200);
            player.playerInventoryManager.currentGold += 50;
            player.UIManager.questHolder.SetActive(true);
            player.UIManager.additionalQuest2.SetActive(true);
            TeleportBack();
        }

        IEnumerator WaitBeforeSecondTeleport()
        {
            yield return new WaitForSeconds(3f);
            player.playerStatsManager.AddXP(300);
            player.playerInventoryManager.currentGold += 100;
            player.UIManager.questHolder.SetActive(true);
            player.UIManager.additionalQuest4.SetActive(true);
            TeleportBack();
        }

        IEnumerator WaitBeforeThirdTeleport()
        {
            yield return new WaitForSeconds(3f);
            if(companionPrefab01.gameObject == true)
                Destroy(companionPrefab01.gameObject);
            player.playerStatsManager.AddXP(1000);
            player.playerInventoryManager.currentGold += 200;
            player.UIManager.questHolder.SetActive(true);
            player.UIManager.additionalQuest6.SetActive(true);
            TeleportBack();
        }

        IEnumerator WaitBeforeFourthTeleport()
        {
            yield return new WaitForSeconds(5f);
            player.playerStatsManager.AddXP(10000);
            player.playerInventoryManager.currentGold += 1000;
            player.UIManager.questHolder.SetActive(true);
            player.UIManager.additionalQuest7.SetActive(true);
            TeleportBack();
        }

    }
}

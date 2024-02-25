using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class WorldEventManager : MonoBehaviour
    {
        //Fog Wall
        public List<FogWall> fogWalls;
        public UIBossHealthBar bossHealthBar;
        public AICharacterBossManager enemyBossManager;

        public bool bossFightIsActive; //Is currently fighting boss
        public bool bossHasBeenAwakened; //Woke the boss/watched cutscene but died during fight
        public bool bossHasBeenDefeated; //Boss has been defeated

        private void Awake()
        {
            bossHealthBar = FindObjectOfType<UIBossHealthBar>();
        }

        public void ActivateBossFight()
        {
            bossFightIsActive = true;
            bossHasBeenAwakened = true;
            bossHealthBar.SetUIHealthBarToActive();
            //Activate Fog Wall(s)
            foreach (var fogWall in fogWalls)
            {
                fogWall.ActivateFogWall();
            }
        }

        public void BossHasBeenDefeated() 
        {
            bossHasBeenDefeated = true;
            bossFightIsActive = false;
            //Deactivate Fog Walls
            foreach (var fogWall in fogWalls)
            {
                fogWall.DeactivateFogWall();
            }
        }
    }
}

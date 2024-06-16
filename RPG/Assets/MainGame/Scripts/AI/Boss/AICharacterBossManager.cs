using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class AICharacterBossManager : MonoBehaviour
    {
        public string bossName;

        UIBossHealthBar bossHealthBar;
        AICharacterManager enemy;

        private void Awake()
        {
            bossHealthBar = FindAnyObjectByType<UIBossHealthBar>();
            enemy = GetComponent<AICharacterManager>();
            bossHealthBar.SetBossName(bossName);
            bossHealthBar.SetBossMaxHealth(enemy.aiCharacterStatsManager.maxHealth);
        }

        public void UpdateBossHealthBar(int currentHealth, int maxHealth)
        {
            bossHealthBar.SetBossCurrentHealth(currentHealth);
        }
    }
}

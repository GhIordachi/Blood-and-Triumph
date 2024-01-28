using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class EnemyBossManager : MonoBehaviour
    {
        public string bossName;

        UIBossHealthBar bossHealthBar;
        EnemyStats enemyStats;
        //Handle switching phases
        //Handle switching attack patterns

        private void Awake()
        {
            bossHealthBar = FindAnyObjectByType<UIBossHealthBar>();
            enemyStats = GetComponent<EnemyStats>();
        }

        private void Start()
        {
            bossHealthBar.SetBossName(bossName);
            bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
        }

        public void UpdateBossHealthBar(int currentHealth)
        {
            bossHealthBar.SetBossCurrentHealth(currentHealth);
        }
    }
}

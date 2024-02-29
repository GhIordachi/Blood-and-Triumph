using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class UIEnemyHealthBar : MonoBehaviour
    {
        public Slider slider;
        float timeUntilBarIsHidden = 0;
        UIYellowBar yellowBar;
        [SerializeField] float yellowBarTimer = 2;
        [SerializeField] Text damageText;
        [SerializeField] int currentDamageTaken;

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
            yellowBar = GetComponentInChildren<UIYellowBar>();
        }

        private void OnDisable()
        {
            currentDamageTaken = 0;
        }

        public void SetHealth(int health)
        {
            if (yellowBar != null)
            {
                yellowBar.gameObject.SetActive(true); //Triggers the OnEnable function

                yellowBar.timer = yellowBarTimer;  //Every time we hit, we reset the timer

                if (health > slider.value)
                {
                    yellowBar.slider.value = health;
                }
            }

            currentDamageTaken = currentDamageTaken + Mathf.RoundToInt(slider.value - health);
            damageText.text = currentDamageTaken.ToString();

            slider.value = health;
            timeUntilBarIsHidden = 3;
        }

        public void SetMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;

            if(yellowBar != null)
            {
                yellowBar.SetMaxStat(maxHealth);
            }
        }

        private void Update()
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);
            timeUntilBarIsHidden = timeUntilBarIsHidden - Time.deltaTime;

            if(slider != null)
            {
                if (timeUntilBarIsHidden <= 0)
                {
                    timeUntilBarIsHidden = 0;
                    slider.gameObject.SetActive(false);
                }
                else
                {
                    if (!slider.gameObject.activeInHierarchy)
                    {
                        slider.gameObject.SetActive(true);
                    }
                }

                if (slider.value <= 0)
                {
                    Destroy(slider.gameObject);
                }
            }
        }
    }
}

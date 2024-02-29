using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class UIYellowBar : MonoBehaviour
    {
        public Slider slider;
        UIEnemyHealthBar parentHealthBar;
        public float timer;

        private void Awake()
        {
            slider = GetComponent<Slider>();
            parentHealthBar = GetComponentInParent<UIEnemyHealthBar>();
        }

        private void OnEnable()
        {
            if(timer <= 0)
            {
                timer = 1f; //How long the bar is waiting before subtracting
            }
        }

        public void SetMaxStat(int maxStat)
        {
            slider.maxValue = maxStat;
            slider.value = maxStat;
        }

        private void Update()
        {
            if(timer <= 0)
            {
                if(slider.value > parentHealthBar.slider.value)
                {
                    slider.value = slider.value - 0.5f;
                }
                else if(slider.value <= parentHealthBar.slider.value)
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                timer = timer - Time.deltaTime;
            }
        }
    }
}

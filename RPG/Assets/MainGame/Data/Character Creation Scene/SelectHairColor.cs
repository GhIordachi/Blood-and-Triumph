using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class SelectHairColor : MonoBehaviour
    {
        PlayerManager player;

        [Header("Color Values")]
        public float redAmount;
        public float greenAmount;
        public float blueAmount;

        [Header("Color Sliders")]
        public Slider redSlider;
        public Slider greenSlider;
        public Slider blueSlider;

        private Color currentHairColor;

        //We grab the material from the skin mesh renderer, and change the color properties of the material
        public List<SkinnedMeshRenderer> rendererList = new List<SkinnedMeshRenderer>();

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }

        public void UpdateSliders()
        {
            redAmount = redSlider.value;
            greenAmount = greenSlider.value;
            blueAmount = blueSlider.value;
            SetHairColor();
        }

        public void SetHairColor()
        {
            currentHairColor = new Color(redAmount, greenAmount, blueAmount);

            for(int i = 0; i < rendererList.Count; i++)
            {
                rendererList[i].material.SetColor("_Color_Hair", currentHairColor);
            }

            if(player != null)
            {
                player.playerEquipmentManager.hairColor = currentHairColor;
            }
        }
    }
}

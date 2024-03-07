using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace GI
{
    public class ColorFacialFeatures : MonoBehaviour
    {
        PlayerManager player;

        private Color currentHairColor;

        //We grab the material from the skin mesh renderer, and change the color properties of the material
        public List<SkinnedMeshRenderer> rendererList = new List<SkinnedMeshRenderer>();

        private void Awake()
        {
            player = GetComponentInParent<PlayerManager>();            
        }

        public void SetHairColor()
        {
            currentHairColor = player.playerEquipmentManager.hairColor;

            for (int i = 0; i < rendererList.Count; i++)
            {
                rendererList[i].material.SetColor("_Color_Hair", currentHairColor);
            }
        }
    }
}

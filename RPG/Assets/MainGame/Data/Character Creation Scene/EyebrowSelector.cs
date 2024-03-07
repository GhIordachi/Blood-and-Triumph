using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class EyebrowSelector : MonoBehaviour
    {
        public List<GameObject> eyebrowModels;
        PlayerManager player;

        private void Awake()
        {
            player = GetComponentInParent<PlayerManager>();
            GetAllEyebrowModels();
        }

        private void GetAllEyebrowModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                eyebrowModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void GetEyebrowByID(int eyebrowID)
        {
            if (eyebrowID == -1)
                player.playerEquipmentManager.eyebrows = null;
            for(int i = 0;i < eyebrowModels.Count; i++)
            {
                if(eyebrowID == i)
                {
                    player.playerEquipmentManager.eyebrows = eyebrowModels[i];
                }
            }
        }
    }
}

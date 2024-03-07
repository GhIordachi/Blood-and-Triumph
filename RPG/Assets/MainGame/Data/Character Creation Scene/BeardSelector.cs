using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class BeardSelector : MonoBehaviour
    {
        public List<GameObject> beardModels;
        PlayerManager player;

        private void Awake()
        {
            player = GetComponentInParent<PlayerManager>();
            GetAllBeardModels();
        }

        private void GetAllBeardModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                beardModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void GetBeardByID(int beardID)
        {
            if (beardID == -1)
                player.playerEquipmentManager.beard = null;
            for (int i = 0; i < beardModels.Count; i++)
            {
                if (beardID == i)
                {
                    player.playerEquipmentManager.beard = beardModels[i];
                }
            }
        }
    }
}

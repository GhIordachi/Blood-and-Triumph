using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class HairSelector : MonoBehaviour
    {
        public List<GameObject> hairModels;
        PlayerManager player;

        private void Awake()
        {
            player = GetComponentInParent<PlayerManager>();
            GetAllHairModels();
        }

        private void GetAllHairModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
                hairModels.Add(transform.GetChild(i).gameObject);
        }

        public void GetHairByID(int hairID)
        {
            if (hairID == -1)
                player.playerEquipmentManager.hair = null;
            for (int i = 0; i < hairModels.Count; i++)
            {
                if (hairID == i)
                    player.playerEquipmentManager.hair = hairModels[i];
            }
        }
    }
}

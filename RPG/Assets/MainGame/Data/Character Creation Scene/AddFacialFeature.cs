using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class AddFacialFeature : MonoBehaviour
    {
        PlayerManager player;
        public GameObject eyebrowParent;
        public GameObject beardParent;
        public GameObject hairParent;

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }

        public void AddFacialFeatures()
        {
            if (eyebrowParent != null)
            {
                for (int i = 0; i < eyebrowParent.transform.childCount; i++)
                {
                    var child = eyebrowParent.transform.GetChild(i).gameObject;

                    if (child != null && child.activeSelf)
                    {
                        player.playerEquipmentManager.eyebrows = child;
                        player.playerEquipmentManager.eyebrowID = i;
                    }
                }
            }

            if (beardParent != null)
            {
                for (int i = 0; i < beardParent.transform.childCount; i++)
                {
                    var child = beardParent.transform.GetChild(i).gameObject;

                    if (child != null && child.activeSelf)
                    {
                        player.playerEquipmentManager.beard = child;
                        player.playerEquipmentManager.beardID = i;
                    }
                }
            }

            if (hairParent != null)
            {
                for (int i = 0; i < hairParent.transform.childCount; i++)
                {
                    var child = hairParent.transform.GetChild(i).gameObject;

                    if (child != null && child.activeSelf)
                    {
                        player.playerEquipmentManager.hair = child;
                        player.playerEquipmentManager.hairID = i;
                    }
                }
            }
        }
    }
}

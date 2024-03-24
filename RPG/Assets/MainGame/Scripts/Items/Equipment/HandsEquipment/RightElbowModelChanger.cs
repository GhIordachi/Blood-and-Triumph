using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class RightElbowModelChanger : MonoBehaviour
    {
        public List<GameObject> elbowModels;

        private void Awake()
        {
            GetAllElbowModels();
        }

        private void GetAllElbowModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                elbowModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnEquipAllElbowModels()
        {
            foreach (GameObject item in elbowModels)
            {
                item.SetActive(false);
            }
        }

        public void EquipElbowModelByName(string elbowName)
        {
            for (int i = 0; i < elbowModels.Count; i++)
            {
                if (elbowModels[i].name == elbowName)
                {
                    elbowModels[i].SetActive(true);
                }
            }
        }
    }
}

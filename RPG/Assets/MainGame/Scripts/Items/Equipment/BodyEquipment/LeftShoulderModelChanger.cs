using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class LeftShoulderModelChanger : MonoBehaviour
    {
        public List<GameObject> leftShoulderModels;

        private void Awake()
        {
            GetAllLeftShoulderModels();
        }

        private void GetAllLeftShoulderModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                leftShoulderModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnEquipAllLeftShoulderModels()
        {
            foreach (GameObject item in leftShoulderModels)
            {
                item.SetActive(false);
            }
        }

        public void EquipLeftShoulderModelByName(string shoulderName)
        {
            for (int i = 0; i < leftShoulderModels.Count; i++)
            {
                if (leftShoulderModels[i].name == shoulderName)
                {
                    leftShoulderModels[i].SetActive(true);
                }
            }
        }
    }
}

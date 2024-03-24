using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class RightShoulderModelChanger : MonoBehaviour
    {
        public List<GameObject> rightShoulderModels;

        private void Awake()
        {
            GetAllRightShoulderModels();
        }

        private void GetAllRightShoulderModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                rightShoulderModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnEquipAllRightShoulderModels()
        {
            foreach (GameObject item in rightShoulderModels)
            {
                item.SetActive(false);
            }
        }

        public void EquipRightShoulderModelByName(string shoulderName)
        {
            for (int i = 0; i < rightShoulderModels.Count; i++)
            {
                if (rightShoulderModels[i].name == shoulderName)
                {
                    rightShoulderModels[i].SetActive(true);
                }
            }
        }
    }
}

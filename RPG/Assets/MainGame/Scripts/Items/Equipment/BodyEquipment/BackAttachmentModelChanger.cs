using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class BackAttachmentModelChanger : MonoBehaviour
    {
        public List<GameObject> backAttachmentModels;

        private void Awake()
        {
            GetAllBackAttachmentModels();
        }

        private void GetAllBackAttachmentModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                backAttachmentModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnEquipAllBackAttachmentModels()
        {
            foreach (GameObject item in backAttachmentModels)
            {
                item.SetActive(false);
            }
        }

        public void EquipBackAttachmentModelByName(string backAttachmentName)
        {
            for (int i = 0; i < backAttachmentModels.Count; i++)
            {
                if (backAttachmentModels[i].name == backAttachmentName)
                {
                    backAttachmentModels[i].SetActive(true);
                }
            }
        }
    }
}

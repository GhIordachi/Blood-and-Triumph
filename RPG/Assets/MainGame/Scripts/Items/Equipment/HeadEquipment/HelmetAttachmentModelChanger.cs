using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class HelmetAttachmentModelChanger : MonoBehaviour
    {
        public List<GameObject> helmetAttachmentModels;

        private void Awake()
        {
            GetAllHelmetAttachmentModels();
        }

        private void GetAllHelmetAttachmentModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                helmetAttachmentModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnEquipAllHelmetAttachmentModels()
        {
            foreach (GameObject item in helmetAttachmentModels)
            {
                item.SetActive(false);
            }
        }

        public void EquipHelmetAttachmentModelByName(string helmetName)
        {
            for (int i = 0; i < helmetAttachmentModels.Count; i++)
            {
                if (helmetAttachmentModels[i].name == helmetName)
                {
                    helmetAttachmentModels[i].SetActive(true);
                }
            }
        }
    }
}

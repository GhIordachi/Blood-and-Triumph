using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parentOverride;
        public bool isLeftHandSlot;
        public bool isRightHandSlot;

        public GameObject curentWeaponModel;

        public void UnloadWeapon()
        {
            if(curentWeaponModel != null)
            {
                curentWeaponModel.SetActive(false);
            }
        }

        public void UnloadWeaponAndDestroy()
        {
            if (curentWeaponModel != null)
            {
                Destroy(curentWeaponModel);
            }
        }

        public void LoadWeaponItem(WeaponItem weaponItem)
        {
            UnloadWeaponAndDestroy();

            if(weaponItem == null)
            {
                UnloadWeapon();
                return;
            }

            GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
            if(model != null )
            {
                if(parentOverride != null)
                {
                    model.transform.parent = parentOverride;
                }
                else
                {
                    model.transform.parent = transform;
                }

                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }

            curentWeaponModel = model;
        }
    }
}

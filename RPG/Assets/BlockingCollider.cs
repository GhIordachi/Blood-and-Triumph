using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class BlockingCollider : MonoBehaviour
    {
        public BoxCollider blockingCollider;

        public float blockingPhysicalDamageAbsortion;

        private void Awake()
        {
            blockingCollider = GetComponent<BoxCollider>();
        }

        public void SetColliderDamageAbsorption(WeaponItem weapon)
        {
            if(weapon != null)
            {
                blockingPhysicalDamageAbsortion = weapon.physicalDamageAbsorption;
            }
        }

        public void EnableBlockingCollider()
        {
            blockingCollider.enabled = true;
        }

        public void DisableBlockingCollider()
        {
            blockingCollider.enabled = false;
        }
    }
}

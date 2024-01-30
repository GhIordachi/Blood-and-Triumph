using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class DestroyAfterTime : MonoBehaviour
    {
        public float timeUntilDestroyed = 2;
        private void Awake()
        {
            Destroy(gameObject, timeUntilDestroyed);
        }
    }
}

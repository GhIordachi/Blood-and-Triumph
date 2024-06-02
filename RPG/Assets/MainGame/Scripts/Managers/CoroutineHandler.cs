using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class CoroutineHandler : MonoBehaviour
    {
        public static CoroutineHandler instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartRoutine(IEnumerator routine)
        {
            StartCoroutine(routine);
        }
    }
}

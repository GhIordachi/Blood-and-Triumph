using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class HorseManager : MonoBehaviour
    {
        public bool isHorseMounted;

        public void MountHorse()
        {
            if(isHorseMounted)
            {
                return;
            }
            else
            {
                isHorseMounted = true;

            }
        }
    }
}

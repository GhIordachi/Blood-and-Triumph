using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [System.Serializable]
    public class ClassStats 
    {
        public string playerClass;
        public int classLevel;

        [TextArea]
        public string classDescription;

        [Header("Class Stats")]
        public int healthLevel;
        public int staminaLevel;
        public int focusLevel;
        public int poiseLevel;
        public int strengthLevel;
        public int faithLevel;
    }
}

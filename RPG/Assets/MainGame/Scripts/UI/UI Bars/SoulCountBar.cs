using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class SoulCountBar : MonoBehaviour
    {
        public Text xpCountText;

        public void SetXPCountText(int xpCount)
        {
            xpCountText.text = xpCount.ToString();
        }
    }
}

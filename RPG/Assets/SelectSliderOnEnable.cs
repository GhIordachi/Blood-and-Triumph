using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI
{
    public class SelectSliderOnEnable : MonoBehaviour
    {
        public Slider statSlider;

        private void OnEnable()
        {
            statSlider.Select();
            statSlider.OnSelect(null);
        }
    }
}

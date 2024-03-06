using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [CreateAssetMenu(menuName = "Items/Equipment/Helmet Equipment")]
    public class HelmetEquipment : EquipmentItem
    {
        public string helmetModelName;

        public bool hideHair;
        public bool hideEyebrows;
        public bool hideBeard;
    }
}

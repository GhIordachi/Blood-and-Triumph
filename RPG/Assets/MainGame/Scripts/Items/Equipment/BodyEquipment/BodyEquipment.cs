using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [CreateAssetMenu(menuName = "Items/Equipment/Torso Equipment")]
    public class BodyEquipment : EquipmentItem
    {
        public string torsoModelName;
        public string upperLeftArmModelName;
        public string upperRightArmModelName;
        public string backAttachmentModelName;
        public string shoulderRightAttachmentModelName;
        public string shoulderLeftAttachmentModelName;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class ArmorVendorInventoryManager : VendorInventoryManager
    {
        public List<HelmetEquipment> headEquipmentInventory;
        public List<BodyEquipment> bodyEquipmentInventory;
        public List<LegEquipment> legEquipmentInventory;
        public List<HandEquipment> handEquipmentInventory;
    }
}

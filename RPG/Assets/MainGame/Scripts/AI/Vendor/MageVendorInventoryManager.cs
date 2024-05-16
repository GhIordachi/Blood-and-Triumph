using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class MageVendorInventoryManager : VendorInventoryManager
    {
        public List<SpellItem> spellInventory;
        public List<ConsumableItem> consumableInventory;
        public List<RingItem> ringInventory;
    }
}

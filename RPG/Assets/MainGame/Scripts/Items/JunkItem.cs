using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [CreateAssetMenu(menuName = "Items/Junk")]
    public class JunkItem : Item
    {
        [Header("Item Description")]
        [TextArea] public string itemBackStory;
    }
}

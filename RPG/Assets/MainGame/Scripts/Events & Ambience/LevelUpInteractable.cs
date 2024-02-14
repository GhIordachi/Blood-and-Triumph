using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class LevelUpInteractable : Interactable
    {
        public override void Interact(PlayerManager playerManager)
        {
            playerManager.UIManager.levelUpWindow.SetActive(true);
        }
    }
}

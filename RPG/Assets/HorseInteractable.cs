using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class HorseInteractable : Interactable
    {
        HorseManager horseManager;
        public Transform characterStandingPosition;

        protected override void Awake()
        {
            horseManager = GetComponentInParent<HorseManager>();
        }

        public override void Interact(PlayerManager player)
        {
            horseManager.MountHorse();
            player.MountHorseInteraction(characterStandingPosition);
        }
    }
}

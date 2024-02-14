using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [CreateAssetMenu(menuName = "Item Actions/Draw Arrow Action")]
    public class DrawArrowAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isInteracting)
                return;

            if (player.isHoldingArrow)
                return;

            //Animate Player
            player.animator.SetBool("isHoldingArrow", true);
            player.playerAnimatorManager.PlayTargetAnimation("Bow_TH_Draw_01", true);
            //Instantiate Arrow
            GameObject loadedArrow = Instantiate(player.playerInventoryManager.currentAmmo.loadedItemModel, player.playerWeaponSlotManager.leftHandSlot.transform);

            //Animate Bow
            player.StartCoroutine(PlayBowAnimation(player));
            player.playerEffectsManager.currentRangeFX = loadedArrow;
        }

        private IEnumerator PlayBowAnimation(PlayerManager player)
        {
            yield return new WaitForSeconds(1f);
            Animator bowAnimator = player.playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", true);
            bowAnimator.Play("Bow_TH_Draw_01");
        }
    }
}

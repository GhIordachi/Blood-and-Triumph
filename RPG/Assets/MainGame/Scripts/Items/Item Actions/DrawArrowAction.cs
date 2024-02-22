using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [CreateAssetMenu(menuName = "Item Actions/Draw Arrow Action")]
    public class DrawArrowAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting)
                return;

            if (character.isHoldingArrow)
                return;

            //Animate Player
            character.animator.SetBool("isHoldingArrow", true);
            character.characterAnimatorManager.PlayTargetAnimation("Bow_TH_Draw_01", true);
            //Instantiate Arrow
            GameObject loadedArrow = Instantiate(character.characterInventoryManager.currentAmmo.loadedItemModel, character.characterWeaponSlotManager.leftHandSlot.transform);

            //Animate Bow
            character.StartCoroutine(PlayBowAnimation(character));
            character.characterEffectsManager.instantiatedFXModel = loadedArrow;
        }

        private IEnumerator PlayBowAnimation(CharacterManager character)
        {
            yield return new WaitForSeconds(1f);
            Animator bowAnimator = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", true);
            bowAnimator.Play("Bow_TH_Draw_01");
        }
    }
}

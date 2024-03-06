using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI {
    public class ResetAnimatorBool : StateMachineBehaviour
    {
        public string isUsingRightHand = "isUsingRightHand";
        public bool isUsingRightHandStatus = false;

        public string isUsingLeftHand = "isUsingRightHand";
        public bool isUsingLeftHandStatus = false;

        public string isInvulnerable = "isInvulnerable";
        public bool isInvulnerableStatus = false;

        public string isInteractingBool = "isInteracting";
        public bool isInteractingStatus = false;

        public string isFiringSpellBool = "isFiringSpell";
        public bool isFiringSpellStatus = false;

        public string isRotatingWithRootMotion = "isRotatingWithRootMotion";
        public bool isRotatingWithRootMotionStatus = false;

        public string canRotateBool = "canRotate";
        public bool canRotateStatus = true;

        public string isMirroredBool = "isMirrored";
        public bool isMirroredStatus = false;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            CharacterManager character = animator.GetComponent<CharacterManager>();

            if(character != null)
            {
                character.isUsingLeftHand = false;
                character.isUsingRightHand = false;
                character.isAttacking = false;
                character.isBeingBackStabbed = false;
                character.isBeingRiposted = false;
                character.isPerformingBackStab = false;
                character.isPerformingRiposte = false;
                character.canBeParried = false;
                character.canBeRiposted = false;
                character.canRoll = true;

                // After the damage animation ends, reset our previous poise damage to 0
                character.characterCombatManager.previousPoiseDamageTaken = 0;
            }

            animator.SetBool(isInvulnerable, isInvulnerableStatus);
            animator.SetBool(isInteractingBool, isInteractingStatus);
            animator.SetBool(isFiringSpellBool, isFiringSpellStatus);
            animator.SetBool(isRotatingWithRootMotion, isRotatingWithRootMotionStatus);
            animator.SetBool(canRotateBool, canRotateStatus);
            animator.SetBool(isMirroredBool, isMirroredStatus);
        }
    }
}

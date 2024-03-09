using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [CreateAssetMenu(menuName = "A.I/Humanoid Actions/Item Based Attack Action")]
    public class ItemBasedAttackAction : ScriptableObject
    {
        [Header("Attack Type")]
        public AIAttackActionType actionAttackType = AIAttackActionType.meleeAttackAction;
        public AttackType attackType = AttackType.light;

        [Header("Action Combo Settings")]
        public bool actionCanCombo = false;

        [Header("Right Hand Or Left Hand Action")]
        bool isRightHandedAction = true;

        [Header("Action Settings")]
        public int attackScore = 3;
        public float recoveryTime = 2;
        public float maximumAttackAngle = 35;
        public float minimumAttackAngle = -35;
        public float minimumDistanceNeededToAttack = 0;
        public float maximumDistanceNeededToAttack = 3;

        public void PerformAttackAction(AICharacterManager enemy)
        {
            if(isRightHandedAction)
            {
                enemy.UpdateWhichHandCharacterIsUsing(true);
                PerformRightHandItemActionBasedOnAttackType(enemy);
            }
            else
            {
                enemy.UpdateWhichHandCharacterIsUsing(false);
                PerformLeftHandItemActionBasedOnAttackType(enemy);
            }
        }

        //Decide which hand performs the action
        private void PerformRightHandItemActionBasedOnAttackType(AICharacterManager enemy)
        {
            if(actionAttackType == AIAttackActionType.meleeAttackAction)
            {
                PerformRightHandedMeleeAction(enemy);
            }
            else if(actionAttackType == AIAttackActionType.rangedAttackAction)
            {

            }
        }

        private void PerformLeftHandItemActionBasedOnAttackType(AICharacterManager enemy)
        {
            if (actionAttackType == AIAttackActionType.meleeAttackAction)
            {

            }
            else if (actionAttackType == AIAttackActionType.rangedAttackAction)
            {

            }
        }

        //Right Hand Actions
        private void PerformRightHandedMeleeAction(AICharacterManager enemy)
        {
            if(enemy.isTwoHandingWeapon)
            {
                if(attackType == AttackType.light)
                {
                    enemy.characterInventoryManager.rightWeapon.th_tap_Left_Click.PerformAction(enemy);
                }
                else if (attackType == AttackType.heavy)
                {
                    enemy.characterInventoryManager.rightWeapon.th_tap_R_Action.PerformAction(enemy);
                }
            }
            else
            {
                if (attackType == AttackType.light)
                {
                    enemy.characterInventoryManager.rightWeapon.oh_tap_Left_Click.PerformAction(enemy);
                }
                else if (attackType == AttackType.heavy)
                {
                    enemy.characterInventoryManager.rightWeapon.oh_tap_R_Action.PerformAction(enemy);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        public bool t_Input;
        public bool consume_Input;
        public bool y_Input;

        public bool tap_rb_Input;
        public bool hold_rb_Input;
        public bool tap_rt_Input;
        public bool hold_rt_Input;

        public bool block_Input;
        public bool tap_lb_Input;
        public bool tap_lt_Input;

        public bool jump_Input;
        public bool inventory_Input;
        public bool lockOn_Input;
        public bool lockOnRight_Input;
        public bool lockOnLeft_Input;

        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;

        public bool rollFlag;
        public bool twoHandFlag;
        public bool comboFlag;
        public bool lockOnFlag;
        public bool fireFlag;
        public bool inventoryFlag;
        public float rollInputTimer;

        public bool input_Has_Been_Qued;
        public float current_Qued_Input_Timer;
        public float default_Qued_Input_Timer;
        public bool qued_RB_Input;
        public bool qued_LB_Input;
        public bool qued_RT_Input;
        public bool qued_LT_Input;

        PlayerControls inputActions;
        PlayerManager player;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        public void OnEnable()
        {
            if(inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerActions.RB.performed += i => tap_rb_Input = true;

                inputActions.PlayerActions.HoldRB.performed += i => hold_rb_Input = true;
                inputActions.PlayerActions.HoldRB.canceled += i => hold_rb_Input = false;
                inputActions.PlayerActions.HoldRB.canceled += i => tap_rb_Input = true;

                inputActions.PlayerActions.HoldRT.performed += i => hold_rt_Input = true;
                inputActions.PlayerActions.HoldRT.canceled += i => hold_rt_Input = false;

                inputActions.PlayerActions.RT.performed += i => tap_rt_Input = true;
                inputActions.PlayerActions.TapLB.performed += i => tap_lb_Input = true;
                inputActions.PlayerActions.HoldLB.performed += i => block_Input = true;
                inputActions.PlayerActions.HoldLB.canceled += i => block_Input = false;
                inputActions.PlayerActions.LT.performed += i => tap_lt_Input = true;
                inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
                inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
                inputActions.PlayerActions.PickUpItem.performed += i => t_Input = true;
                inputActions.PlayerActions.Consume.performed += i => consume_Input = true;
                inputActions.PlayerActions.Roll.performed += i => b_Input = true;
                inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
                inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
                inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOn_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => lockOnLeft_Input = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => lockOnRight_Input = true;
                inputActions.PlayerActions.Y.performed += i => y_Input = true;

                inputActions.PlayerActions.QuedRB.performed += i => QeuInput(ref qued_RB_Input);
                inputActions.PlayerActions.QuedRT.performed += i => QeuInput(ref qued_RT_Input);
                inputActions.PlayerActions.QuedLB.performed += i => QeuInput(ref qued_LB_Input);
                inputActions.PlayerActions.QuedLT.performed += i => QeuInput(ref qued_LT_Input);
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput()
        {
            if(player.isDead) 
                return;

            HandleMoveInput();
            HandleRollInput();

            HandleHoldRBInput();
            HandleHoldLBInput();
            HandleHoldRTInput();

            HandleTapLBInput();
            HandleTapRBInput();
            HandleTapRTInput();
            HandleTapLTInput();

            HandleQuickSlotInput();
            HandleInventoryInput();

            HandleLockOnInput();
            HandleTwoHandInput();
            HandleUseConsumableInput();
            HandleQuedInput();
        }

        private void HandleMoveInput()
        {
            if (player.isHoldingArrow || player.playerStatsManager.encumbraceLevel == EncumbranceLevel.Overloaded)
            {
                horizontal = movementInput.x;
                vertical = movementInput.y;
                moveAmount = Mathf.Clamp01((Mathf.Abs(horizontal) + Mathf.Abs(vertical)) / 2);
                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
            else
            {
                horizontal = movementInput.x;
                vertical = movementInput.y;
                moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
        }

        private void HandleRollInput()
        {
            if (b_Input)
            {
                rollInputTimer += Time.deltaTime;
                
                if(player.playerStatsManager.currentStamina <= 0)
                {
                    b_Input = false;
                    player.isSprinting = false;
                }

                if(moveAmount > 0.5f && player.playerStatsManager.currentStamina > 0)
                {
                    player.isSprinting = true;
                }
            }
            else
            {
                player.isSprinting = false;

                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }

        private void HandleTapRBInput()
        {            
            if (tap_rb_Input)
            {
                tap_rb_Input = false;

                player.UpdateWhichHandCharacterIsUsing(true);
                player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;

                if (player.isTwoHandingWeapon)
                {
                    if (player.playerInventoryManager.rightWeapon.th_tap_RB_Action != null)
                    {
                        player.playerInventoryManager.rightWeapon.th_tap_RB_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.rightWeapon.oh_tap_RB_Action != null)
                    {
                        player.playerInventoryManager.rightWeapon.oh_tap_RB_Action.PerformAction(player);
                    }
                }
            }
        }

        private void HandleHoldRBInput()
        {
            if (hold_rb_Input)
            {
                player.UpdateWhichHandCharacterIsUsing(true);
                player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;

                if(player.isTwoHandingWeapon)
                {
                    if (player.playerInventoryManager.rightWeapon.th_hold_RB_Action != null)
                    {
                        player.playerInventoryManager.rightWeapon.th_hold_RB_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.rightWeapon.oh_hold_RB_Action != null)
                    {
                        player.playerInventoryManager.rightWeapon.oh_hold_RB_Action.PerformAction(player);
                    }
                }
            }
        }

        private void HandleHoldRTInput()
        {
            player.animator.SetBool("isChargingAttack", hold_rt_Input);
            
            if (hold_rt_Input)
            {
                player.UpdateWhichHandCharacterIsUsing(true);
                player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;

                if (player.isTwoHandingWeapon)
                {
                    if (player.playerInventoryManager.rightWeapon.th_hold_RT_Action != null)
                    {
                        player.playerInventoryManager.rightWeapon.th_hold_RT_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.rightWeapon.oh_hold_RT_Action != null)
                    {

                        player.playerInventoryManager.rightWeapon.oh_hold_RT_Action.PerformAction(player);
                    }
                }
            }
        }

        private void HandleTapRTInput()
        {
            if (tap_rt_Input)
            {
                tap_rt_Input = false;

                if (player.playerInventoryManager.rightWeapon.oh_tap_RT_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.oh_tap_RT_Action.PerformAction(player);
                }
            }
        }

        private void HandleTapLTInput()
        {
            if (tap_lt_Input)
            {
                tap_lt_Input = false;

                if(player.isTwoHandingWeapon)
                {
                    //It will be the right handed weapon
                    if (player.playerInventoryManager.rightWeapon.oh_tap_LT_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                        player.playerInventoryManager.rightWeapon.oh_tap_LT_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.leftWeapon.oh_tap_LT_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.oh_tap_LT_Action.PerformAction(player);
                    }
                }
            }
        }

        private void HandleHoldLBInput()
        {
            if(!player.isGrounded ||
                player.isSprinting ||
                player.isFiringSpell)
            {
                block_Input = false;
                return;
            }
            if (block_Input)
            {
                if (player.isTwoHandingWeapon)
                {
                    if (player.playerInventoryManager.rightWeapon.oh_hold_LB_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                        player.playerInventoryManager.rightWeapon.oh_hold_LB_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.leftWeapon.oh_hold_LB_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.oh_hold_LB_Action.PerformAction(player);
                    }
                }
            }
            else if (block_Input == false)
            {
                if(player.isAiming)
                {
                    player.isAiming = false;
                    player.UIManager.crossHair.SetActive(false);
                    player.cameraHandler.ResetAimCameraRotations();
                }

                if (player.isBlocking)
                {
                    player.isBlocking = false;
                }
            }
        }

        private void HandleTapLBInput()
        {
            if (tap_lb_Input)
            {
                tap_lb_Input = false;

                if (player.isTwoHandingWeapon)
                {
                    if (player.playerInventoryManager.rightWeapon.oh_tap_LB_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                        player.playerInventoryManager.rightWeapon.oh_tap_LB_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.leftWeapon.oh_tap_LB_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.oh_tap_LB_Action.PerformAction(player);
                    }
                }
            }
        }

        private void HandleQuickSlotInput()
        {        

            if (d_Pad_Right)
            {
                player.playerInventoryManager.ChangeRightWeapon();
            }
            else if (d_Pad_Left)
            {
                player.playerInventoryManager.ChangeLeftWeapon();
            }
        }

        private void HandleInventoryInput()
        {
            if (inventoryFlag)
            {
                player.UIManager.UpdateUI();
            }
            if (inventory_Input)
            {
                inventoryFlag = !inventoryFlag;

                if (inventoryFlag)
                {
                    player.UIManager.OpenSelectWindow();
                    player.UIManager.UpdateUI();
                    player.UIManager.hudWindow.SetActive(false);
                }
                else
                {
                    player.UIManager.CloseSelectWindow();
                    player.UIManager.CloseAllInventoryWindows();
                    player.UIManager.hudWindow.SetActive(true);
                }
            }
        }

        private void HandleLockOnInput()
        {
            if (lockOn_Input && lockOnFlag == false)
            {
                lockOn_Input = false;
                player.cameraHandler.HandleLockOn();
                if(player.cameraHandler.nearestLockOnTarget != null)
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOn_Input && lockOnFlag)
            {
                lockOn_Input = false;
                lockOnFlag = false;
                player.cameraHandler.ClearLockOnTargets();
            }

            if(lockOnFlag && lockOnLeft_Input)
            {
                lockOnLeft_Input = false;
                player.cameraHandler.HandleLockOn();
                if(player.cameraHandler.leftLockTarget != null)
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.leftLockTarget;
                }
            } else if (lockOnFlag && lockOnRight_Input)
            {
                lockOnRight_Input = false;
                player.cameraHandler.HandleLockOn();
                if( player.cameraHandler.rightLockTarget != null)
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.rightLockTarget;
                }
            }

            if(player.cameraHandler != null)
            {
                player.cameraHandler.SetCameraHeight();
            }
        }

        private void HandleTwoHandInput()
        {
            if(y_Input)
            {
                y_Input = false;
                twoHandFlag = !twoHandFlag;

                if(twoHandFlag)
                {
                    player.isTwoHandingWeapon = true;
                    player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);
                    player.playerWeaponSlotManager.LoadTwoHandIKTargets(true);
                }
                else
                {
                    player.isTwoHandingWeapon = false;
                    player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);
                    player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.leftWeapon, true);
                    player.playerWeaponSlotManager.LoadTwoHandIKTargets(false);
                }
            }
        }

        private void HandleUseConsumableInput()
        {
            if(consume_Input)
            {
                consume_Input = false;
                player.playerInventoryManager.currentConsumable.AttemptToConsumeItem(player);
            }
        }

        private void QeuInput(ref bool quedInput)
        {
            //Disable all other qued inputs
            qued_LB_Input = false;
            qued_RB_Input = false;
            qued_LT_Input = false;
            qued_RT_Input = false;

            //Enable the referenced input by reference
            //If we are interacting, we can que an input
            if(player.isInteracting)
            {
                quedInput = true;
                current_Qued_Input_Timer = default_Qued_Input_Timer;
                input_Has_Been_Qued = true;
            }
        }

        private void HandleQuedInput()
        {
            if(input_Has_Been_Qued)
            {
                if(current_Qued_Input_Timer > 0)
                {
                    current_Qued_Input_Timer = current_Qued_Input_Timer - Time.deltaTime;
                    ProcessQuedInput();
                }
                else
                {
                    input_Has_Been_Qued = false;
                    current_Qued_Input_Timer = 0;
                }
            }
        }

        private void ProcessQuedInput()
        {
            if (qued_RB_Input)
            {
                tap_rb_Input = true;
            }
            if (qued_RT_Input)
            {
                tap_rt_Input = true;
            }
            if (qued_LB_Input)
            {
                tap_lb_Input = true;
            }
            if (qued_LT_Input)
            {
                tap_lt_Input = true;
            }
        }
    }
}

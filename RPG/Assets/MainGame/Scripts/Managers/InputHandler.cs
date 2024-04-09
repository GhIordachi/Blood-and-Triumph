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

        public bool shift_Input;
        public bool e_Input;
        public bool consume_Input;
        public bool y_Input;

        public bool tap_Left_Click_Input;
        public bool hold_Left_Click_Input;
        public bool tap_R_Input;
        public bool hold_R_Input;

        public bool hold_Right_Click_Input;
        public bool tap_Right_Click_Input;
        public bool tap_Q_Input;

        public bool jump_Input;
        public bool inventory_Input;
        public bool lockOn_Input;
        public bool lockOnRight_Input;
        public bool lockOnLeft_Input;

        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool left_Arrow_Input;
        public bool right_Arrow_Input;

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
        public bool queued_Left_Click_Input;
        public bool queued_Right_Click_Input;
        public bool queued_R_Input;
        public bool queued_Q_Input;

        private bool isResettingTapRInput = false;

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

                inputActions.PlayerActions.LeftClick.performed += i => tap_Left_Click_Input = true;
                inputActions.PlayerActions.HoldLeftClick.performed += i => hold_Left_Click_Input = true;
                inputActions.PlayerActions.HoldLeftClick.canceled += i => hold_Left_Click_Input = false;
                inputActions.PlayerActions.HoldLeftClick.canceled += i => tap_Left_Click_Input = true;

                inputActions.PlayerActions.R.performed += i => tap_R_Input = true;
                inputActions.PlayerActions.HoldR.performed += i => hold_R_Input = true;
                inputActions.PlayerActions.HoldR.canceled += i => hold_R_Input = false;

                inputActions.PlayerActions.RightClick.performed += i => tap_Right_Click_Input = true;
                inputActions.PlayerActions.HoldRightClick.performed += i => hold_Right_Click_Input = true;
                inputActions.PlayerActions.HoldRightClick.canceled += i => hold_Right_Click_Input = false;

                inputActions.PlayerActions.Q.performed += i => tap_Q_Input = true;
                inputActions.PlayerQuickSlots.LeftArrow.performed += i => left_Arrow_Input = true;
                inputActions.PlayerQuickSlots.RightArrow.performed += i => right_Arrow_Input = true;
                inputActions.PlayerActions.PickUpItem.performed += i => e_Input = true;
                inputActions.PlayerActions.Consume.performed += i => consume_Input = true;

                inputActions.PlayerActions.Shift.performed += i => shift_Input = true;
                inputActions.PlayerActions.Shift.canceled += i => shift_Input = false;

                inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
                inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOn_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => lockOnLeft_Input = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => lockOnRight_Input = true;
                inputActions.PlayerActions.Y.performed += i => y_Input = true;

                inputActions.PlayerActions.QueuedLeftClick.performed += i => QueueInput(ref queued_Left_Click_Input);
                inputActions.PlayerActions.QueuedR.performed += i => QueueInput(ref queued_R_Input);
                inputActions.PlayerActions.QueuedRightClick.performed += i => QueueInput(ref queued_Right_Click_Input);
                inputActions.PlayerActions.QueuedQ.performed += i => QueueInput(ref queued_Q_Input);
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
            HandleShiftInput();

            HandleHoldLeftClickInput();
            HandleHoldRightClickInput();
            HandleTapRInput();
            HandleHoldRInput();

            HandleTapRightClickInput();
            HandleTapLeftClickInput();
            HandleTapQInput();

            HandleQuickSlotInput();
            HandleInventoryInput();

            HandleLockOnInput();
            HandleTwoHandInput();
            HandleUseConsumableInput();
            HandleQueuedInput();
        }

        private void HandleMoveInput()
        {
            if (inventoryFlag)
                return;

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

        private void HandleShiftInput()
        {
            if (inventoryFlag)
                return;

            if (shift_Input)
            {
                rollInputTimer += Time.deltaTime;
                
                if(player.playerStatsManager.currentStamina <= 0)
                {
                    shift_Input = false;
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
                if (player.playerStatsManager.currentStamina > 0)
                {
                    if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                    {
                        rollFlag = true;
                    }
                }
                rollInputTimer = 0;                
            }
        }

        //Left Click Input Actions
        private void HandleTapLeftClickInput()
        {
            if (inventoryFlag)
                return;

            if (tap_Left_Click_Input)
            {
                tap_Left_Click_Input = false;
                player.UpdateWhichHandCharacterIsUsing(true);
                player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;

                if (player.isTwoHandingWeapon)
                {
                    if (player.playerInventoryManager.rightWeapon.th_tap_Left_Click != null)
                    {
                        player.playerInventoryManager.rightWeapon.th_tap_Left_Click.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.rightWeapon.oh_tap_Left_Click != null)
                    {
                        player.playerInventoryManager.rightWeapon.oh_tap_Left_Click.PerformAction(player);
                    }
                }
            }
        }

        private void HandleHoldLeftClickInput()
        {
            if (inventoryFlag)
                return;

            if (hold_Left_Click_Input)
            {
                player.UpdateWhichHandCharacterIsUsing(true);
                player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;

                if(player.isTwoHandingWeapon)
                {
                    if (player.playerInventoryManager.rightWeapon.th_hold_Left_Click != null)
                    {
                        player.playerInventoryManager.rightWeapon.th_hold_Left_Click.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.rightWeapon.oh_hold_Left_Click != null)
                    {
                        player.playerInventoryManager.rightWeapon.oh_hold_Left_Click.PerformAction(player);
                    }
                }
            }
        }

        //R Input Actions
        private void HandleTapRInput()
        {
            if (inventoryFlag)
                return;

            if (tap_R_Input)
            {
                tap_R_Input = false;
                player.UpdateWhichHandCharacterIsUsing(true);
                player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;

                if (player.isTwoHandingWeapon)
                {
                    if (player.playerInventoryManager.rightWeapon.th_tap_R_Action != null)
                    {
                        player.playerInventoryManager.rightWeapon.th_tap_R_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.rightWeapon.oh_tap_R_Action != null)
                    {

                        player.playerInventoryManager.rightWeapon.oh_tap_R_Action.PerformAction(player);
                    }
                }
            }
        }

        private void HandleHoldRInput()
        {
            if (inventoryFlag)
                return;

            if (hold_R_Input)
            {
                player.animator.SetBool("isChargingAttack", hold_R_Input);
                player.UpdateWhichHandCharacterIsUsing(true);
                player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;

                if (player.isTwoHandingWeapon)
                {
                    if (player.playerInventoryManager.rightWeapon.th_hold_R_Action != null)
                    {
                        player.playerInventoryManager.rightWeapon.th_hold_R_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.rightWeapon.oh_hold_R_Action != null)
                    {

                        player.playerInventoryManager.rightWeapon.oh_hold_R_Action.PerformAction(player);
                    }
                }
            }
        }

        //Q Input Actions
        private void HandleTapQInput()
        {
            if (inventoryFlag)
                return;

            if (tap_Q_Input)
            {
                tap_Q_Input = false;

                if(player.isTwoHandingWeapon)
                {
                    //It will be the right handed weapon
                    if (player.playerInventoryManager.rightWeapon.oh_tap_Q_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                        player.playerInventoryManager.rightWeapon.oh_tap_Q_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.leftWeapon.oh_tap_Q_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.oh_tap_Q_Action.PerformAction(player);
                    }
                }
            }
        }

        //Right Click Input Actions
        private void HandleHoldRightClickInput()
        {
            if (inventoryFlag)
                return;

            if (!player.isGrounded ||
                player.isSprinting ||
                player.isFiringSpell)
            {
                hold_Right_Click_Input = false;
                return;
            }
            if (hold_Right_Click_Input)
            {
                if (player.isTwoHandingWeapon)
                {
                    if (player.playerInventoryManager.rightWeapon.oh_hold_Right_Click != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                        player.playerInventoryManager.rightWeapon.oh_hold_Right_Click.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.leftWeapon.oh_hold_Right_Click != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.oh_hold_Right_Click.PerformAction(player);
                    }
                }
            }
            else if (hold_Right_Click_Input == false)
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

        private void HandleTapRightClickInput()
        {
            if (inventoryFlag)
                return;

            if (tap_Right_Click_Input)
            {
                tap_Right_Click_Input = false;

                if (player.isTwoHandingWeapon)
                {
                    if (player.playerInventoryManager.rightWeapon.oh_tap_Right_Click != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                        player.playerInventoryManager.rightWeapon.oh_tap_Right_Click.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.leftWeapon.oh_tap_Right_Click != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.oh_tap_Right_Click.PerformAction(player);
                    }
                }
            }
        }


        private void HandleQuickSlotInput()
        {
            if (inventoryFlag)
                return;

            if (right_Arrow_Input)
            {
                player.playerInventoryManager.ChangeRightWeapon();
            }
            else if (left_Arrow_Input)
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
                    if (player.UIManager.levelUpWindow.activeSelf == true)
                    {
                        player.UIManager.CloseAllInventoryWindows();
                        inventoryFlag = !inventoryFlag;
                        return;
                    }
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
            if (inventoryFlag)
                return;

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
            if (inventoryFlag)
                return;

            if (y_Input)
            {
                y_Input = false;
                twoHandFlag = !twoHandFlag;

                if(twoHandFlag && player.playerInventoryManager.rightWeapon.canBeTwoHanded == true)
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
            if (inventoryFlag)
                return;

            if (consume_Input)
            {
                consume_Input = false;
                player.playerInventoryManager.currentConsumable.AttemptToConsumeItem(player);
            }
        }

        private void QueueInput(ref bool quedInput)
        {
            if (inventoryFlag)
                return;

            //Disable all other qued inputs
            queued_Right_Click_Input = false;
            queued_Left_Click_Input = false;
            queued_Q_Input = false;
            queued_R_Input = false;

            //Enable the referenced input by reference
            //If we are interacting, we can que an input
            if(player.isInteracting)
            {
                quedInput = true;
                current_Qued_Input_Timer = default_Qued_Input_Timer;
                input_Has_Been_Qued = true;
            }
        }

        private void HandleQueuedInput()
        {
            if(input_Has_Been_Qued)
            {
                if(current_Qued_Input_Timer > 0)
                {
                    current_Qued_Input_Timer = current_Qued_Input_Timer - Time.deltaTime;
                    ProcessQueuedInput();
                }
                else
                {
                    input_Has_Been_Qued = false;
                    current_Qued_Input_Timer = 0;
                }
            }
        }

        private void ProcessQueuedInput()
        {
            if (queued_Left_Click_Input)
            {
                tap_Left_Click_Input = true;
            }
            if (queued_R_Input)
            {
                tap_R_Input = true;
            }
            if (queued_Right_Click_Input)
            {
                tap_Right_Click_Input = true;
            }
            if (queued_Q_Input)
            {
                tap_Q_Input = true;
            }
        }
    }
}

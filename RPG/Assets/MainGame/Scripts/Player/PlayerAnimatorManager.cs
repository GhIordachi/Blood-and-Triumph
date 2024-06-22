using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace GI
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        PlayerManager player;
        int vertical;
        int horizontal;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical
            float v = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }
            else
            {
                v = 0;
            }
            #endregion

            #region Horizontal
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            #endregion

            if (isSprinting)
            {
                v = 2;
                h = horizontalMovement;
            }

            player.animator.SetFloat(vertical, v, 0.1f, Time.fixedDeltaTime);
            player.animator.SetFloat(horizontal, h, 0.1f, Time.fixedDeltaTime);
        }

        public void DisableCollision()
        {
            player.characterController.enabled = false;
        }

        public void EnableCollision()
        {
            player.characterController.enabled = true;
        }

        public virtual void SuccessfullyUseCurrentConsumable()
        {
            if (character.characterInventoryManager.currentConsumable != null)
            {
                character.characterInventoryManager.currentConsumable.SuccessfullyConsumeItem(player);
            }
        }

        public override void HandleDeath()
        {
            player.characterController.enabled = false;
            player.UIManager.ActivateDeathScreenPopUp();

            StartCoroutine(WaitForLoadAfterDeath());
        }

        private IEnumerator WaitForLoadAfterDeath()
        {
            yield return new WaitForSeconds(3.5f);

            WorldSaveGameManager save = FindObjectOfType<WorldSaveGameManager>();

            save.LoadGame();
        }

        public void AwardSoulsOnDeath()
        {
            
        }

        public void ResetTheArenaBool()
        {
            
        }

        public void DropItemOnDeath()
        {
            
        }
    }
}

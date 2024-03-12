using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class ElevatorInteractable : Interactable
    {
        [Header("Interactable Collider")]
        [SerializeField] Collider interactableCollider;

        [Header("Destination")]
        [SerializeField] Vector3 destinationHigh; // The highest point elevator will travel
        [SerializeField] Vector3 destinationLow;  // The lowest point elevator will travel
        [SerializeField] bool isTravelling = false;
        [SerializeField] bool buttonIsReleased = true;

        [Header("Animator")]
        [SerializeField] Animator elevatorAnimator;
        [SerializeField] string buttonPressAnimation = "Elevator_Button_Press_01";
        [SerializeField] List<CharacterManager> charactersOnButton;

        private void OnTriggerEnter(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if(character != null)
            {
                AddCharacterToListOfCharactersStandingOnButton(character);

                if(!isTravelling && buttonIsReleased)
                {
                    ActivateElevator();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if (character != null)
            {
                StartCoroutine(ReleaseButton(character));
            }
        }

        private void ActivateElevator()
        {
            elevatorAnimator.SetBool("isPressed", true);
            buttonIsReleased = false;
            elevatorAnimator.Play(buttonPressAnimation);
            Debug.Log(transform.position);
            if (transform.position == destinationHigh)
            {
                Debug.Log("aaa");
                StartCoroutine(MoveElevator(destinationLow, 0.8f));
            }
            if(transform.position == destinationLow)
            {
                Debug.Log("bbb");
                StartCoroutine(MoveElevator(destinationHigh, 0.8f));
            }
        }

        private IEnumerator MoveElevator(Vector3 finalPosition, float duration)
        {
            isTravelling = true;

            if(duration > 0)
            {
                float startTime = Time.time;
                float endTime = startTime + duration;
                yield return null;

                while(Time.time < endTime)
                {
                    transform.position = Vector3.Lerp(transform.position, finalPosition, duration * Time.deltaTime);
                    Vector3 movementVelocity = Vector3.Lerp(transform.position, finalPosition, duration * Time.deltaTime);
                    Vector3 characterMovementVelocity = new Vector3(0, movementVelocity.y, 0);

                    foreach(var character in charactersOnButton)
                    {
                        character.characterController.Move(characterMovementVelocity * Time.deltaTime);
                    }

                    yield return null;
                }

                transform.position = finalPosition;
                isTravelling=false;
            }
        }

        private IEnumerator ReleaseButton(CharacterManager character)
        {
            while(isTravelling)
                yield return null;

            yield return new WaitForSeconds(2);

            RemoveCharacterFromListOfCharactersStandingOnButton(character);

            if(charactersOnButton.Count == 0)
            {
                elevatorAnimator.SetBool("isPressed", false);
                buttonIsReleased = true;
            }
        }

        private void AddCharacterToListOfCharactersStandingOnButton(CharacterManager character)
        {
            if (charactersOnButton.Contains(character))
                return;

            charactersOnButton.Add(character);
        }

        private void RemoveCharacterFromListOfCharactersStandingOnButton(CharacterManager character)
        {
            charactersOnButton.Remove(character);

            // Removes null entries from list
            for(int i = charactersOnButton.Count - 1;i > -1; i--)
            {
                if (charactersOnButton[i] == null)
                {
                    charactersOnButton.RemoveAt(i);
                }
            }
        }
    }
}

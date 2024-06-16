using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public class RotateObject : MonoBehaviour
    {
        public float rotationAmount = 1;
        public float rotationSpeed = 5;

        Vector3 currentRotation;
        Vector3 targetRotation;

        private bool rotatingLeft = false;
        private bool rotatingRight = false;

        private void Start()
        {
            currentRotation = transform.eulerAngles;
            targetRotation = transform.eulerAngles;
        }

        private void Update()
        {
            if (rotatingLeft)
            {
                targetRotation.y -= rotationAmount * Time.deltaTime;
            }
            if (rotatingRight)
            {
                targetRotation.y += rotationAmount * Time.deltaTime;
            }
            currentRotation = Vector3.Lerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = currentRotation;
        }

        public void StartRotateLeft()
        {
            rotatingLeft = true;
        }

        public void StartRotateRight()
        {
            rotatingRight = true;
        }

        public void StopRotate()
        {
            rotatingLeft = false;
            rotatingRight = false;
        }
    }
}

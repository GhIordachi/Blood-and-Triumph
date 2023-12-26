using UnityEngine;
using UnityEngine.InputSystem;

namespace RPGCharacterAnims
{
	public class CameraController:MonoBehaviour
	{
		public GameObject cameraTarget;
		public float cameraTargetOffsetY = 1.0f;
		private Vector3 cameraTargetOffset;
		public float rotateSpeed;
		private float rotate;
		public float height = 3.0f;
		public float distance = 5.0f;
		public float zoomAmount = 0.1f;
		public float smoothing = 2.0f;
		private Vector3 offset;
		private bool following = true;
		private Vector3 lastPosition;

		private void Start()
		{
			offset = new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y + height, cameraTarget.transform.position.z - distance);
			lastPosition = new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y + height, cameraTarget.transform.position.z - distance);
			distance = 5;
			height = 3;
		}

		private void FixedUpdate()
		{
			// Follow cam.
			if (Keyboard.current.fKey.isPressed) {
				if (following) { following = false; } else { following = true; }
			}
			if (following) { CameraFollow(); } else { transform.position = lastPosition; }

			// Rotate cam.
			//if (Keyboard.current.qKey.isPressed) { rotate = -1; } else if (Keyboard.current.eKey.isPressed) { rotate = 1; } else { rotate = 0; }

			// Mouse zoom.
			if (Mouse.current.scroll.ReadValue().y > 0f) { distance += zoomAmount; height += zoomAmount; }
			else if (Mouse.current.scroll.ReadValue().y < 0f) { distance -= zoomAmount; height -= zoomAmount; }

			// Set cameraTargetOffset as cameraTarget + cameraTargetOffsetY.
			cameraTargetOffset = cameraTarget.transform.position + new Vector3(0, cameraTargetOffsetY, 0);

			// Smoothly look at cameraTargetOffset.
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(cameraTargetOffset - transform.position), Time.deltaTime * smoothing);
		}

        private void CameraFollow()
        {
            // Rotate the offset based on the character's rotation.
            Quaternion rotation = Quaternion.Euler(0, cameraTarget.transform.eulerAngles.y, 0);
            Vector3 rotatedOffset = rotation * new Vector3(0, height, -distance);

            // Use the rotated offset to set the camera position.
            transform.position = Vector3.Lerp(lastPosition, cameraTarget.transform.position + rotatedOffset, smoothing * Time.deltaTime);

            // Make the camera look at the character.
            transform.LookAt(cameraTarget.transform.position + Vector3.up * cameraTargetOffsetY);
        }


        private void LateUpdate()
		{
			lastPosition = transform.position;
		}
	}
}
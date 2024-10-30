using UnityEngine;

using AstroNut.Managers;

namespace AstroNut.Characters.Player
{
    public class PlayerController : MonoBehaviour
    {
        public float rotationFactor;
        
        // Input values
        private float _currentRotationInput;
        
        private void OnEnable()
        {
            // Subscribe to Rotation event
            if (InputManager.Instance != null)
            {
                InputManager.Instance.RotateEvent += HandleRotation;
            }
        }

        private void OnDisable()
        {
            // Unsubscribe from Rotation event
            if (InputManager.Instance != null)
            {
                InputManager.Instance.RotateEvent -= HandleRotation;
            }
        }

        private void Update()
        {
            Rotate();
        }

        private void Rotate()
        {
            // Continuously apply rotation based on the latest input
            if (_currentRotationInput == 0f) return;
            
            // Heh heh, spin player :)
            float factor = 90 * rotationFactor;
            float rotationAngle = _currentRotationInput * factor;
            transform.Rotate(Vector3.forward, rotationAngle * Time.deltaTime);
        }

        private void HandleRotation(float value)
        {
            // Update the current rotation input to be used in the Update loop.
            _currentRotationInput = value;
        }
    }
}

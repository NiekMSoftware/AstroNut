using System;
using UnityEngine;

using AstroNut.Managers;

namespace AstroNut.Characters.Player
{
    public class PlayerController : MonoBehaviour
    {
        public float rotationFactor;
        
        [Space]
        public float thrustFactor;
        [Range(15f, 30f)] public float thrustValue;
        
        // Input values
        private float _currentRotationInput;
        private float _currentThrustValue;
        
        // Components of player
        private Rigidbody2D _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            // Subscribe to Rotation event
            if (InputManager.Instance == null) return;
            
            InputManager.Instance.RotateEvent += HandleRotation;
            InputManager.Instance.ThrustEvent += HandleThrust;
        }

        private void OnDisable()
        {
            // Unsubscribe from Rotation event
            if (InputManager.Instance == null) return;
            
            InputManager.Instance.RotateEvent -= HandleRotation;
            InputManager.Instance.ThrustEvent -= HandleThrust;
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
            float rotationForce = 90 * rotationFactor;
            float rotationAngle = _currentRotationInput * rotationForce;
            transform.Rotate(Vector3.forward, rotationAngle * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            Thrust();
        }

        private void Thrust()
        {
            // Continuously apply force to the player
            if (_currentThrustValue == 0f) return;
            
            // Calculate the force to apply based on thrust factor and current thrust value
            float force = thrustFactor * thrustValue;
            
            // Apply the force in the player's local up direction
            _rigidbody.AddForce(transform.up * force, ForceMode2D.Force);
        }

        private void HandleRotation(float value)
        {
            // Update the current rotation input to be used in the Update loop.
            _currentRotationInput = value;
        }

        private void HandleThrust(float value)
        {
            // Update the current thrust value
            _currentThrustValue = value;
        }
    }
}

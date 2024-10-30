using UnityEngine;

using System;

using AstroNut.InputActions;

namespace AstroNut.Managers
{
    public class InputManager : MonoBehaviour
    {
        private static InputManager _instance;

        public static InputManager Instance
        {
            get
            {
                // If no instance exists, find one.
                if (_instance == null) _instance = FindFirstObjectByType<InputManager>();
                return _instance;
            }
        }

        // Player Input Actions
        private PlayerInputActions _playerInputActions;
        private PlayerInputActions.PlayerActions _playerActions;
        
        // Define events
        public event Action<float> RotateEvent;
        public event Action<float> ThrustEvent;
        
        private void Awake()
        {
            // If still no instance exists, initialize it
            if (_instance == null) _instance = this;
            
            // Initialize input actions
            _playerInputActions = new PlayerInputActions();
            _playerActions = _playerInputActions.Player;
            
            // Register event handlers
            _playerActions.Rotate.performed += context => OnRotate(context.ReadValue<float>());
            _playerActions.Rotate.canceled += _ => OnRotate(0f);
            
            _playerActions.Thrust.performed += context => OnThrust(context.ReadValue<float>());
            _playerActions.Thrust.canceled += _ => OnThrust(0f);
        }

        private void OnEnable()
        {
            _playerInputActions.Enable();
        }

        private void OnDisable()
        {
            _playerInputActions.Disable();
        }

        #region Event Handling

        private void OnRotate(float value)
        {
            RotateEvent?.Invoke(value);
        }

        private void OnThrust(float value)
        {
            ThrustEvent?.Invoke(value);
        }

        #endregion
    }
}
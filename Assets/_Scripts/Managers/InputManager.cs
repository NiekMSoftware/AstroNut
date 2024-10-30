using System;

using AstroNut.Input;

namespace AstroNut.Managers
{
    public class InputManager : Singleton<InputManager>
    {

        // Player Input Actions
        private PlayerInputActions _playerInputActions;
        private PlayerInputActions.PlayerActions _playerActions;
        
        // Define events
        public event Action<float> RotateEvent;
        
        // Thrust events
        public event Action<float> ThrustEventStart;
        public event Action ThrustEventStop;
        
        protected override void Awake()
        {
            base.Awake();
            
            // Initialize input actions
            _playerInputActions = new PlayerInputActions();
            _playerActions = _playerInputActions.Player;
            
            // Register event handlers
            _playerActions.Rotate.performed += context => OnRotate(context.ReadValue<float>());
            _playerActions.Rotate.canceled += _ => OnRotate(0f);
            
            _playerActions.Thrust.performed += context => OnThrust(context.ReadValue<float>());
            _playerActions.Thrust.canceled += _ => OnThrustStop();
        }

        private void OnEnable()
        {
            // guard clause for null check
            if (!Instance) return;
            _playerInputActions.Enable();
        }

        private void OnDisable()
        {
            Cleanup();
        }

        private void OnApplicationQuit()
        {
            Cleanup();
        }

        protected override void OnDestroy()
        {
            Cleanup();
            base.OnDestroy();
        }

        private void Cleanup()
        {
            if (_playerInputActions != null)
            {
                _playerInputActions.Disable();
                _playerInputActions.Dispose();
                _playerInputActions = null;
            }
            
            RotateEvent = null;
            ThrustEventStart = null;
            ThrustEventStop = null;
        }

        #region Event Handling

        private void OnRotate(float value)
        {
            RotateEvent?.Invoke(value);
        }

        private void OnThrust(float value)
        {
            ThrustEventStart?.Invoke(value);
        }

        private void OnThrustStop()
        {
            ThrustEventStop?.Invoke();
        }

        #endregion
    }
}
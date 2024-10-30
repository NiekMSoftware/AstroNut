using UnityEngine;

using AstroNut.Managers;

namespace AstroNut.Characters.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private JetpackSO jetpack;
        
        // Needed components
        private Rigidbody2D _rigidbody;
        
        // Input values
        private float _rotationInput;
        private float _thrustInput;
        
        // Thrust variables
        private bool _canThrust = true;

        public void Initialize(Rigidbody2D body)
        {
            _rigidbody = body;
        }

        private void OnEnable()
        {
            SubscribeToInputEvents(true);
        }

        private void OnDisable()
        {
            SubscribeToInputEvents(false);
        }

        #region Handle Movement

        public void OnInputRotated()
        {
            Rotate();
        }

        public void OnInputThrust()
        {
            Thrust(_rigidbody);
        }

        private void Rotate()
        {
            // Continuously apply rotation based on the latest input
            if (_rotationInput == 0f) return;  
            
            // Heh heh, spin player :)
            float rotationForce = jetpack.jetpack.rotationSpeed * jetpack.jetpack.rotationFactor;
            float rotationAngle = _rotationInput * rotationForce;
            transform.Rotate(Vector3.forward, rotationAngle * Time.deltaTime);
        }

        private void Thrust(Rigidbody2D body)
        {
            // Continuously apply force to the player
            if (_thrustInput == 0f || !_canThrust) return;
            
            // Calculate the force to apply based on thrust factor and current thrust value
            float force = jetpack.jetpack.thrustFactor * jetpack.jetpack.thrustLevel;
            
            // Apply the force in the player's local up direction
            body.AddForce(transform.up * force, ForceMode2D.Force);
        }

        #endregion

        #region Event Handling

        private void SubscribeToInputEvents(bool subscribe)
        {
            // If no input manager is found return.
            if (InputManager.Instance == null) return;

            // subscribe and unsubscribe
            if (subscribe)
            {
                InputManager.Instance.RotateEvent += HandleRotation;
                InputManager.Instance.ThrustEventStart += HandleThrust;
                InputManager.Instance.ThrustEventStop += HandleThrustStop;
            }
            else
            {
                InputManager.Instance.RotateEvent -= HandleRotation;
                InputManager.Instance.ThrustEventStart -= HandleThrust;
                InputManager.Instance.ThrustEventStop -= HandleThrustStop;
            }
        }

        private void HandleRotation(float value)
        {
            // Update the current rotation input to be used in the Update loop.
            _rotationInput = value;
        }

        private void HandleThrust(float value)
        {
            // Update the current thrust value
            _thrustInput = value;
        }

        private void HandleThrustStop()
        {
            _thrustInput = 0f;
        }

        #endregion

        public void EnableThrust()
        {
            _canThrust = true;
        }
        
        public void DisableThrust()
        {
            _canThrust = false;
        }
    }
}

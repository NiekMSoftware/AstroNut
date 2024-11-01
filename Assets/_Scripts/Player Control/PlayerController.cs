using UnityEngine;

using AstroNut.Input_Handling;

namespace AstroNut.Player_Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private JetpackSO jetpack;
        
        // Needed components
        private Rigidbody2D _rigidbody;
        private RigidbodyConstraints2D _originalConstraints;
        
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
            _originalConstraints = _rigidbody.constraints;
            SubscribeToInputEvents(true);
        }
        
        #region Handle Movement

        public void OnInputRotated()
        {
            // Disable rigidbody rotation
            _rigidbody.constraints = _originalConstraints;
            Rotate();
        }

        public void OnInputThrust()
        {
            Thrust(_rigidbody);
        }

        private void Rotate()
        {
            // Continuously apply rotation based on the latest input
            if (_rotationInput == 0f)
            {
                /* TODO: Ask for some feedback on this.
                // // Rotate the player back to 0 on their respective forward transformation over time
                // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity,  200 * Time.deltaTime);
                */
                
                // Enable rotation again
                _rigidbody.constraints = RigidbodyConstraints2D.None;
                return;
            }  
            
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
            if (InputManager.Instance == null || InputManager.Instance.Equals(null)) return;

            // subscribe and unsubscribe
            if (subscribe)
            {
                InputManager.Instance.RotateEvent += HandleRotation;
                InputManager.Instance.ThrustEventStart += HandleThrust;
                InputManager.Instance.ThrustEventStop += HandleThrustStop;
            }
            else // Save to leave this as is, in case it will ever be needed.
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

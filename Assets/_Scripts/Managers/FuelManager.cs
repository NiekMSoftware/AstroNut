using UnityEngine;

using AstroNut.Characters.Player;

namespace AstroNut.Managers
{
    public class FuelManager : MonoBehaviour
    {
        [SerializeField] private JetpackSO data;

        // Reference to the player controller
        private PlayerController playerController;

        private float _thrustInput;
        
        private void Awake()
        {
            playerController = FindFirstObjectByType<PlayerController>();
        }

        private void OnEnable()
        {
            InputManager.Instance.ThrustEvent += HandleThrustEvent;
        }

        private void OnDisable()
        {
            InputManager.Instance.ThrustEvent -= HandleThrustEvent;
        }

        private void OnDestroy()
        {
            InputManager.Instance.ThrustEvent -= HandleThrustEvent;
        }

        private void HandleThrustEvent(float thrustInput)
        {
            _thrustInput = thrustInput;
            DepleteFuel();
        }

        private void DepleteFuel()
        {
            // Return if jetpack has no fuel left
            if (data.jetpack.fuelLevel <= 0) return;
            
            // Deref the structure to adjust values
            Jetpack jetpack = data.jetpack;
            
            // Deplete fuel based on thrust input and consumption rate
            jetpack.fuelLevel -= _thrustInput * (data.jetpack.fuelConsumptionFactor * Time.deltaTime);
            data.jetpack = jetpack;
            
            // Clamp fuel level to be non-negative
            if (!(data.jetpack.fuelLevel < 0)) return;
            
            // Notify other systems that fuel is depleted
            jetpack.fuelLevel = 0;
            Debug.LogWarning("Fuel depleted.");
            playerController.DisableThrust();
        }
    }
}

using AstroNut.Input;

using System.Collections;
using UnityEngine;
using UnityEngine.Profiling;

namespace AstroNut.Player_Control
{
    public class FuelManager : MonoBehaviour
    {
        [SerializeField] private JetpackSO data;
        private Jetpack _jetpack;

        // Reference to the player controller
        private PlayerController playerController;
        private Coroutine fuelDepletionCoroutine;
        
        private float _currentThrustInput;
        
        private void Awake()
        {
            playerController = FindFirstObjectByType<PlayerController>();
            
            // Initialize Jetpack data
            _jetpack = data.jetpack;
            _jetpack.fuelLevel = data.jetpack.maxFuel;
        }

        private void OnEnable()
        {
            // Subscribe to events
            if (InputManager.Instance == null) return;
            InputManager.Instance.ThrustEventStart += HandleThrustEventStart;
            InputManager.Instance.ThrustEventStop += HandleThrustEventStop;
        }

        private void OnDisable()
        {
            // Stop routine
            if (fuelDepletionCoroutine == null) return;
            StopCoroutine(fuelDepletionCoroutine);
            fuelDepletionCoroutine = null;
        }

        private void HandleThrustEventStart(float thrustInput)
        {
            // Update the current thrust input
            _currentThrustInput = thrustInput;
            fuelDepletionCoroutine ??= StartCoroutine(DepleteFuel(thrustInput));
        }

        private void HandleThrustEventStop()
        {
            // Update current thrust input to 0 when stopping
            _currentThrustInput = 0;
            
            // return if no routine is running
            if (fuelDepletionCoroutine == null) return;
            
            // Stop the routine if it's still runnning.
            StopCoroutine(fuelDepletionCoroutine);
            fuelDepletionCoroutine = null;
        }

        private IEnumerator DepleteFuel(float thrustInput)
        {
            while (true)
            {
                Profiler.BeginSample("Deplete Fuel");
                
                // Break if no input is given.
                if (_currentThrustInput == 0f) yield break;
                
                // Only consider the case for new fuel consumption
                if (_jetpack.fuelLevel <= _jetpack.minFuel)
                {
                    OnFuelDepleted();
                    Profiler.EndSample();
                    yield break;
                }
                
                // Deplete fuel directly
                _jetpack.fuelLevel -= _jetpack.fuelConsumptionFactor * thrustInput * Time.deltaTime;
                
                // Ensure jetpack fuels stays at 0 and handle depletion.
                if (_jetpack.fuelLevel <= data.jetpack.minFuel)
                {
                    _jetpack.fuelLevel = 0;
                    OnFuelDepleted();
                }
                
                Profiler.EndSample();
                yield return null;
            }
        }

        private void OnFuelDepleted()
        {
            print("Fuel Depleted");
            playerController.DisableThrust();
        }
    }
}

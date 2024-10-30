using System.Collections;
using UnityEngine;

using AstroNut.Characters.Player;
using UnityEngine.Profiling;

namespace AstroNut.Managers
{
    public class FuelManager : MonoBehaviour
    {
        [SerializeField] private JetpackSO data;

        // Reference to the player controller
        private PlayerController playerController;
        private Coroutine fuelDepletionCoroutine;
        
        private float _currentThrustInput;
        
        private void Awake()
        {
            playerController = FindFirstObjectByType<PlayerController>();
        }

        private void OnEnable()
        {
            InputManager.Instance.ThrustEventStart += HandleThrustEventStart;
            InputManager.Instance.ThrustEventStop += HandleThrustEventStop;
        }

        private void OnDisable()
        {
            InputManager.Instance.ThrustEventStart -= HandleThrustEventStart;
            InputManager.Instance.ThrustEventStop -= HandleThrustEventStop;
        }

        private void OnDestroy()
        {
            InputManager.Instance.ThrustEventStart -= HandleThrustEventStart;
            InputManager.Instance.ThrustEventStop -= HandleThrustEventStop;
        }

        private void HandleThrustEventStart(float thrustInput)
        {
            _currentThrustInput = thrustInput;
            fuelDepletionCoroutine ??= StartCoroutine(DepleteFuel(thrustInput));
        }

        private void HandleThrustEventStop()
        {
            // Update current thrust input to 0 when stopping
            _currentThrustInput = 0;
            
            // return if no routine is running
            if (fuelDepletionCoroutine == null) return;
            
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
                
                // Break already if fuel is 0.
                if (data.jetpack.fuelLevel <= 0)
                {
                    OnFuelDepleted();
                    yield break;
                }

                // Deref jetpack from data
                Jetpack jetpack = data.jetpack;
                
                // Deplete fuel
                jetpack.fuelLevel -= jetpack.fuelConsumptionFactor * thrustInput * Time.deltaTime;
                
                // Make sure jetpack fuel stays at 0.
                if (jetpack.fuelLevel <= 0)
                {
                    jetpack.fuelLevel = 0;
                    OnFuelDepleted();
                }
                
                // Assign data
                data.jetpack = jetpack;

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

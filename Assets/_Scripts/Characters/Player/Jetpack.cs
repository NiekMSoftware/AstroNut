using UnityEngine;

namespace AstroNut.Characters.Player
{
    [System.Serializable]
    public struct Jetpack
    {
        [field: Header("Jetpack Fuel Properties")]
        [field: SerializeField] public float fuelLevel { get; set; }
        [field: Space, SerializeField] public float minFuel { get; set; }
        [field: SerializeField] public float maxFuel { get; set; }
        
        [field: Header("Jetpack Fuel Consumption")]
        [field: SerializeField] public float fuelConsumption { get; set; }
        
        [field: Space]
        [field: SerializeField] public float minFuelConsumption { get; set; }
        [field: SerializeField] public float maxFuelConsumption { get; set; }
        
        [field: Header("Jetpack Thrust Properties")]
        [field: SerializeField] public float thrustLevel { get; set; }
        [field: Space, SerializeField] public float thrustFactor { get; set; }
        
        [Header("Jetpack Rotation Properties")]
        [field: SerializeField] public float rotationFactor { get; set; }
        [field: SerializeField] public float rotationSpeed { get; set; }
    }
}
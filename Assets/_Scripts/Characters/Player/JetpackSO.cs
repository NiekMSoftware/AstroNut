using UnityEngine;

namespace AstroNut.Characters.Player
{
    [CreateAssetMenu(fileName = "JetpackSO", menuName = "Scriptable Objects/Jetpacks")]
    public class JetpackSO : ScriptableObject
    {
        [field: SerializeField] public Jetpack jetpack { get; set; }
    }
}

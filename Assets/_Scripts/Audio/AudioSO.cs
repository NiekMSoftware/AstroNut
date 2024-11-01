using UnityEngine;
using UnityEngine.Serialization;

namespace AstroNut.Audio
{
    [CreateAssetMenu(fileName = "AudioSO", menuName = "Scriptable Objects/Audio")]
    public class AudioSO : ScriptableObject
    {
        [FormerlySerializedAs("AudioData")]public AudioData audioData;
        [FormerlySerializedAs("AudioName")]public SoundName soundName;
    }
}

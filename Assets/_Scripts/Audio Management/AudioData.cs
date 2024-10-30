using UnityEngine;
using UnityEngine.Audio;

namespace AstroNut.Audio_Management
{
    [System.Serializable]
    public struct AudioData
    {
        [Header("Audio Components")]
        // Audio Components
        public AudioSource source;
        public AudioClip clip;
        
        [Space] 
        public AudioMixerGroup targetMixerGroup;
        
        [Header("Playback Parameters")]
        // Volume
        [Range(0f, 1f)] public float volume;
        
        // Playback
        public bool loop;
    }
}

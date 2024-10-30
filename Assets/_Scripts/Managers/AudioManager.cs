using UnityEngine;

using AstroNut.Audio;
using UnityEngine.Audio;

using System.Collections.Generic;

namespace AstroNut.Managers
{
    public enum SoundName
    {
        BackgroundMusic,
        Ambience_Waves,
        Ambience_Carriage,
        Player_Walk,
        Player_Hurt,
        Player_Death
    }
    
    /// <summary>
    /// Manages and pools all audio related game objects.
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("Audio Settings")] 
        [Tooltip("Please insert the Audio Scriptable Objects in here that you need.")] public AudioSO[] audios;
        [SerializeField] private AudioMixerGroup masterAudioMixerGroup;
        [SerializeField] private AudioMixerGroup musicAudioMixerGroup;
        [SerializeField] private AudioMixerGroup sfxAudioMixerGroup;
        
        private readonly Dictionary<SoundName, AudioSource> _audioDictionary = new();

        protected override void Awake()
        {
            base.Awake();
            
            // Initialize and pool all sounds
            foreach (AudioSO sound in audios)
            {
                InitializeAudio(sound);
            }
        }

        private void InitializeAudio(AudioSO sound)
        {
            // Create a game object
            var soundObject = new GameObject(sound.name);
            AudioData audioData = sound.audioData;  // local audio data object
                
            // Initialize audio
            audioData.source = soundObject.AddComponent<AudioSource>();  // add an audio source to the game object
            audioData.source.clip = sound.audioData.clip;
                
            // Set initial values of clip
            audioData.source.volume = sound.audioData.volume;
            audioData.source.outputAudioMixerGroup = sound.audioData.targetMixerGroup;
            audioData.source.loop = sound.audioData.loop;
                
            // Set the parent of the newly created Game Object to this one.
            soundObject.transform.SetParent(transform);

            _audioDictionary[sound.soundName] = audioData.source;  // Add to the dictionary
        }

        #region Audio Playback Methods

        public void Play(SoundName soundName)
        {
            if (_audioDictionary.TryGetValue(soundName, out AudioSource audioSource))
            {
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning($"Sound '{soundName}' not found.");
            }
        }

        public void Pause(SoundName soundName)
        {
            if (_audioDictionary.TryGetValue(soundName, out AudioSource audioSource))
            {
                audioSource.Pause();
            }
            else
            {
                Debug.LogWarning($"Sound '{soundName}' not found.");
            }
        }

        public void Stop(SoundName soundName)
        {
            if (_audioDictionary.TryGetValue(soundName, out AudioSource audioSource))
            {
                audioSource.Stop();
            }
            else
            {
                Debug.LogWarning($"Sound '{soundName}' not found.");
            }
        }
        
        #endregion
    }
}
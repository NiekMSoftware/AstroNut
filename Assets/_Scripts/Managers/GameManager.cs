using UnityEngine;

namespace AstroNut.Managers
{
    /// <summary>
    /// Manages everything that happens in the game.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        // Create a singleton instance.
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                // if no instance is active, find one.
                if (_instance == null)
                    FindFirstObjectByType<GameManager>();
                
                return _instance;
            }
        }

        private void Awake()
        {
            // If there is an instance, but it isn't the original game object, delete the new one.
            if (_instance != null && _instance != this)
            {
                Debug.LogWarning("More than once instance of GameManager found in the scene. Destroying this one.");
                Destroy(gameObject);
                return;
            }
            
            Debug.Log("Game Manager has been instantiated.");
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}

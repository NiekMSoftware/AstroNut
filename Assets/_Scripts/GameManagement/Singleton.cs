using UnityEngine;

namespace AstroNut.GameManagement
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance != null) return _instance;
                    
                    _instance = FindFirstObjectByType<T>();

                    if (_instance != null) return _instance;
                        
                    var singletonGameObject = new GameObject();
                    _instance = singletonGameObject.AddComponent<T>();
                    singletonGameObject.name = typeof(T) + " (Singleton)";
                    DontDestroyOnLoad(singletonGameObject);
                }
                return _instance;
            }
        }
        
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
                Destroy(gameObject);
        }

        protected virtual void OnDestroy()
        {
            // Clean up singleton
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}
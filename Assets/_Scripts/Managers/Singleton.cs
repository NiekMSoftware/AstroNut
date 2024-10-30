using UnityEngine;

namespace AstroNut.Managers
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();
        private static bool _isShuttingDown = false;

        public static T Instance
        {
            get
            {
                if (_isShuttingDown)
                {
                    Debug.LogWarning($"[Singleton] Instance of '{typeof(T)}' already destroyed. Returning null.");
                    return null;
                }
                
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindFirstObjectByType<T>();

                        if (_instance != null) return _instance;
                        
                        var singletonGameObject = new GameObject();
                        _instance = singletonGameObject.AddComponent<T>();
                        singletonGameObject.name = typeof(T).ToString() + " (Singleton)";
                        DontDestroyOnLoad(singletonGameObject);
                    }
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
            else
                Destroy(gameObject);
        }

        protected virtual void OnApplicationQuit()
        {
            _isShuttingDown = true;
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }
    }
}
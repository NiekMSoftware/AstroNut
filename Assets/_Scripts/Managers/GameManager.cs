using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AstroNut.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public enum GameState
        {
            InMainMenu,
            InGame,
            Exiting
        }

        public enum PauseState
        {
            Paused,
            Unpaused
        }

        [field: SerializeField] public GameState CurrentGameState { get; private set; } = GameState.InGame;
        [field: SerializeField] public PauseState CurrentPauseState { get; private set; } = PauseState.Unpaused;

        public void StartGame()
        {
            CurrentGameState = GameState.InGame;
            CurrentPauseState = PauseState.Unpaused;
            StartCoroutine(LoadSceneAsync("Main Level"));
        }

        public void QuitApplication()
        {
            CurrentGameState = GameState.Exiting;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        }

        public void TogglePause()
        {
            CurrentPauseState = CurrentPauseState == PauseState.Paused ? PauseState.Unpaused : PauseState.Paused;
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (asyncLoad != null && !asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}

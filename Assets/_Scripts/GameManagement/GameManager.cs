using System.Collections;
using AstroNut.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AstroNut.GameManagement
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

        private static IEnumerator LoadSceneAsync(string sceneName)
        {
            UIManager.Instance.ShowLoadingScreen();
            
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            while (asyncLoad is { isDone: false })
            {
                UIManager.Instance.UpdateLoadingProgress(asyncLoad.progress);
                yield return null;
            }
            
            UIManager.Instance.UpdateLoadingProgress(1f);
            UIManager.Instance.HideLoadingScreen();
        }
    }
}

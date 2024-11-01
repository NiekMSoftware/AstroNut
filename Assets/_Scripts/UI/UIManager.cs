using System.Collections;
using AstroNut.GameManagement;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace AstroNut.UI
{
    #region UI Structures

    [System.Serializable]
    public struct UILabel
    {
        public TMP_Text label;
    }
    
    [System.Serializable]
    public struct UIButton
    {
        public Button button;
        [FormerlySerializedAs("text")] public TMP_Text label;
    }

    [System.Serializable]
    public struct UISlider
    {
        public Slider slider;
        public TMP_Text text;
    }

    [System.Serializable]
    public struct UIPanel
    {
        public GameObject panel;
        public TMP_Text header;
        
        // Optional buttons/sliders
        [Tooltip("The UIButton container.")] public UIButton[] buttons;
        [Tooltip("The UISlider container.")] public UISlider[] sliders;
    }

    [System.Serializable]
    public struct LoadingScreen
    {
        public GameObject transition;
        public GameObject panel;
        public TMP_Text loadingText;

        [Space] 
        public Animator animator;
        public UISlider slider;
    }

    #endregion
    
    public class UIManager : Singleton<UIManager>
    {
        [Header("Main Menu UI Elements")]
        [SerializeField] private UIPanel mainMenuPanel;
        [SerializeField] private UILabel gameVersion;

        [field: Header("Loading Screen")]
        [field: SerializeField] public LoadingScreen loadingScreen { get; private set; }

        private void Start()
        {
            SetupMainMenu();
        }
        
        #region Main Menu Button Setup
        
        private void SetupMainMenu()
        {
            if (mainMenuPanel.panel != null)
            {
                mainMenuPanel.panel.SetActive(true);
                
                // Set necessary text
                mainMenuPanel.header.text = "AstroNut";
                gameVersion.label.text = Application.version;
                
                // Take out the buttons (e.g. Start, Options & Quit) and apply appropriate events.
                if (mainMenuPanel.buttons.Length > 0)
                {
                    foreach (UIButton uiButton in mainMenuPanel.buttons)
                    {
                        switch (uiButton.label.text)
                        {
                            case "Play":
                                uiButton.button.onClick.AddListener(OnPlayButtonClicked);
                                break;
                            
                            case "Options":
                                uiButton.button.onClick.AddListener(OnOptionsButtonClicked);
                                break;
                            
                            case "Quit":
                                uiButton.button.onClick.AddListener(OnQuitButtonClicked);
                                break;
                        }
                    }
                }
            }
        }
        
        private void OnPlayButtonClicked()
        {
            mainMenuPanel.panel.SetActive(false);
            GameManager.Instance.StartGame();
        }

        private void OnOptionsButtonClicked()
        {
            Debug.Log("OnOptionsButtonClicked");
        }

        private void OnQuitButtonClicked()
        {
            GameManager.Instance.QuitApplication();
        }
        
        #endregion

        #region LoadingScreen
        
        public void ShowLoadingScreen()
        {
            loadingScreen.panel.SetActive(true);
        }

        public void HideLoadingScreen()
        {
            loadingScreen.panel.SetActive(false);
            loadingScreen.transition.SetActive(false);
        }

        public void UpdateLoadingProgress(float progress, string message = "")
        {
            loadingScreen.slider.slider.value = progress;
            int progressPercentage = Mathf.RoundToInt(progress * 100);
            loadingScreen.loadingText.text = string.IsNullOrEmpty(message) ? "Loading..." : message;
            loadingScreen.slider.text.text = $"{progressPercentage.ToString()}%";
        }

        #endregion
    }
}
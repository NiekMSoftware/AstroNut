using TMPro;

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using AstroNut.GameManagement;

namespace AstroNut.UI_Management
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

    #endregion
    
    public class UIManager : Singleton<UIManager>
    {
        [Header("Main Menu UI Elements")]
        [SerializeField] private UIPanel mainMenuPanel;
        [SerializeField] private UILabel gameVersion;

        private void Start()
        {
            SetupMainMenu();
        }
        
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

        #region Main Menu Button Setup
        
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
    }
}
using TMPro;

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace AstroNut.Managers
{
    #region UI Structures

    [System.Serializable]
    public struct UIButton
    {
        public Button button;
        public TMP_Text text;
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
    }
}
using UnityEngine;

namespace AnakRantau.UI
{
    /// <summary>
    /// Basic placeholder UI shell for the player HUD.
    /// We keep this intentionally small for Level 1.
    /// </summary>
    public sealed class PlayerInterfaceController : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject mapPanel;
        [SerializeField] private GameObject statusPanel;
        [SerializeField] private GameObject phonePanel;

        [Header("Buttons")]
        [SerializeField] private UnityEngine.UI.Button mapButton;
        [SerializeField] private UnityEngine.UI.Button statusButton;
        [SerializeField] private UnityEngine.UI.Button phoneButton;
        [SerializeField] private UnityEngine.UI.Button closeStatusButton;
        [SerializeField] private UnityEngine.UI.Button closePhoneButton;

        private void Awake()
        {
            WireButton(mapButton, OpenMap);
            WireButton(statusButton, OpenStatus);
            WireButton(phoneButton, OpenPhone);
            WireButton(closeStatusButton, CloseAllPanels);
            WireButton(closePhoneButton, CloseAllPanels);
        }

        private void Start()
        {
            ShowOnly(mapPanel);
        }

        private static void WireButton(UnityEngine.UI.Button button, UnityEngine.Events.UnityAction action)
        {
            if (button != null)
            {
                button.onClick.AddListener(action);
            }
        }

        public void OpenMap()
        {
            ShowOnly(mapPanel);
        }

        public void OpenStatus()
        {
            ShowOnly(statusPanel);
        }

        public void OpenPhone()
        {
            ShowOnly(phonePanel);
        }

        public void CloseAllPanels()
        {
            ShowOnly(null);
        }

        private void ShowOnly(GameObject activePanel)
        {
            SetPanelActive(mapPanel, activePanel == mapPanel);
            SetPanelActive(statusPanel, activePanel == statusPanel);
            SetPanelActive(phonePanel, activePanel == phonePanel);
        }

        private static void SetPanelActive(GameObject panel, bool isActive)
        {
            if (panel != null)
            {
                panel.SetActive(isActive);
            }
        }
    }
}

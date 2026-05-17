using UnityEngine;
using UnityEngine.UI;

using AnakRantau.Core;
using AnakRantau.SceneManagement;

namespace AnakRantau.UI
{
    /// <summary>
    /// Basic main menu controller for Level 1.
    /// </summary>
    public sealed class MainMenuController : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button continueButton;

        [Header("Settings Panel")]
        [SerializeField] private GameObject settingsPanel;

        private void Start()
        {
            if (Bootstrapper.HasInstance)
            {
                Bootstrapper.Instance.SetGameState(AppState.MainMenu);
            }

            if (continueButton != null)
            {
                continueButton.interactable = false;
            }

            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
            }
        }

        public void OnStartGameClicked()
        {
            SceneFlow.StartLevel1Game();
        }

        public void OnContinueClicked()
        {
            Debug.Log("Continue is disabled for Level 1.");
        }

        public void OnSettingsClicked()
        {
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(true);
            }
        }

        public void OnCloseSettingsClicked()
        {
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
            }
        }

        public void OnExitClicked()
        {
            if (Application.isEditor)
            {
                Debug.Log("Exit clicked in editor. Application.Quit() is ignored in Play Mode.");
                return;
            }

            Application.Quit();
        }
    }
}

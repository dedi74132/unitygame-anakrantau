using UnityEngine.SceneManagement;

using AnakRantau.Core;

namespace AnakRantau.SceneManagement
{
    /// <summary>
    /// Convenience entry points for the simple scene flow used in Level 1.
    /// </summary>
    public static class SceneFlow
    {
        public static void GoToMainMenu()
        {
            LoadViaLoadingScreen(SceneNames.MainMenu, "Menyiapkan menu utama...");
        }

        public static void StartLevel1Game()
        {
            LoadViaLoadingScreen(SceneNames.GameLevel1Room, "Memuat kamar kost...");
        }

        public static void LoadViaLoadingScreen(string targetSceneName, string loadingMessage)
        {
            SceneTransitionContext.SetRequest(targetSceneName, loadingMessage);

            if (Bootstrapper.HasInstance)
            {
                Bootstrapper.Instance.SetGameState(AppState.Loading);
                Bootstrapper.Instance.StartCoroutine(SceneLoader.LoadSceneAsync(SceneNames.LoadingScreen));
                return;
            }

            SceneManager.LoadSceneAsync(SceneNames.LoadingScreen);
        }
    }
}

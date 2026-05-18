using UnityEngine;
using UnityEngine.SceneManagement;

namespace AnakRantau.Core
{
    /// <summary>
    /// If Play Mode starts from a non-bootstrap scene in the Editor, bounce into Bootstrap
    /// so the intended flow is always visible.
    /// </summary>
    public static class AutoBootstrapAtPlay
    {
        private static bool hasRedirected;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void RedirectToBootstrapIfNeeded()
        {
            if (hasRedirected)
            {
                return;
            }

            hasRedirected = true;

            if (!Application.isPlaying)
            {
                return;
            }

            Scene activeScene = SceneManager.GetActiveScene();
            if (activeScene.name == SceneNames.Bootstrap)
            {
                return;
            }

            if (Application.CanStreamedLevelBeLoaded(SceneNames.Bootstrap))
            {
                SceneManager.LoadScene(SceneNames.Bootstrap);
            }
            else
            {
                Debug.LogWarning($"Bootstrap scene '{SceneNames.Bootstrap}' is not available in Build Settings.");
            }
        }
    }
}

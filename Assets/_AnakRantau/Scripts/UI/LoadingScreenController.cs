using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using AnakRantau.Core;
using AnakRantau.SceneManagement;

namespace AnakRantau.UI
{
    /// <summary>
    /// Shows loading progress and pushes the next scene once the loading screen is ready.
    /// </summary>
    public sealed class LoadingScreenController : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Slider progressBar;
        [SerializeField] private Text loadingText;
        [SerializeField] private Text progressLabel;

        [Header("Fallback Text")]
        [SerializeField] private string defaultLoadingMessage = "Memuat...";

        [Header("Timing")]
        [SerializeField] private float minimumVisibleSeconds = 5.0f;

        private void Start()
        {
            if (Bootstrapper.HasInstance)
            {
                Bootstrapper.Instance.SetGameState(AppState.Loading);
            }

            string targetSceneName = SceneTransitionContext.HasRequest
                ? SceneTransitionContext.TargetSceneName
                : SceneNames.MainMenu;

            string message = SceneTransitionContext.HasRequest
                ? SceneTransitionContext.LoadingMessage
                : defaultLoadingMessage;

            SceneTransitionContext.Clear();

            if (loadingText != null)
            {
                loadingText.text = message;
            }

            if (progressBar != null)
            {
                progressBar.value = 0f;
            }

            if (progressLabel != null)
            {
                progressLabel.text = "0%";
            }

            StartCoroutine(LoadTargetScene(targetSceneName));
        }

        private IEnumerator LoadTargetScene(string targetSceneName)
        {
            if (minimumVisibleSeconds > 0f)
            {
                yield return new WaitForSecondsRealtime(minimumVisibleSeconds);
            }

            if (!Application.CanStreamedLevelBeLoaded(targetSceneName))
            {
                Debug.LogError($"Scene '{targetSceneName}' is not added to Build Settings.");
                yield break;
            }

            yield return StartCoroutine(SceneLoader.LoadSceneAsync(
                targetSceneName,
                progress =>
                {
                    if (progressBar != null)
                    {
                        progressBar.value = progress;
                    }

                    if (progressLabel != null)
                    {
                        int percent = Mathf.RoundToInt(progress * 100f);
                        progressLabel.text = $"{percent}%";
                    }
                },
                holdAtMaxProgress: true));
        }
    }
}

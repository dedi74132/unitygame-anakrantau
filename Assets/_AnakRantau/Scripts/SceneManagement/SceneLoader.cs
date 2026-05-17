using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AnakRantau.SceneManagement
{
    /// <summary>
    /// Small coroutine-based scene loading helper.
    /// Uses SceneManager.LoadSceneAsync under the hood.
    /// </summary>
    public static class SceneLoader
    {
        public static IEnumerator LoadSceneAsync(
            string sceneName,
            Action<float> onProgress = null,
            bool holdAtMaxProgress = false)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);

            if (loadOperation == null)
            {
                Debug.LogError($"Failed to start loading scene: {sceneName}");
                yield break;
            }

            if (holdAtMaxProgress)
            {
                loadOperation.allowSceneActivation = false;
            }

            while (!loadOperation.isDone)
            {
                float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
                onProgress?.Invoke(progress);

                if (holdAtMaxProgress && loadOperation.progress >= 0.9f)
                {
                    onProgress?.Invoke(1f);
                    loadOperation.allowSceneActivation = true;
                }

                yield return null;
            }

            onProgress?.Invoke(1f);
        }
    }
}

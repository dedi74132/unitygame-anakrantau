namespace AnakRantau.SceneManagement
{
    /// <summary>
    /// Temporary data shared between scene requests and the loading screen.
    /// </summary>
    public static class SceneTransitionContext
    {
        public static string TargetSceneName { get; private set; } = string.Empty;

        public static string LoadingMessage { get; private set; } = "Memuat...";

        public static bool HasRequest => !string.IsNullOrWhiteSpace(TargetSceneName);

        public static void SetRequest(string targetSceneName, string loadingMessage)
        {
            TargetSceneName = targetSceneName;
            LoadingMessage = string.IsNullOrWhiteSpace(loadingMessage) ? "Memuat..." : loadingMessage;
        }

        public static void Clear()
        {
            TargetSceneName = string.Empty;
            LoadingMessage = "Memuat...";
        }
    }
}

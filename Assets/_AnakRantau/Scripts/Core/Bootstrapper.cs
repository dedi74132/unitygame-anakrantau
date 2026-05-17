using UnityEngine;

using AnakRantau.SceneManagement;

namespace AnakRantau.Core
{
    /// <summary>
    /// Persistent entry point that creates core services once.
    /// Place this only in the Bootstrap scene.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class Bootstrapper : MonoBehaviour
    {
        public static Bootstrapper Instance { get; private set; }

        public GameStateService GameState { get; private set; }

        public static bool HasInstance => Instance != null;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            GameState = new GameStateService();
            GameState.SetState(AppState.Bootstrap);
        }

        private void Start()
        {
            SceneFlow.GoToMainMenu();
        }

        public void SetGameState(AppState state)
        {
            GameState?.SetState(state);
        }
    }
}

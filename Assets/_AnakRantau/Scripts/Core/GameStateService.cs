using System;

namespace AnakRantau.Core
{
    /// <summary>
    /// Tiny state holder for the current app flow.
    /// </summary>
    public sealed class GameStateService
    {
        public AppState CurrentState { get; private set; } = AppState.Bootstrap;

        public event Action<AppState> StateChanged;

        public void SetState(AppState newState)
        {
            if (CurrentState == newState)
            {
                return;
            }

            CurrentState = newState;
            StateChanged?.Invoke(CurrentState);
        }
    }
}

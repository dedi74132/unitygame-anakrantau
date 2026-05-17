using UnityEngine;
using UnityEngine.UI;

using AnakRantau.Core;

namespace AnakRantau.Game
{
    /// <summary>
    /// Placeholder controller for the first playable room.
    /// </summary>
    public sealed class GameLevel1RoomController : MonoBehaviour
    {
        [SerializeField] private Text roomLabel;
        [SerializeField] private string placeholderText = "Kamar Kost Level 1";

        private void Start()
        {
            if (Bootstrapper.HasInstance)
            {
                Bootstrapper.Instance.SetGameState(AppState.InGame);
            }

            if (roomLabel != null)
            {
                roomLabel.text = placeholderText;
            }
        }
    }
}

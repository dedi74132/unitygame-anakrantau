using UnityEngine;
using UnityEngine.UI;

using AnakRantau.Core;

namespace AnakRantau.Game
{
    /// <summary>
    /// Entry controller for the Level 1 gameplay scene.
    /// For now this scene behaves like a map + HUD placeholder.
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

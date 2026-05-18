using System;
using UnityEngine;
using UnityEngine.UI;

namespace AnakRantau.Game
{
    /// <summary>
    /// Placeholder map controller for Level 1.
    /// It lets us preview area selection and area descriptions before gameplay exists.
    /// </summary>
    public sealed class Level1MapController : MonoBehaviour
    {
        [Serializable]
        private class AreaInfo
        {
            public Level1AreaId areaId;
            public string displayName;
            [TextArea(2, 5)] public string description;
        }

        [Header("UI")]
        [SerializeField] private Text mapTitleText;
        [SerializeField] private Text selectedAreaText;
        [SerializeField] private Text selectedAreaDescriptionText;

        [Header("Area Buttons")]
        [SerializeField] private Button terminalButton;
        [SerializeField] private Button jalanRayaButton;
        [SerializeField] private Button gangKosButton;
        [SerializeField] private Button warungButton;
        [SerializeField] private Button areaKerjaAwalButton;
        [SerializeField] private Button kosPemainButton;

        [Header("Area Data")]
        [SerializeField] private AreaInfo[] areas;

        private void Reset()
        {
            EnsureDefaultAreas();
        }

        private void OnValidate()
        {
            EnsureDefaultAreas();
        }

        private void Awake()
        {
            EnsureDefaultAreas();
            WireButton(terminalButton, Level1AreaId.Terminal);
            WireButton(jalanRayaButton, Level1AreaId.JalanRaya);
            WireButton(gangKosButton, Level1AreaId.GangKos);
            WireButton(warungButton, Level1AreaId.Warung);
            WireButton(areaKerjaAwalButton, Level1AreaId.AreaKerjaAwal);
            WireButton(kosPemainButton, Level1AreaId.KosPemain);
        }

        private void Start()
        {
            if (mapTitleText != null)
            {
                mapTitleText.text = "Peta Level 1";
            }

            SelectArea(Level1AreaId.Terminal);
        }

        private void WireButton(Button button, Level1AreaId areaId)
        {
            if (button == null)
            {
                return;
            }

            button.onClick.AddListener(() => SelectArea(areaId));
        }

        public void SelectArea(Level1AreaId areaId)
        {
            AreaInfo area = FindArea(areaId);
            if (area == null)
            {
                return;
            }

            if (selectedAreaText != null)
            {
                selectedAreaText.text = area.displayName;
            }

            if (selectedAreaDescriptionText != null)
            {
                selectedAreaDescriptionText.text = area.description;
            }
        }

        private AreaInfo FindArea(Level1AreaId areaId)
        {
            if (areas == null)
            {
                return null;
            }

            for (int i = 0; i < areas.Length; i++)
            {
                if (areas[i] != null && areas[i].areaId == areaId)
                {
                    return areas[i];
                }
            }

            return null;
        }

        private void EnsureDefaultAreas()
        {
            if (areas != null && areas.Length > 0)
            {
                return;
            }

            areas = new[]
            {
                new AreaInfo
                {
                    areaId = Level1AreaId.Terminal,
                    displayName = "Terminal",
                    description = "Area awal pemain turun dari bus. Tempat pertama untuk memulai perjalanan di kota."
                },
                new AreaInfo
                {
                    areaId = Level1AreaId.JalanRaya,
                    displayName = "Jalan Raya",
                    description = "Jalur utama menuju area lain. Cocok untuk transisi antar lokasi."
                },
                new AreaInfo
                {
                    areaId = Level1AreaId.GangKos,
                    displayName = "Gang Kos",
                    description = "Gang sempit menuju area kos pemain dan lingkungan sekitar."
                },
                new AreaInfo
                {
                    areaId = Level1AreaId.Warung,
                    displayName = "Warung",
                    description = "Tempat beli makan, minum, dan kebutuhan sederhana untuk anak rantau."
                },
                new AreaInfo
                {
                    areaId = Level1AreaId.AreaKerjaAwal,
                    displayName = "Area Kerja Awal",
                    description = "Area pekerjaan sederhana untuk mulai menghasilkan uang di awal game."
                },
                new AreaInfo
                {
                    areaId = Level1AreaId.KosPemain,
                    displayName = "Kos Pemain",
                    description = "Tempat tinggal utama pemain. Nanti bisa dikembangkan seiring progres."
                }
            };
        }
    }
}

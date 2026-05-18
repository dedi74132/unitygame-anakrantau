using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using AnakRantau.Game;
using AnakRantau.SceneManagement;
using AnakRantau.UI;

namespace AnakRantau.Core
{
    /// <summary>
    /// Runtime fallback UI so empty scenes still show visible content during early development.
    /// </summary>
    public static class RuntimeSceneUiFactory
    {
        private const float MinimumVisibleSeconds = 5f;
        private static Font builtinFont;
        private static Sprite solidSprite;

        public static bool HasCanvas(Scene scene)
        {
            if (!scene.IsValid())
            {
                return false;
            }

            GameObject[] roots = scene.GetRootGameObjects();
            for (int i = 0; i < roots.Length; i++)
            {
                if (roots[i].GetComponentInChildren<Canvas>(true) != null)
                {
                    return true;
                }
            }

            return false;
        }

        public static void BuildLoadingScreen(Bootstrapper owner)
        {
            Scene scene = SceneManager.GetActiveScene();
            if (HasCanvas(scene))
            {
                return;
            }

            EnsureBuiltinAssets();

            GameObject canvas = CreateCanvas("RuntimeLoadingCanvas");
            CreateFullscreenPanel(canvas.transform, "Background", new Color(0.08f, 0.10f, 0.08f, 1f));

            GameObject card = CreatePanel(canvas.transform, "LoadingCard", new Color(0.14f, 0.16f, 0.14f, 0.96f), new Vector2(760f, 460f));
            SetRect(card.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(760f, 460f), Vector2.zero);

            CreateText(card.transform, "Title", "ANAK RANTAU", 54, TextAnchor.UpperCenter, Color.white, new Vector2(0f, -10f), new Vector2(680f, 90f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f));
            CreateText(card.transform, "LoadingText", "Memuat...", 28, TextAnchor.MiddleCenter, Color.white, new Vector2(0f, -110f), new Vector2(680f, 90f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
            CreateText(card.transform, "TipText", "Menyiapkan perjalanan anak rantau...", 20, TextAnchor.UpperCenter, new Color(0.85f, 0.82f, 0.74f), new Vector2(0f, -180f), new Vector2(680f, 60f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));

            Slider slider = CreateSlider(card.transform, "ProgressBar");
            SetRect(slider.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(640f, 34f), new Vector2(0f, -250f));
            slider.value = 0f;

            owner.StartCoroutine(LoadTargetAfterLoadingScreen(slider));
        }

        public static void BuildMainMenu()
        {
            Scene scene = SceneManager.GetActiveScene();
            if (HasCanvas(scene))
            {
                return;
            }

            EnsureBuiltinAssets();

            GameObject canvas = CreateCanvas("RuntimeMainMenuCanvas");
            CreateFullscreenPanel(canvas.transform, "Background", new Color(0.94f, 0.90f, 0.83f, 1f));

            GameObject leftPanel = CreatePanel(canvas.transform, "BrandPanel", new Color(0.96f, 0.93f, 0.86f, 0.92f), new Vector2(440f, 1920f));
            SetRect(leftPanel.GetComponent<RectTransform>(), new Vector2(0f, 0f), new Vector2(0.43f, 1f), new Vector2(0f, 0.5f), Vector2.zero, Vector2.zero);

            CreateText(leftPanel.transform, "GameTitle", "ANAK RANTAU", 60, TextAnchor.UpperLeft, new Color(0.22f, 0.15f, 0.08f), new Vector2(48f, -70f), new Vector2(520f, 120f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f));
            CreateText(leftPanel.transform, "Subtitle", "ExplosionCrash Gameplay", 24, TextAnchor.UpperLeft, new Color(0.28f, 0.20f, 0.12f), new Vector2(52f, -150f), new Vector2(480f, 60f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f));
            CreateText(leftPanel.transform, "Description", "Mulai sebagai anak rantau, lalu bertahan, bekerja, berkembang, dan membangun hidup yang lebih mapan.", 24, TextAnchor.UpperLeft, new Color(0.24f, 0.20f, 0.16f), new Vector2(52f, -240f), new Vector2(440f, 220f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f));

            GameObject menuPanel = CreatePanel(canvas.transform, "MenuPanel", new Color(0.10f, 0.11f, 0.10f, 0.92f), new Vector2(640f, 1260f));
            SetRect(menuPanel.GetComponent<RectTransform>(), new Vector2(0.50f, 0.18f), new Vector2(0.90f, 0.88f), new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);

            CreateText(menuPanel.transform, "MenuHeader", "Menu Utama", 34, TextAnchor.UpperCenter, Color.white, new Vector2(0f, -30f), new Vector2(420f, 60f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f));

            Button start = CreateButton(menuPanel.transform, "StartGameButton", "Start Game", new Color(0.23f, 0.43f, 0.21f, 1f), new Vector2(0f, -110f), new Vector2(320f, 74f));
            Button cont = CreateButton(menuPanel.transform, "ContinueButton", "Continue", new Color(0.34f, 0.26f, 0.17f, 1f), new Vector2(0f, -200f), new Vector2(320f, 74f));
            Button settings = CreateButton(menuPanel.transform, "SettingsButton", "Settings", new Color(0.34f, 0.26f, 0.17f, 1f), new Vector2(0f, -290f), new Vector2(320f, 74f));
            Button exit = CreateButton(menuPanel.transform, "ExitButton", "Exit", new Color(0.42f, 0.18f, 0.16f, 1f), new Vector2(0f, -380f), new Vector2(320f, 74f));

            GameObject settingsPanel = CreatePanel(canvas.transform, "SettingsPanel", new Color(0.22f, 0.24f, 0.16f, 0.96f), new Vector2(520f, 420f));
            SetRect(settingsPanel.GetComponent<RectTransform>(), new Vector2(0.55f, 0.5f), new Vector2(0.88f, 0.72f), new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            settingsPanel.SetActive(false);
            CreateText(settingsPanel.transform, "SettingsTitle", "Settings", 34, TextAnchor.UpperCenter, Color.white, new Vector2(0f, -22f), new Vector2(320f, 50f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f));
            CreateText(settingsPanel.transform, "SettingsBody", "Placeholder settings panel.\nNanti kita isi audio, kontrol, dan kualitas.", 22, TextAnchor.MiddleCenter, new Color(0.95f, 0.92f, 0.85f), new Vector2(0f, -98f), new Vector2(320f, 130f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
            Button close = CreateButton(settingsPanel.transform, "CloseButton", "Close", new Color(0.47f, 0.26f, 0.16f, 1f), new Vector2(0f, -220f), new Vector2(180f, 54f));

            var controllerObject = new GameObject("RuntimeMainMenuController");
            var controller = controllerObject.AddComponent<MainMenuController>();

            start.onClick.AddListener(controller.OnStartGameClicked);
            cont.onClick.AddListener(controller.OnContinueClicked);
            settings.onClick.AddListener(controller.OnSettingsClicked);
            exit.onClick.AddListener(controller.OnExitClicked);
            close.onClick.AddListener(controller.OnCloseSettingsClicked);

        }

        public static void BuildGameScene()
        {
            Scene scene = SceneManager.GetActiveScene();
            if (HasCanvas(scene))
            {
                return;
            }

            EnsureBuiltinAssets();

            GameObject canvas = CreateCanvas("RuntimeGameCanvas");
            CreateFullscreenPanel(canvas.transform, "Background", new Color(0.08f, 0.10f, 0.11f, 1f));

            CreateText(canvas.transform, "RoomLabel", "Kamar Kost Level 1", 32, TextAnchor.UpperLeft, Color.white, new Vector2(28f, -24f), new Vector2(420f, 54f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f));

            GameObject mapPanel = CreatePanel(canvas.transform, "MapPanel", new Color(0.95f, 0.92f, 0.85f, 0.98f), new Vector2(760f, 1440f));
            SetRect(mapPanel.GetComponent<RectTransform>(), new Vector2(0f, 0.14f), new Vector2(0.72f, 0.90f), new Vector2(0f, 0.5f), Vector2.zero, Vector2.zero);

            CreateText(mapPanel.transform, "MapTitle", "Peta Level 1", 34, TextAnchor.UpperLeft, new Color(0.25f, 0.18f, 0.10f), new Vector2(24f, -18f), new Vector2(300f, 50f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f));
            Text selectedArea = CreateText(mapPanel.transform, "SelectedArea", "Terminal", 28, TextAnchor.UpperLeft, new Color(0.18f, 0.14f, 0.10f), new Vector2(24f, -76f), new Vector2(300f, 40f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f));
            Text selectedDesc = CreateText(mapPanel.transform, "SelectedAreaDescription", "Area awal pemain turun dari bus. Tempat pertama untuk memulai perjalanan di kota.", 20, TextAnchor.UpperLeft, new Color(0.23f, 0.18f, 0.14f), new Vector2(24f, -124f), new Vector2(420f, 120f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f));

            GameObject hudBar = CreatePanel(canvas.transform, "BottomBar", new Color(0.08f, 0.09f, 0.09f, 0.96f), new Vector2(1080f, 200f));
            SetRect(hudBar.GetComponent<RectTransform>(), new Vector2(0f, 0f), new Vector2(1f, 0.11f), new Vector2(0.5f, 0f), Vector2.zero, Vector2.zero);

            Button mapButton = CreateButton(hudBar.transform, "MapButton", "Map", new Color(0.23f, 0.43f, 0.21f, 1f), new Vector2(-220f, 24f), new Vector2(160f, 58f));
            Button statusButton = CreateButton(hudBar.transform, "StatusButton", "Status", new Color(0.34f, 0.26f, 0.17f, 1f), new Vector2(0f, 24f), new Vector2(160f, 58f));
            Button phoneButton = CreateButton(hudBar.transform, "PhoneButton", "Phone", new Color(0.34f, 0.26f, 0.17f, 1f), new Vector2(220f, 24f), new Vector2(160f, 58f));

            GameObject statusPanel = CreatePanel(canvas.transform, "StatusPanel", new Color(0.12f, 0.14f, 0.16f, 0.98f), new Vector2(320f, 300f));
            SetRect(statusPanel.GetComponent<RectTransform>(), new Vector2(0.75f, 0.34f), new Vector2(0.98f, 0.70f), new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            statusPanel.SetActive(false);
            CreateText(statusPanel.transform, "StatusTitle", "Status", 30, TextAnchor.UpperCenter, Color.white, new Vector2(0f, -20f), new Vector2(280f, 40f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f));
            CreateText(statusPanel.transform, "StatusBody", "Uang: Rp 125.000\nEnergi: 80/100\nLapar: 60/100\nStamina: 70/100", 22, TextAnchor.UpperLeft, new Color(0.95f, 0.92f, 0.86f), new Vector2(24f, -82f), new Vector2(230f, 160f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f));
            Button closeStatus = CreateButton(statusPanel.transform, "CloseButton", "Close", new Color(0.43f, 0.24f, 0.18f, 1f), new Vector2(0f, -260f), new Vector2(140f, 48f));

            GameObject phonePanel = CreatePanel(canvas.transform, "PhonePanel", new Color(0.12f, 0.14f, 0.16f, 0.98f), new Vector2(320f, 300f));
            SetRect(phonePanel.GetComponent<RectTransform>(), new Vector2(0.75f, 0.34f), new Vector2(0.98f, 0.70f), new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            phonePanel.SetActive(false);
            CreateText(phonePanel.transform, "PhoneTitle", "RantauPhone", 30, TextAnchor.UpperCenter, Color.white, new Vector2(0f, -20f), new Vector2(280f, 40f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f));
            CreateText(phonePanel.transform, "PhoneBody", "Placeholder menu HP.\nNanti dipakai untuk pesan, map, dan menu lain.", 22, TextAnchor.UpperLeft, new Color(0.95f, 0.92f, 0.86f), new Vector2(24f, -82f), new Vector2(230f, 160f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f));
            Button closePhone = CreateButton(phonePanel.transform, "CloseButton", "Close", new Color(0.43f, 0.24f, 0.18f, 1f), new Vector2(0f, -260f), new Vector2(140f, 48f));

            mapButton.onClick.AddListener(() => ShowOnly(mapPanel, mapPanel, statusPanel, phonePanel));
            statusButton.onClick.AddListener(() => ShowOnly(statusPanel, mapPanel, statusPanel, phonePanel));
            phoneButton.onClick.AddListener(() => ShowOnly(phonePanel, mapPanel, statusPanel, phonePanel));
            closeStatus.onClick.AddListener(() => ShowOnly(mapPanel, mapPanel, statusPanel, phonePanel));
            closePhone.onClick.AddListener(() => ShowOnly(mapPanel, mapPanel, statusPanel, phonePanel));

            Button terminal = CreateButton(mapPanel.transform, "TerminalButton", "Terminal", new Color(0.33f, 0.49f, 0.69f, 1f), new Vector2(120f, 370f), new Vector2(180f, 62f));
            Button jalanRaya = CreateButton(mapPanel.transform, "JalanRayaButton", "Jalan Raya", new Color(0.63f, 0.42f, 0.22f, 1f), new Vector2(300f, 220f), new Vector2(180f, 62f));
            Button gangKos = CreateButton(mapPanel.transform, "GangKosButton", "Gang Kos", new Color(0.78f, 0.37f, 0.22f, 1f), new Vector2(470f, 300f), new Vector2(180f, 62f));
            Button warung = CreateButton(mapPanel.transform, "WarungButton", "Warung", new Color(0.34f, 0.58f, 0.28f, 1f), new Vector2(250f, 100f), new Vector2(180f, 62f));
            Button areaKerja = CreateButton(mapPanel.transform, "AreaKerjaAwalButton", "Area Kerja Awal", new Color(0.45f, 0.28f, 0.63f, 1f), new Vector2(520f, 120f), new Vector2(180f, 62f));
            Button kosPemain = CreateButton(mapPanel.transform, "KosPemainButton", "Kos Pemain", new Color(0.56f, 0.38f, 0.20f, 1f), new Vector2(550f, 370f), new Vector2(180f, 62f));

            terminal.onClick.AddListener(() =>
            {
                selectedArea.text = "Terminal";
                selectedDesc.text = "Area awal pemain turun dari bus. Tempat pertama untuk memulai perjalanan di kota.";
            });
            jalanRaya.onClick.AddListener(() =>
            {
                selectedArea.text = "Jalan Raya";
                selectedDesc.text = "Jalur utama menuju area lain. Cocok untuk transisi antar lokasi.";
            });
            gangKos.onClick.AddListener(() =>
            {
                selectedArea.text = "Gang Kos";
                selectedDesc.text = "Gang sempit menuju area kos pemain dan lingkungan sekitar.";
            });
            warung.onClick.AddListener(() =>
            {
                selectedArea.text = "Warung";
                selectedDesc.text = "Tempat beli makan, minum, dan kebutuhan sederhana untuk anak rantau.";
            });
            areaKerja.onClick.AddListener(() =>
            {
                selectedArea.text = "Area Kerja Awal";
                selectedDesc.text = "Area pekerjaan sederhana untuk mulai menghasilkan uang di awal game.";
            });
            kosPemain.onClick.AddListener(() =>
            {
                selectedArea.text = "Kos Pemain";
                selectedDesc.text = "Tempat tinggal utama pemain. Nanti bisa dikembangkan seiring progres.";
            });
        }

        private static System.Collections.IEnumerator LoadTargetAfterLoadingScreen(Slider slider)
        {
            yield return null;

            if (MinimumVisibleSeconds > 0f)
            {
                yield return new WaitForSecondsRealtime(MinimumVisibleSeconds);
            }

            string target = SceneTransitionContext.HasRequest ? SceneTransitionContext.TargetSceneName : SceneNames.MainMenu;

            SceneTransitionContext.Clear();

            yield return SceneLoader.LoadSceneAsync(
                target,
                progress => slider.value = progress,
                holdAtMaxProgress: true);
        }

        private static void EnsureBuiltinAssets()
        {
            if (builtinFont == null)
            {
                builtinFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            }

            if (solidSprite == null)
            {
                Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                texture.SetPixel(0, 0, Color.white);
                texture.Apply();
                solidSprite = Sprite.Create(
                    texture,
                    new Rect(0f, 0f, 1f, 1f),
                    new Vector2(0.5f, 0.5f),
                    1f);
            }
        }

        private static GameObject CreateCanvas(string name)
        {
            GameObject canvasObject = new GameObject(name);
            canvasObject.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObject.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1080f, 1920f);
            canvasObject.GetComponent<CanvasScaler>().matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            return canvasObject;
        }

        private static GameObject CreateFullscreenPanel(Transform parent, string name, Color color)
        {
            GameObject panel = CreatePanel(parent, name, color, new Vector2(100f, 100f));
            SetRect(panel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            return panel;
        }

        private static GameObject CreatePanel(Transform parent, string name, Color color, Vector2 sizeDelta)
        {
            GameObject panel = new GameObject(name);
            panel.transform.SetParent(parent, false);
            Image image = panel.AddComponent<Image>();
            image.sprite = solidSprite;
            image.type = Image.Type.Simple;
            image.color = color;
            SetRect(panel.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), sizeDelta, Vector2.zero);
            return panel;
        }

        private static Text CreateText(Transform parent, string name, string value, int fontSize, TextAnchor alignment, Color color, Vector2 anchoredPosition, Vector2 sizeDelta, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot)
        {
            GameObject textObject = new GameObject(name);
            textObject.transform.SetParent(parent, false);
            Text text = textObject.AddComponent<Text>();
            text.font = builtinFont;
            text.text = value;
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.color = color;
            text.raycastTarget = false;
            SetRect(textObject.GetComponent<RectTransform>(), anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            return text;
        }

        private static Button CreateButton(Transform parent, string name, string label, Color color, Vector2 anchoredPosition, Vector2 sizeDelta)
        {
            GameObject buttonObject = new GameObject(name);
            buttonObject.transform.SetParent(parent, false);
            Image image = buttonObject.AddComponent<Image>();
            image.sprite = solidSprite;
            image.type = Image.Type.Simple;
            image.color = color;
            Button button = buttonObject.AddComponent<Button>();
            button.targetGraphic = image;

            CreateText(buttonObject.transform, "Label", label, 24, TextAnchor.MiddleCenter, Color.white, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f));
            SetRect(buttonObject.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), sizeDelta, anchoredPosition);
            return button;
        }

        private static Slider CreateSlider(Transform parent, string name)
        {
            GameObject sliderObject = new GameObject(name);
            sliderObject.transform.SetParent(parent, false);

            Image background = sliderObject.AddComponent<Image>();
            background.sprite = solidSprite;
            background.type = Image.Type.Simple;
            background.color = new Color(0.22f, 0.18f, 0.12f, 1f);

            Slider slider = sliderObject.AddComponent<Slider>();
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = 0f;

            GameObject fillArea = new GameObject("Fill Area");
            fillArea.transform.SetParent(sliderObject.transform, false);
            RectTransform fillAreaRect = fillArea.AddComponent<RectTransform>();
            fillAreaRect.anchorMin = new Vector2(0f, 0f);
            fillAreaRect.anchorMax = new Vector2(1f, 1f);
            fillAreaRect.offsetMin = new Vector2(8f, 8f);
            fillAreaRect.offsetMax = new Vector2(-8f, -8f);

            GameObject fill = new GameObject("Fill");
            fill.transform.SetParent(fillArea.transform, false);
            Image fillImage = fill.AddComponent<Image>();
            fillImage.sprite = solidSprite;
            fillImage.type = Image.Type.Simple;
            fillImage.color = new Color(0.89f, 0.70f, 0.20f, 1f);
            RectTransform fillRect = fill.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;
            slider.fillRect = fillRect;

            GameObject handleArea = new GameObject("Handle Slide Area");
            handleArea.transform.SetParent(sliderObject.transform, false);
            RectTransform handleAreaRect = handleArea.AddComponent<RectTransform>();
            handleAreaRect.anchorMin = Vector2.zero;
            handleAreaRect.anchorMax = Vector2.one;
            handleAreaRect.offsetMin = Vector2.zero;
            handleAreaRect.offsetMax = Vector2.zero;

            GameObject handle = new GameObject("Handle");
            handle.transform.SetParent(handleArea.transform, false);
            Image handleImage = handle.AddComponent<Image>();
            handleImage.sprite = solidSprite;
            handleImage.color = Color.white;
            RectTransform handleRect = handle.GetComponent<RectTransform>();
            handleRect.sizeDelta = new Vector2(24f, 24f);
            slider.handleRect = handleRect;

            slider.direction = Slider.Direction.LeftToRight;
            slider.targetGraphic = handleImage;

            SetRect(sliderObject.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(320f, 28f), Vector2.zero);
            return slider;
        }

        private static void SetRect(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition)
        {
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            rect.sizeDelta = sizeDelta;
            rect.anchoredPosition = anchoredPosition;
        }

        private static void ShowOnly(GameObject activePanel, params GameObject[] panels)
        {
            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i] != null)
                {
                    panels[i].SetActive(panels[i] == activePanel);
                }
            }
        }
    }
}

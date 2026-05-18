#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using AnakRantau.Core;
using AnakRantau.Game;
using AnakRantau.SceneManagement;
using AnakRantau.UI;

namespace AnakRantau.EditorTools
{
    /// <summary>
    /// Generates the Level 1 placeholder scenes and UI so the project has visible content quickly.
    /// Run from the Unity menu: Anak Rantau > Level 1 > Generate Placeholder Scenes
    /// </summary>
    public static class Level1SceneSetupTool
    {
        private const string BaseFolder = "Assets/_AnakRantau";
        private const string ScenesFolder = BaseFolder + "/Scenes";
        private const string BootstrapScenePath = ScenesFolder + "/Bootstrap.unity";
        private const string LoadingScenePath = ScenesFolder + "/LoadingScreen.unity";
        private const string MainMenuScenePath = ScenesFolder + "/MainMenu.unity";
        private const string GameScenePath = ScenesFolder + "/Game_Level1_Room.unity";
        private const string LoadingBackgroundPath = "Assets/_AnakRantau/Art/UI/LoadingScreen/Background.png";

        private static readonly Color WarmBackground = new Color(0.94f, 0.90f, 0.83f);
        private static readonly Color DarkPanel = new Color(0.14f, 0.16f, 0.14f, 0.96f);
        private static readonly Color OlivePanel = new Color(0.22f, 0.24f, 0.16f, 0.96f);
        private static readonly Color GoldAccent = new Color(0.93f, 0.73f, 0.24f, 1f);
        private static readonly Color ButtonGreen = new Color(0.23f, 0.43f, 0.21f, 1f);
        private static readonly Color ButtonBrown = new Color(0.34f, 0.26f, 0.17f, 1f);
        private static Font builtinFont;
        private static Sprite solidSprite;

        [MenuItem("Anak Rantau/Level 1/Generate Placeholder Scenes")]
        public static void GeneratePlaceholderScenes()
        {
            EnsureFolderExists(BaseFolder);
            EnsureFolderExists(ScenesFolder);
            EnsureBuiltinAssets();
            EnsureLoadingBackgroundImportedAsSprite();

            CreateBootstrapScene();
            CreateLoadingScene();
            CreateMainMenuScene();
            CreateGameScene();
            ConfigureBuildSettings();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog(
                "Anak Rantau",
                "Level 1 placeholder scenes have been generated.",
                "OK");
        }

        private static void CreateBootstrapScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            GameObject bootstrapper = new GameObject("Bootstrapper");
            bootstrapper.AddComponent<Bootstrapper>();

            SaveScene(scene, BootstrapScenePath);
        }

        private static void CreateLoadingScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            CreateEventSystem();
            GameObject canvas = CreateCanvas("Canvas");
            CreateFullscreenBackground(canvas.transform);
            CreateFullscreenPanel(canvas.transform, "Overlay", new Color(0.03f, 0.04f, 0.03f, 0.50f));

            GameObject card = CreatePanel(canvas.transform, "LoadingCard", new Color(0.14f, 0.16f, 0.14f, 0.82f));
            ConfigureRect(card.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(820f, 520f), Vector2.zero);

            CreateText(card.transform, "Title", "ANAK RANTAU", 58, TextAnchor.UpperCenter, Color.white, new Vector2(0f, -8f), new Vector2(720f, 90f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f));
            CreateText(card.transform, "Subtitle", "ExplosionCrash Gameplay", 22, TextAnchor.UpperCenter, new Color(0.85f, 0.82f, 0.74f), new Vector2(0f, -92f), new Vector2(600f, 36f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f));

            GameObject loadingTextObj = CreateText(card.transform, "LoadingText", "Memuat...", 30, TextAnchor.MiddleCenter, Color.white, new Vector2(0f, -176f), new Vector2(700f, 72f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f)).gameObject;
            GameObject progressLabelObj = CreateText(card.transform, "ProgressLabel", "0%", 24, TextAnchor.UpperCenter, new Color(0.93f, 0.73f, 0.24f, 1f), new Vector2(0f, -224f), new Vector2(180f, 34f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f)).gameObject;
            CreateText(card.transform, "TipText", "Menyiapkan perjalanan anak rantau...", 20, TextAnchor.UpperCenter, new Color(0.85f, 0.82f, 0.74f), new Vector2(0f, -278f), new Vector2(700f, 60f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));

            Slider sliderObj = CreateSlider(card.transform, "ProgressBar");
            ConfigureRect(sliderObj.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(680f, 34f), new Vector2(0f, -336f));

            GameObject controllerObj = new GameObject("LoadingScreenController");
            var controller = controllerObj.AddComponent<LoadingScreenController>();
            SetSerializedReference(controller, "loadingText", loadingTextObj.GetComponent<Text>());
            SetSerializedReference(controller, "progressBar", sliderObj);
            SetSerializedReference(controller, "progressLabel", progressLabelObj.GetComponent<Text>());

            SaveScene(scene, LoadingScenePath);
        }

        private static void CreateMainMenuScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            CreateEventSystem();
            GameObject canvas = CreateCanvas("Canvas");
            CreateFullscreenPanel(canvas.transform, "Background", WarmBackground);

            GameObject leftPanel = CreatePanel(canvas.transform, "BrandPanel", new Color(0.96f, 0.93f, 0.86f, 0.92f));
            ConfigureRect(leftPanel.GetComponent<RectTransform>(), new Vector2(0f, 0f), new Vector2(0.43f, 1f), new Vector2(0f, 0.5f), new Vector2(0f, 0f), new Vector2(0f, 0f));

            CreateText(leftPanel.transform, "GameTitle", "ANAK RANTAU", 60, TextAnchor.UpperLeft, new Color(0.22f, 0.15f, 0.08f), new Vector2(48f, -70f), new Vector2(520f, 120f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f));
            CreateText(leftPanel.transform, "Subtitle", "ExplosionCrash Gameplay", 24, TextAnchor.UpperLeft, new Color(0.28f, 0.20f, 0.12f), new Vector2(52f, -150f), new Vector2(480f, 60f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f));
            CreateText(leftPanel.transform, "Description", "Sebagai seorang anak rantau, kamu harus bertahan, bekerja keras, berkembang, dan membangun hidup yang lebih mapan.", 24, TextAnchor.UpperLeft, new Color(0.24f, 0.20f, 0.16f), new Vector2(52f, -240f), new Vector2(440f, 220f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f));

            GameObject buttonPanel = CreatePanel(canvas.transform, "MenuPanel", new Color(0.10f, 0.11f, 0.10f, 0.92f));
            ConfigureRect(buttonPanel.GetComponent<RectTransform>(), new Vector2(0.50f, 0.18f), new Vector2(0.90f, 0.88f), new Vector2(0.5f, 0.5f), new Vector2(0f, 0f), Vector2.zero);

            CreateText(buttonPanel.transform, "MenuHeader", "Menu Utama", 34, TextAnchor.UpperCenter, Color.white, new Vector2(0f, -30f), new Vector2(420f, 60f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f));

            Button startButton = CreateButton(buttonPanel.transform, "StartGameButton", "Start Game", ButtonGreen, new Vector2(0f, -110f), new Vector2(320f, 74f));
            Button continueButton = CreateButton(buttonPanel.transform, "ContinueButton", "Continue", ButtonBrown, new Vector2(0f, -200f), new Vector2(320f, 74f));
            Button settingsButton = CreateButton(buttonPanel.transform, "SettingsButton", "Settings", ButtonBrown, new Vector2(0f, -290f), new Vector2(320f, 74f));
            Button exitButton = CreateButton(buttonPanel.transform, "ExitButton", "Exit", new Color(0.42f, 0.18f, 0.16f, 1f), new Vector2(0f, -380f), new Vector2(320f, 74f));

            continueButton.interactable = false;

            GameObject settingsPanel = CreatePanel(canvas.transform, "SettingsPanel", OlivePanel);
            ConfigureRect(settingsPanel.GetComponent<RectTransform>(), new Vector2(0.55f, 0.5f), new Vector2(0.88f, 0.72f), new Vector2(0.5f, 0.5f), new Vector2(0f, 0f), Vector2.zero);
            settingsPanel.SetActive(false);
            CreateText(settingsPanel.transform, "SettingsTitle", "Settings", 34, TextAnchor.UpperCenter, Color.white, new Vector2(0f, -22f), new Vector2(320f, 50f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f));
            CreateText(settingsPanel.transform, "SettingsBody", "Placeholder settings panel.\nNanti kita isi audio, kontrol, dan kualitas.", 22, TextAnchor.MiddleCenter, new Color(0.95f, 0.92f, 0.85f), new Vector2(0f, -98f), new Vector2(320f, 130f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
            Button closeSettingsButton = CreateButton(settingsPanel.transform, "CloseButton", "Close", new Color(0.47f, 0.26f, 0.16f, 1f), new Vector2(0f, -220f), new Vector2(180f, 54f));

            GameObject controllerObj = new GameObject("MainMenuController");
            var controller = controllerObj.AddComponent<MainMenuController>();
            SetSerializedReference(controller, "startButton", startButton);
            SetSerializedReference(controller, "continueButton", continueButton);
            SetSerializedReference(controller, "settingsButton", settingsButton);
            SetSerializedReference(controller, "exitButton", exitButton);
            SetSerializedReference(controller, "closeSettingsButton", closeSettingsButton);
            SetSerializedReference(controller, "settingsPanel", settingsPanel);

            SaveScene(scene, MainMenuScenePath);
        }

        private static void CreateGameScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            CreateEventSystem();
            GameObject canvas = CreateCanvas("Canvas");
            CreateFullscreenPanel(canvas.transform, "Background", new Color(0.08f, 0.10f, 0.11f, 1f));

            GameObject root = new GameObject("GameRoot");
            var gameController = root.AddComponent<GameLevel1RoomController>();

            Text roomLabel = CreateText(canvas.transform, "RoomLabel", "Kamar Kost Level 1", 32, TextAnchor.UpperLeft, Color.white, new Vector2(28f, -24f), new Vector2(420f, 54f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f));
            SetSerializedReference(gameController, "roomLabel", roomLabel);

            GameObject mapPanel = CreatePanel(canvas.transform, "MapPanel", new Color(0.95f, 0.92f, 0.85f, 0.98f));
            ConfigureRect(mapPanel.GetComponent<RectTransform>(), new Vector2(0f, 0.14f), new Vector2(0.72f, 0.90f), new Vector2(0f, 0.5f), Vector2.zero, Vector2.zero);

            Text mapTitle = CreateText(mapPanel.transform, "MapTitle", "Peta Level 1", 34, TextAnchor.UpperLeft, new Color(0.25f, 0.18f, 0.10f), new Vector2(24f, -18f), new Vector2(300f, 50f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f));
            Text selectedAreaText = CreateText(mapPanel.transform, "SelectedArea", "Terminal", 28, TextAnchor.UpperLeft, new Color(0.18f, 0.14f, 0.10f), new Vector2(24f, -76f), new Vector2(300f, 40f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f));
            Text selectedAreaDescription = CreateText(mapPanel.transform, "SelectedAreaDescription", "Area awal pemain turun dari bus. Tempat pertama untuk memulai perjalanan di kota.", 20, TextAnchor.UpperLeft, new Color(0.23f, 0.18f, 0.14f), new Vector2(24f, -124f), new Vector2(420f, 120f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f));

            GameObject mapBody = CreatePanel(mapPanel.transform, "MapBody", new Color(0.87f, 0.83f, 0.75f, 1f));
            ConfigureRect(mapBody.GetComponent<RectTransform>(), new Vector2(0.02f, 0.02f), new Vector2(0.74f, 0.62f), new Vector2(0f, 0f), Vector2.zero, Vector2.zero);

            Button terminalButton = CreateMapButton(mapBody.transform, "TerminalButton", "Terminal", new Vector2(120f, 370f), new Color(0.33f, 0.49f, 0.69f, 1f));
            Button jalanRayaButton = CreateMapButton(mapBody.transform, "JalanRayaButton", "Jalan Raya", new Vector2(300f, 220f), new Color(0.63f, 0.42f, 0.22f, 1f));
            Button gangKosButton = CreateMapButton(mapBody.transform, "GangKosButton", "Gang Kos", new Vector2(470f, 300f), new Color(0.78f, 0.37f, 0.22f, 1f));
            Button warungButton = CreateMapButton(mapBody.transform, "WarungButton", "Warung", new Vector2(250f, 100f), new Color(0.34f, 0.58f, 0.28f, 1f));
            Button areaKerjaAwalButton = CreateMapButton(mapBody.transform, "AreaKerjaAwalButton", "Area Kerja Awal", new Vector2(520f, 120f), new Color(0.45f, 0.28f, 0.63f, 1f));
            Button kosPemainButton = CreateMapButton(mapBody.transform, "KosPemainButton", "Kos Pemain", new Vector2(550f, 370f), new Color(0.56f, 0.38f, 0.20f, 1f));

            GameObject mapControllerObj = new GameObject("Level1MapController");
            var mapController = mapControllerObj.AddComponent<Level1MapController>();
            SetSerializedReference(mapController, "mapTitleText", mapTitle);
            SetSerializedReference(mapController, "selectedAreaText", selectedAreaText);
            SetSerializedReference(mapController, "selectedAreaDescriptionText", selectedAreaDescription);
            SetSerializedReference(mapController, "terminalButton", terminalButton);
            SetSerializedReference(mapController, "jalanRayaButton", jalanRayaButton);
            SetSerializedReference(mapController, "gangKosButton", gangKosButton);
            SetSerializedReference(mapController, "warungButton", warungButton);
            SetSerializedReference(mapController, "areaKerjaAwalButton", areaKerjaAwalButton);
            SetSerializedReference(mapController, "kosPemainButton", kosPemainButton);

            GameObject mapHudRoot = CreatePanel(canvas.transform, "HudRoot", new Color(0f, 0f, 0f, 0f));
            ConfigureRect(mapHudRoot.GetComponent<RectTransform>(), new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);

            GameObject hudBar = CreatePanel(mapHudRoot.transform, "BottomBar", new Color(0.08f, 0.09f, 0.09f, 0.96f));
            ConfigureRect(hudBar.GetComponent<RectTransform>(), new Vector2(0f, 0f), new Vector2(1f, 0.11f), new Vector2(0.5f, 0f), Vector2.zero, Vector2.zero);

            Button mapButton = CreateButton(hudBar.transform, "MapButton", "Map", ButtonGreen, new Vector2(-220f, 24f), new Vector2(160f, 58f));
            Button statusButton = CreateButton(hudBar.transform, "StatusButton", "Status", ButtonBrown, new Vector2(0f, 24f), new Vector2(160f, 58f));
            Button phoneButton = CreateButton(hudBar.transform, "PhoneButton", "Phone", ButtonBrown, new Vector2(220f, 24f), new Vector2(160f, 58f));

            GameObject statusPanel = CreatePanel(mapHudRoot.transform, "StatusPanel", new Color(0.12f, 0.14f, 0.16f, 0.98f));
            ConfigureRect(statusPanel.GetComponent<RectTransform>(), new Vector2(0.75f, 0.34f), new Vector2(0.98f, 0.70f), new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            statusPanel.SetActive(false);
            CreateText(statusPanel.transform, "StatusTitle", "Status", 30, TextAnchor.UpperCenter, Color.white, new Vector2(0f, -20f), new Vector2(280f, 40f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f));
            CreateText(statusPanel.transform, "StatusBody", "Uang: Rp 125.000\nEnergi: 80/100\nLapar: 60/100\nStamina: 70/100", 22, TextAnchor.UpperLeft, new Color(0.95f, 0.92f, 0.86f), new Vector2(24f, -82f), new Vector2(230f, 160f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f));
            Button closeStatusButton = CreateButton(statusPanel.transform, "CloseButton", "Close", new Color(0.43f, 0.24f, 0.18f, 1f), new Vector2(0f, -260f), new Vector2(140f, 48f));

            GameObject phonePanel = CreatePanel(mapHudRoot.transform, "PhonePanel", new Color(0.12f, 0.14f, 0.16f, 0.98f));
            ConfigureRect(phonePanel.GetComponent<RectTransform>(), new Vector2(0.75f, 0.34f), new Vector2(0.98f, 0.70f), new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            phonePanel.SetActive(false);
            CreateText(phonePanel.transform, "PhoneTitle", "RantauPhone", 30, TextAnchor.UpperCenter, Color.white, new Vector2(0f, -20f), new Vector2(280f, 40f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f));
            CreateText(phonePanel.transform, "PhoneBody", "Placeholder menu HP.\nNanti dipakai untuk pesan, map, dan menu lain.", 22, TextAnchor.UpperLeft, new Color(0.95f, 0.92f, 0.86f), new Vector2(24f, -82f), new Vector2(230f, 160f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f));
            Button closePhoneButton = CreateButton(phonePanel.transform, "CloseButton", "Close", new Color(0.43f, 0.24f, 0.18f, 1f), new Vector2(0f, -260f), new Vector2(140f, 48f));

            GameObject interfaceControllerObj = new GameObject("PlayerInterfaceController");
            var interfaceController = interfaceControllerObj.AddComponent<PlayerInterfaceController>();
            SetSerializedReference(interfaceController, "mapPanel", mapPanel);
            SetSerializedReference(interfaceController, "statusPanel", statusPanel);
            SetSerializedReference(interfaceController, "phonePanel", phonePanel);
            SetSerializedReference(interfaceController, "mapButton", mapButton);
            SetSerializedReference(interfaceController, "statusButton", statusButton);
            SetSerializedReference(interfaceController, "phoneButton", phoneButton);
            SetSerializedReference(interfaceController, "closeStatusButton", closeStatusButton);
            SetSerializedReference(interfaceController, "closePhoneButton", closePhoneButton);

            SaveScene(scene, GameScenePath);
        }

        private static GameObject CreateCanvas(string name)
        {
            EnsureBuiltinAssets();

            GameObject canvasObject = new GameObject(name);
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.pixelPerfect = true;

            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080f, 1920f);
            scaler.matchWidthOrHeight = 0.5f;

            canvasObject.AddComponent<GraphicRaycaster>();

            return canvasObject;
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

        private static void EnsureLoadingBackgroundImportedAsSprite()
        {
            TextureImporter importer = AssetImporter.GetAtPath(LoadingBackgroundPath) as TextureImporter;
            if (importer == null)
            {
                return;
            }

            if (importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.alphaIsTransparency = true;
                importer.mipmapEnabled = false;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.SaveAndReimport();
            }
        }

        private static void CreateEventSystem()
        {
            GameObject eventSystemObject = new GameObject("EventSystem");
            System.Type eventSystemType = FindType("UnityEngine.EventSystems.EventSystem");
            if (eventSystemType != null)
            {
                eventSystemObject.AddComponent(eventSystemType);
            }

            System.Type inputModuleType = FindType("UnityEngine.InputSystem.UI.InputSystemUIInputModule");
            if (inputModuleType != null)
            {
                eventSystemObject.AddComponent(inputModuleType);
            }
        }

        private static GameObject CreateFullscreenPanel(Transform parent, string name, Color color)
        {
            GameObject panel = CreatePanel(parent, name, color);
            ConfigureRect(panel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            return panel;
        }

        private static GameObject CreateFullscreenBackground(Transform parent)
        {
            GameObject panel = new GameObject("Background");
            panel.transform.SetParent(parent, false);

            Image image = panel.AddComponent<Image>();
            Sprite backgroundSprite = AssetDatabase.LoadAssetAtPath<Sprite>(LoadingBackgroundPath);
            if (backgroundSprite != null)
            {
                image.sprite = backgroundSprite;
                image.type = Image.Type.Simple;
                image.preserveAspect = false;
            }
            else
            {
                image.sprite = solidSprite;
                image.type = Image.Type.Simple;
                image.color = new Color(0.10f, 0.11f, 0.10f, 1f);
            }

            ConfigureRect(panel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            return panel;
        }

        private static GameObject CreatePanel(Transform parent, string name, Color color)
        {
            GameObject panel = new GameObject(name);
            panel.transform.SetParent(parent, false);
            var image = panel.AddComponent<Image>();
            image.sprite = solidSprite;
            image.type = Image.Type.Simple;
            image.color = color;
            ConfigureRect(panel.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(100f, 100f), Vector2.zero);
            return panel;
        }

        private static Text CreateText(
            Transform parent,
            string name,
            string value,
            int fontSize,
            TextAnchor alignment,
            Color color,
            Vector2 anchoredPosition,
            Vector2 sizeDelta,
            Vector2 anchorMin,
            Vector2 anchorMax,
            Vector2 pivot)
        {
            GameObject textObject = new GameObject(name);
            textObject.transform.SetParent(parent, false);

            var text = textObject.AddComponent<Text>();
            text.font = builtinFont;
            text.text = value;
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.color = color;
            text.raycastTarget = false;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;

            ConfigureRect(textObject.GetComponent<RectTransform>(), anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            return text;
        }

        private static Button CreateButton(Transform parent, string name, string label, Color color, Vector2 anchoredPosition, Vector2 sizeDelta)
        {
            GameObject buttonObject = new GameObject(name);
            buttonObject.transform.SetParent(parent, false);

            var image = buttonObject.AddComponent<Image>();
            image.sprite = solidSprite;
            image.type = Image.Type.Simple;
            image.color = color;

            var button = buttonObject.AddComponent<Button>();
            button.targetGraphic = image;

            CreateText(buttonObject.transform, "Label", label, 24, TextAnchor.MiddleCenter, Color.white, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f));

            ConfigureRect(buttonObject.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), sizeDelta, anchoredPosition);
            return button;
        }

        private static Slider CreateSlider(Transform parent, string name)
        {
            GameObject sliderObject = new GameObject(name);
            sliderObject.transform.SetParent(parent, false);

            var background = sliderObject.AddComponent<Image>();
            background.sprite = solidSprite;
            background.type = Image.Type.Simple;
            background.color = new Color(0.22f, 0.18f, 0.12f, 1f);

            var slider = sliderObject.AddComponent<Slider>();
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = 0f;

            GameObject fillArea = new GameObject("Fill Area");
            fillArea.transform.SetParent(sliderObject.transform, false);
            var fillAreaRect = fillArea.AddComponent<RectTransform>();
            fillAreaRect.anchorMin = new Vector2(0f, 0f);
            fillAreaRect.anchorMax = new Vector2(1f, 1f);
            fillAreaRect.offsetMin = new Vector2(8f, 8f);
            fillAreaRect.offsetMax = new Vector2(-8f, -8f);

            GameObject fill = new GameObject("Fill");
            fill.transform.SetParent(fillArea.transform, false);
            var fillImage = fill.AddComponent<Image>();
            fillImage.sprite = solidSprite;
            fillImage.type = Image.Type.Simple;
            fillImage.color = new Color(0.89f, 0.70f, 0.20f, 1f);
            var fillRect = fill.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;

            slider.fillRect = fillRect;

            GameObject handleSlideArea = new GameObject("Handle Slide Area");
            handleSlideArea.transform.SetParent(sliderObject.transform, false);
            var handleSlideRect = handleSlideArea.AddComponent<RectTransform>();
            handleSlideRect.anchorMin = Vector2.zero;
            handleSlideRect.anchorMax = Vector2.one;
            handleSlideRect.offsetMin = Vector2.zero;
            handleSlideRect.offsetMax = Vector2.zero;

            GameObject handle = new GameObject("Handle");
            handle.transform.SetParent(handleSlideArea.transform, false);
            var handleImage = handle.AddComponent<Image>();
            handleImage.sprite = solidSprite;
            handleImage.color = Color.white;
            var handleRect = handle.GetComponent<RectTransform>();
            handleRect.sizeDelta = new Vector2(24f, 24f);
            slider.handleRect = handleRect;

            slider.direction = Slider.Direction.LeftToRight;
            slider.transition = Selectable.Transition.ColorTint;
            slider.targetGraphic = handleImage;

            ConfigureRect(sliderObject.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(320f, 28f), Vector2.zero);
            return slider;
        }

        private static Button CreateMapButton(Transform parent, string name, string label, Vector2 position, Color color)
        {
            return CreateButton(parent, name, label, color, position, new Vector2(180f, 62f));
        }

        private static System.Type FindType(string typeName)
        {
            foreach (System.Reflection.Assembly assembly in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                System.Type type = assembly.GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }

        private static void ConfigureRect(
            RectTransform rect,
            Vector2 anchorMin,
            Vector2 anchorMax,
            Vector2 pivot,
            Vector2 sizeDelta,
            Vector2 anchoredPosition)
        {
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            rect.sizeDelta = sizeDelta;
            rect.anchoredPosition = anchoredPosition;
        }

        private static void SetSerializedReference(Object target, string propertyName, Object value)
        {
            var serializedObject = new SerializedObject(target);
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            if (property != null)
            {
                property.objectReferenceValue = value;
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        private static void EnsureFolderExists(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
            {
                return;
            }

            string parent = System.IO.Path.GetDirectoryName(path)?.Replace('\\', '/');
            string folderName = System.IO.Path.GetFileName(path);

            if (!string.IsNullOrWhiteSpace(parent) && !AssetDatabase.IsValidFolder(path))
            {
                EnsureFolderExists(parent);
                AssetDatabase.CreateFolder(parent, folderName);
            }
        }

        private static void ConfigureBuildSettings()
        {
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(BootstrapScenePath, true),
                new EditorBuildSettingsScene(LoadingScenePath, true),
                new EditorBuildSettingsScene(MainMenuScenePath, true),
                new EditorBuildSettingsScene(GameScenePath, true)
            };
        }

        private static void SaveScene(Scene scene, string path)
        {
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene, path);
        }
    }
}
#endif

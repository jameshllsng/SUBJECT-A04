using SubjectA04.HealthSystem;
using SubjectA04.World;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SubjectA04.UI
{
    [RequireComponent(typeof(Health))]
    public class PlayerDeathMenu : MonoBehaviour
    {
        [SerializeField] private PrototypeWorldController worldController;
        [SerializeField] private Behaviour[] disableWhileDead;

        private Health health;
        private GameObject menuRoot;
        private Vector3 respawnPosition;
        private Quaternion respawnRotation;

        private void Awake()
        {
            health = GetComponent<Health>();
            respawnPosition = transform.position;
            respawnRotation = transform.rotation;
            health.Died += HandleDeath;

            BuildMenu();
        }

        private void OnDestroy()
        {
            if (health != null)
            {
                health.Died -= HandleDeath;
            }
        }

        private void HandleDeath()
        {
            foreach (Behaviour behaviour in disableWhileDead)
            {
                if (behaviour != null)
                {
                    behaviour.enabled = false;
                }
            }

            menuRoot.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Respawn()
        {
            UnityEngine.CharacterController controller =
                GetComponent<UnityEngine.CharacterController>();

            if (controller != null)
            {
                controller.enabled = false;
            }

            transform.SetPositionAndRotation(respawnPosition, respawnRotation);

            if (controller != null)
            {
                controller.enabled = true;
            }

            health.ResetHealth();

            FirstPersonController firstPersonController = GetComponent<FirstPersonController>();
            if (firstPersonController != null)
            {
                firstPersonController.ResetControllerState();
            }

            foreach (Behaviour behaviour in disableWhileDead)
            {
                if (behaviour != null)
                {
                    behaviour.enabled = true;
                }
            }

            menuRoot.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void ResetEnemy()
        {
            worldController?.ResetEnemy();
        }

        private void BuildMenu()
        {
            EnsureEventSystem();

            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            GameObject canvasObject = new GameObject(
                "Death Menu Canvas",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);

            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            menuRoot = CreateImage(
                "Death Menu",
                canvasObject.transform,
                new Color(0.02f, 0.025f, 0.03f, 0.96f));

            RectTransform panelRect = menuRoot.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.pivot = new Vector2(0.5f, 0.5f);
            panelRect.sizeDelta = new Vector2(500f, 340f);

            Text title = CreateText("Title", menuRoot.transform, font, 36, FontStyle.Bold);
            RectTransform titleRect = title.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.5f, 1f);
            titleRect.anchorMax = new Vector2(0.5f, 1f);
            titleRect.pivot = new Vector2(0.5f, 1f);
            titleRect.anchoredPosition = new Vector2(0f, -42f);
            titleRect.sizeDelta = new Vector2(420f, 60f);
            title.alignment = TextAnchor.MiddleCenter;
            title.text = "YOU DIED";

            CreateButton(
                "Respawn Button",
                menuRoot.transform,
                font,
                "RESPAWN",
                new Vector2(0f, 25f),
                Respawn);

            CreateButton(
                "Reset Enemy Button",
                menuRoot.transform,
                font,
                "RESET ENEMY",
                new Vector2(0f, -65f),
                ResetEnemy);

            menuRoot.SetActive(false);
        }

        private static void EnsureEventSystem()
        {
            if (FindFirstObjectByType<EventSystem>() != null)
            {
                return;
            }

            new GameObject(
                "EventSystem",
                typeof(EventSystem),
                typeof(StandaloneInputModule));
        }

        private static void CreateButton(
            string objectName,
            Transform parent,
            Font font,
            string label,
            Vector2 position,
            UnityEngine.Events.UnityAction onClick)
        {
            GameObject buttonObject = CreateImage(
                objectName,
                parent,
                new Color(0.18f, 0.2f, 0.22f, 1f));

            RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
            buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
            buttonRect.pivot = new Vector2(0.5f, 0.5f);
            buttonRect.anchoredPosition = position;
            buttonRect.sizeDelta = new Vector2(320f, 64f);

            Button button = buttonObject.AddComponent<Button>();
            button.targetGraphic = buttonObject.GetComponent<Image>();
            button.onClick.AddListener(onClick);

            Text buttonText = CreateText("Label", buttonObject.transform, font, 22, FontStyle.Bold);
            RectTransform textRect = buttonText.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            buttonText.alignment = TextAnchor.MiddleCenter;
            buttonText.text = label;
        }

        private static GameObject CreateImage(string objectName, Transform parent, Color color)
        {
            GameObject imageObject = new GameObject(objectName, typeof(RectTransform), typeof(Image));
            imageObject.transform.SetParent(parent, false);

            Image image = imageObject.GetComponent<Image>();
            image.color = color;

            return imageObject;
        }

        private static Text CreateText(
            string objectName,
            Transform parent,
            Font font,
            int fontSize,
            FontStyle fontStyle)
        {
            GameObject textObject = new GameObject(objectName, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);

            Text text = textObject.GetComponent<Text>();
            text.font = font;
            text.fontSize = fontSize;
            text.fontStyle = fontStyle;
            text.color = Color.white;
            text.raycastTarget = false;

            return text;
        }
    }
}

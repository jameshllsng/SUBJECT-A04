using SubjectA04.HealthSystem;
using UnityEngine;
using UnityEngine.UI;

namespace SubjectA04.UI
{
    public class HealthHUD : MonoBehaviour
    {
        [Header("Health Sources")]
        [SerializeField] private Health playerHealth;

        private Image playerFill;
        private Text playerText;

        private void Awake()
        {
            if (playerHealth == null)
            {
                playerHealth = GetComponent<Health>();
            }

            BuildHUD();
        }

        private void LateUpdate()
        {
            UpdateHealthBar(playerHealth, playerFill, playerText, "PLAYER");
        }

        private void BuildHUD()
        {
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            GameObject canvasObject = new GameObject(
                "Health HUD",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler));
            canvasObject.transform.SetParent(transform, false);

            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 10;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            CreateHealthBar(
                canvasObject.transform,
                "Player Health",
                new Vector2(0f, 0f),
                new Vector2(0f, 0f),
                new Vector2(0f, 0f),
                new Vector2(28f, 28f),
                new Vector2(340f, 62f),
                new Color(0.18f, 0.72f, 0.35f, 1f),
                font,
                out _,
                out playerFill,
                out playerText);
        }

        private static void CreateHealthBar(
            Transform parent,
            string objectName,
            Vector2 anchorMin,
            Vector2 anchorMax,
            Vector2 pivot,
            Vector2 anchoredPosition,
            Vector2 size,
            Color fillColor,
            Font font,
            out GameObject root,
            out Image fill,
            out Text label)
        {
            root = CreateImageObject(
                objectName,
                parent,
                new Color(0.035f, 0.04f, 0.045f, 0.92f));

            RectTransform rootRect = root.GetComponent<RectTransform>();
            rootRect.anchorMin = anchorMin;
            rootRect.anchorMax = anchorMax;
            rootRect.pivot = pivot;
            rootRect.anchoredPosition = anchoredPosition;
            rootRect.sizeDelta = size;

            GameObject labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(root.transform, false);

            RectTransform labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0f, 1f);
            labelRect.anchorMax = new Vector2(1f, 1f);
            labelRect.pivot = new Vector2(0.5f, 1f);
            labelRect.anchoredPosition = new Vector2(0f, -7f);
            labelRect.sizeDelta = new Vector2(-20f, 22f);

            label = labelObject.GetComponent<Text>();
            label.font = font;
            label.fontSize = 16;
            label.fontStyle = FontStyle.Bold;
            label.alignment = TextAnchor.MiddleLeft;
            label.color = Color.white;
            label.raycastTarget = false;

            GameObject barBackground = CreateImageObject(
                "Bar Background",
                root.transform,
                new Color(0.11f, 0.12f, 0.13f, 1f));

            RectTransform backgroundRect = barBackground.GetComponent<RectTransform>();
            backgroundRect.anchorMin = new Vector2(0f, 0f);
            backgroundRect.anchorMax = new Vector2(1f, 0f);
            backgroundRect.pivot = new Vector2(0.5f, 0f);
            backgroundRect.anchoredPosition = new Vector2(0f, 8f);
            backgroundRect.sizeDelta = new Vector2(-20f, 20f);

            GameObject fillObject = CreateImageObject("Fill", barBackground.transform, fillColor);
            RectTransform fillRect = fillObject.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.offsetMin = new Vector2(2f, 2f);
            fillRect.offsetMax = new Vector2(-2f, -2f);

            fill = fillObject.GetComponent<Image>();
            fill.type = Image.Type.Filled;
            fill.fillMethod = Image.FillMethod.Horizontal;
            fill.fillOrigin = (int)Image.OriginHorizontal.Left;
            fill.fillAmount = 1f;
            fill.raycastTarget = false;
        }

        private static GameObject CreateImageObject(string objectName, Transform parent, Color color)
        {
            GameObject imageObject = new GameObject(objectName, typeof(RectTransform), typeof(Image));
            imageObject.transform.SetParent(parent, false);

            Image image = imageObject.GetComponent<Image>();
            image.color = color;
            image.raycastTarget = false;

            return imageObject;
        }

        private static void UpdateHealthBar(Health health, Image fill, Text label, string displayName)
        {
            if (health == null)
            {
                return;
            }

            float normalizedHealth = health.MaxHealth > 0f
                ? health.CurrentHealth / health.MaxHealth
                : 0f;

            fill.fillAmount = Mathf.Clamp01(normalizedHealth);
            label.text = $"{displayName}  {Mathf.CeilToInt(health.CurrentHealth)} / {Mathf.CeilToInt(health.MaxHealth)}";
        }
    }
}

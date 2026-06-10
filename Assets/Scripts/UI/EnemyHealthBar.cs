using SubjectA04.HealthSystem;
using UnityEngine;
using UnityEngine.UI;

namespace SubjectA04.UI
{
    [RequireComponent(typeof(Health))]
    public class EnemyHealthBar : MonoBehaviour
    {
        [Header("Temporary Enemy Indicator")]
        [SerializeField] private Vector3 worldOffset = new Vector3(0f, 1.2f, 0f);
        [SerializeField, Min(0.001f)] private float worldScale = 0.005f;

        private Health health;
        private Camera playerCamera;
        private RectTransform canvasTransform;
        private Image fill;
        private Text valueText;

        private void Awake()
        {
            health = GetComponent<Health>();
            playerCamera = Camera.main;
            BuildWorldSpaceBar();
        }

        private void LateUpdate()
        {
            if (playerCamera == null)
            {
                playerCamera = Camera.main;
            }

            if (playerCamera != null)
            {
                canvasTransform.rotation = playerCamera.transform.rotation;
            }

            float normalizedHealth = health.MaxHealth > 0f
                ? health.CurrentHealth / health.MaxHealth
                : 0f;

            fill.fillAmount = Mathf.Clamp01(normalizedHealth);
            valueText.text = $"{Mathf.CeilToInt(health.CurrentHealth)} / {Mathf.CeilToInt(health.MaxHealth)}";
        }

        private void BuildWorldSpaceBar()
        {
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            GameObject canvasObject = new GameObject(
                "Temporary Enemy Health Bar",
                typeof(RectTransform),
                typeof(Canvas));
            canvasObject.transform.SetParent(transform, false);

            canvasTransform = canvasObject.GetComponent<RectTransform>();
            canvasTransform.localPosition = worldOffset;
            canvasTransform.localScale = Vector3.one * worldScale;
            canvasTransform.sizeDelta = new Vector2(220f, 38f);

            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = playerCamera;
            canvas.sortingOrder = 20;

            GameObject backgroundObject = CreateImage(
                "Background",
                canvasObject.transform,
                new Color(0.03f, 0.035f, 0.04f, 0.9f));

            RectTransform backgroundRect = backgroundObject.GetComponent<RectTransform>();
            StretchToParent(backgroundRect, Vector2.zero, Vector2.zero);

            GameObject fillObject = CreateImage(
                "Fill",
                backgroundObject.transform,
                new Color(0.8f, 0.16f, 0.12f, 1f));

            RectTransform fillRect = fillObject.GetComponent<RectTransform>();
            StretchToParent(fillRect, new Vector2(4f, 4f), new Vector2(-4f, -4f));

            fill = fillObject.GetComponent<Image>();
            fill.type = Image.Type.Filled;
            fill.fillMethod = Image.FillMethod.Horizontal;
            fill.fillOrigin = (int)Image.OriginHorizontal.Left;
            fill.raycastTarget = false;

            GameObject textObject = new GameObject("Value", typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(canvasObject.transform, false);

            RectTransform textRect = textObject.GetComponent<RectTransform>();
            StretchToParent(textRect, Vector2.zero, Vector2.zero);

            valueText = textObject.GetComponent<Text>();
            valueText.font = font;
            valueText.fontSize = 18;
            valueText.fontStyle = FontStyle.Bold;
            valueText.alignment = TextAnchor.MiddleCenter;
            valueText.color = Color.white;
            valueText.raycastTarget = false;
        }

        private static GameObject CreateImage(string objectName, Transform parent, Color color)
        {
            GameObject imageObject = new GameObject(objectName, typeof(RectTransform), typeof(Image));
            imageObject.transform.SetParent(parent, false);

            Image image = imageObject.GetComponent<Image>();
            image.color = color;
            image.raycastTarget = false;

            return imageObject;
        }

        private static void StretchToParent(
            RectTransform rectTransform,
            Vector2 offsetMin,
            Vector2 offsetMax)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = offsetMin;
            rectTransform.offsetMax = offsetMax;
        }

        private void OnValidate()
        {
            worldScale = Mathf.Max(0.001f, worldScale);
        }
    }
}

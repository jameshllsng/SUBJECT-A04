using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SubjectA04.UI
{
    public class HUDNotification : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float defaultDuration = 2f;

        private Text messageText;
        private Coroutine hideRoutine;

        private void Awake()
        {
            BuildUI();
        }

        public void Show(string message)
        {
            Show(message, defaultDuration);
        }

        public void Show(string message, float duration)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            messageText.text = message;
            messageText.gameObject.SetActive(true);

            if (hideRoutine != null)
            {
                StopCoroutine(hideRoutine);
            }

            hideRoutine = StartCoroutine(HideAfterDelay(Mathf.Max(0.1f, duration)));
        }

        private IEnumerator HideAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            messageText.gameObject.SetActive(false);
            hideRoutine = null;
        }

        private void BuildUI()
        {
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            GameObject canvasObject = new GameObject(
                "HUD Notifications",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler));
            canvasObject.transform.SetParent(transform, false);

            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 50;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            GameObject textObject = new GameObject(
                "Message",
                typeof(RectTransform),
                typeof(Text),
                typeof(Outline));
            textObject.transform.SetParent(canvasObject.transform, false);

            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.5f, 0.25f);
            textRect.anchorMax = new Vector2(0.5f, 0.25f);
            textRect.pivot = new Vector2(0.5f, 0.5f);
            textRect.sizeDelta = new Vector2(700f, 60f);

            messageText = textObject.GetComponent<Text>();
            messageText.font = font;
            messageText.fontSize = 24;
            messageText.fontStyle = FontStyle.Bold;
            messageText.alignment = TextAnchor.MiddleCenter;
            messageText.color = Color.white;
            messageText.raycastTarget = false;

            Outline outline = textObject.GetComponent<Outline>();
            outline.effectColor = new Color(0f, 0f, 0f, 0.95f);
            outline.effectDistance = new Vector2(2f, -2f);

            textObject.SetActive(false);
        }

        private void OnValidate()
        {
            defaultDuration = Mathf.Max(0.1f, defaultDuration);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace SubjectA04.Interactions
{
    public class PlayerInteractor : MonoBehaviour
    {
        [Header("Interaction")]
        [SerializeField] private Camera playerCamera;
        [SerializeField, Min(0.1f)] private float interactionRange = 3.5f;
        [SerializeField] private KeyCode interactionKey = KeyCode.E;

        private Text promptText;

        private void Awake()
        {
            AssignCameraIfNeeded();
            BuildPrompt();
        }

        private void Update()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            IInteractable interactable = FindInteractable();
            UpdatePrompt(interactable);

            if (interactable != null && Input.GetKeyDown(interactionKey))
            {
                interactable.Interact(gameObject);
            }
#else
            Debug.LogError(
                "PlayerInteractor uses the old Unity Input Manager. Set Active Input Handling to 'Input Manager (Old)' or 'Both'.",
                this);
            enabled = false;
#endif
        }

        private IInteractable FindInteractable()
        {
            if (playerCamera == null)
            {
                return null;
            }

            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            Debug.DrawRay(ray.origin, ray.direction * interactionRange, Color.cyan);

            if (!Physics.Raycast(
                    ray,
                    out RaycastHit hit,
                    interactionRange,
                    Physics.DefaultRaycastLayers,
                    QueryTriggerInteraction.Ignore))
            {
                return null;
            }

            return hit.collider.GetComponentInParent<IInteractable>();
        }

        private void UpdatePrompt(IInteractable interactable)
        {
            bool hasInteraction = interactable != null;
            promptText.gameObject.SetActive(hasInteraction);

            if (hasInteraction)
            {
                promptText.text = $"[{interactionKey}] {interactable.InteractionPrompt}";
            }
        }

        private void BuildPrompt()
        {
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            GameObject canvasObject = new GameObject(
                "Interaction Prompt",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler));
            canvasObject.transform.SetParent(transform, false);

            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 30;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            GameObject textObject = new GameObject(
                "Prompt",
                typeof(RectTransform),
                typeof(Text),
                typeof(Outline));
            textObject.transform.SetParent(canvasObject.transform, false);

            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.5f, 0.5f);
            textRect.anchorMax = new Vector2(0.5f, 0.5f);
            textRect.pivot = new Vector2(0.5f, 0.5f);
            textRect.anchoredPosition = new Vector2(0f, -90f);
            textRect.sizeDelta = new Vector2(520f, 50f);

            promptText = textObject.GetComponent<Text>();
            promptText.font = font;
            promptText.fontSize = 22;
            promptText.fontStyle = FontStyle.Bold;
            promptText.alignment = TextAnchor.MiddleCenter;
            promptText.color = Color.white;
            promptText.raycastTarget = false;

            Outline outline = textObject.GetComponent<Outline>();
            outline.effectColor = new Color(0f, 0f, 0f, 0.9f);
            outline.effectDistance = new Vector2(2f, -2f);

            textObject.SetActive(false);
        }

        private void AssignCameraIfNeeded()
        {
            if (playerCamera != null)
            {
                return;
            }

            playerCamera = GetComponentInChildren<Camera>();
            if (playerCamera == null)
            {
                playerCamera = Camera.main;
            }
        }

        private void OnValidate()
        {
            interactionRange = Mathf.Max(0.1f, interactionRange);
        }
    }
}

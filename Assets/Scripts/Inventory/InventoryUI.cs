using System.Text;
using SubjectA04.EquipmentSystem;
using UnityEngine;
using UnityEngine.UI;

namespace SubjectA04.InventorySystem
{
    [RequireComponent(typeof(PlayerInventory))]
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private KeyCode toggleKey = KeyCode.Tab;

        private PlayerInventory inventory;
        private EquipmentManager equipment;
        private GameObject panel;
        private Text itemListText;

        private void Awake()
        {
            inventory = GetComponent<PlayerInventory>();
            equipment = GetComponent<EquipmentManager>();
            BuildUI();
            inventory.Changed += Refresh;

            if (equipment != null)
            {
                equipment.Changed += Refresh;
            }
        }

        private void OnDestroy()
        {
            if (inventory != null)
            {
                inventory.Changed -= Refresh;
            }

            if (equipment != null)
            {
                equipment.Changed -= Refresh;
            }
        }

        private void OnDisable()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        private void Update()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(toggleKey))
            {
                panel.SetActive(!panel.activeSelf);

                if (panel.activeSelf)
                {
                    Refresh();
                }
            }
#else
            Debug.LogError(
                "InventoryUI uses the old Unity Input Manager. Set Active Input Handling to 'Input Manager (Old)' or 'Both'.",
                this);
            enabled = false;
#endif
        }

        private void BuildUI()
        {
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            GameObject canvasObject = new GameObject(
                "Inventory Canvas",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler));
            canvasObject.transform.SetParent(transform, false);

            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 40;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            panel = CreateImage("Inventory Panel", canvasObject.transform, new Color(0.025f, 0.03f, 0.035f, 0.96f));

            RectTransform panelRect = panel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.pivot = new Vector2(0.5f, 0.5f);
            panelRect.sizeDelta = new Vector2(520f, 420f);

            Text title = CreateText("Title", panel.transform, font, 26, FontStyle.Bold);
            RectTransform titleRect = title.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0f, 1f);
            titleRect.anchorMax = new Vector2(1f, 1f);
            titleRect.pivot = new Vector2(0.5f, 1f);
            titleRect.anchoredPosition = new Vector2(0f, -24f);
            titleRect.sizeDelta = new Vector2(-48f, 42f);
            title.alignment = TextAnchor.MiddleLeft;
            title.text = "INVENTORY";

            itemListText = CreateText("Items", panel.transform, font, 21, FontStyle.Normal);
            RectTransform itemsRect = itemListText.GetComponent<RectTransform>();
            itemsRect.anchorMin = Vector2.zero;
            itemsRect.anchorMax = Vector2.one;
            itemsRect.offsetMin = new Vector2(24f, 24f);
            itemsRect.offsetMax = new Vector2(-24f, -86f);
            itemListText.alignment = TextAnchor.UpperLeft;

            panel.SetActive(false);
        }

        private void Refresh()
        {
            StringBuilder content = new StringBuilder();

            content.AppendLine("EQUIPPED");
            if (equipment != null && equipment.HasArmor)
            {
                content.AppendLine(
                    $"Armor: {equipment.EquippedArmorName} ({equipment.ArmorDamageReduction:P0} protection)");
            }
            else
            {
                content.AppendLine("Armor: None");
            }

            content.AppendLine();
            content.AppendLine("ITEMS");

            foreach (InventoryItemStack item in inventory.Items)
            {
                content.AppendLine($"{item.DisplayName}  x{item.Quantity}");
            }

            itemListText.text = content.Length > 0 ? content.ToString() : "EMPTY";
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

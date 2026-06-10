using SubjectA04.Interactions;
using SubjectA04.InventorySystem;
using SubjectA04.UI;
using UnityEngine;

namespace SubjectA04.Items
{
    [RequireComponent(typeof(BoxCollider))]
    public class HealingPickup : MonoBehaviour, IInteractable
    {
        [SerializeField] private string itemId = "medkit";
        [SerializeField] private string itemName = "Medkit";
        [SerializeField, Min(1)] private int quantity = 1;
        [SerializeField] private string prompt = "PICK UP MEDKIT";

        public string InteractionPrompt => prompt;

        private void Awake()
        {
            CreatePrototypeVisualIfNeeded();
        }

        public bool Interact(GameObject interactor)
        {
            PlayerInventory inventory = interactor.GetComponentInParent<PlayerInventory>();
            if (inventory == null)
            {
                return false;
            }

            inventory.AddItem(itemId, itemName, quantity);

            HUDNotification notification = interactor.GetComponentInParent<HUDNotification>();
            notification?.Show($"{itemName} collected (+{quantity}).");

            Debug.Log($"{interactor.name} collected {name}.", this);
            gameObject.SetActive(false);
            return true;
        }

        private void CreatePrototypeVisualIfNeeded()
        {
            if (GetComponentInChildren<Renderer>() != null)
            {
                return;
            }

            GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Cube);
            visual.name = "Prototype Medkit Visual";
            visual.transform.SetParent(transform, false);
            visual.transform.localScale = new Vector3(0.55f, 0.28f, 0.4f);

            Collider visualCollider = visual.GetComponent<Collider>();
            if (visualCollider != null)
            {
                Destroy(visualCollider);
            }

            Renderer visualRenderer = visual.GetComponent<Renderer>();
            visualRenderer.material.color = new Color(0.22f, 0.42f, 0.24f, 1f);
        }

        private void OnValidate()
        {
            quantity = Mathf.Max(1, quantity);
        }
    }
}

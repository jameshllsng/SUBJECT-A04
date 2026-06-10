using SubjectA04.HealthSystem;
using SubjectA04.InventorySystem;
using SubjectA04.UI;
using UnityEngine;

namespace SubjectA04.Items
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(PlayerInventory))]
    public class HealingConsumable : MonoBehaviour
    {
        [Header("Consumable")]
        [SerializeField] private string itemId = "medkit";
        [SerializeField] private string itemName = "Medkit";
        [SerializeField, Min(0.1f)] private float healAmount = 30f;
        [SerializeField, Min(0)] private int startingUses = 3;
        [SerializeField] private KeyCode useKey = KeyCode.H;

        private Health targetHealth;
        private PlayerInventory inventory;
        private HUDNotification notification;

        public int RemainingUses => inventory != null ? inventory.GetQuantity(itemId) : 0;

        private void Awake()
        {
            targetHealth = GetComponent<Health>();
            inventory = GetComponent<PlayerInventory>();
            notification = GetComponent<HUDNotification>();
            inventory.AddItem(itemId, itemName, startingUses);
        }

        private void Update()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(useKey))
            {
                Use();
            }
#else
            Debug.LogError(
                "HealingConsumable uses the old Unity Input Manager. Set Active Input Handling to 'Input Manager (Old)' or 'Both'.",
                this);
            enabled = false;
#endif
        }

        public bool Use()
        {
            if (inventory.GetQuantity(itemId) <= 0)
            {
                Debug.Log($"{name} has no healing items remaining.", this);
                notification?.Show("No medkits available.");
                return false;
            }

            if (targetHealth.CurrentHealth >= targetHealth.MaxHealth)
            {
                Debug.Log($"{name} is already at full health.", this);
                notification?.Show("Health is already full.");
                return false;
            }

            targetHealth.Heal(healAmount);
            inventory.RemoveItem(itemId, 1);

            Debug.Log($"{name} used a healing item. Remaining uses: {RemainingUses}.", this);
            return true;
        }

        public void AddUses(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            inventory.AddItem(itemId, itemName, amount);
        }

        private void OnValidate()
        {
            healAmount = Mathf.Max(0.1f, healAmount);
            startingUses = Mathf.Max(0, startingUses);
        }
    }
}

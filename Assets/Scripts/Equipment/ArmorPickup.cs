using SubjectA04.Interactions;
using SubjectA04.UI;
using UnityEngine;

namespace SubjectA04.EquipmentSystem
{
    [RequireComponent(typeof(BoxCollider))]
    public class ArmorPickup : MonoBehaviour, IInteractable
    {
        [SerializeField] private string armorName = "Field Vest";
        [SerializeField, Range(0f, 0.9f)] private float damageReduction = 0.25f;
        [SerializeField] private string prompt = "EQUIP FIELD VEST";

        public string InteractionPrompt => prompt;

        private void Awake()
        {
            CreatePrototypeVisualIfNeeded();
        }

        public bool Interact(GameObject interactor)
        {
            EquipmentManager equipment = interactor.GetComponentInParent<EquipmentManager>();
            if (equipment == null)
            {
                return false;
            }

            equipment.EquipArmor(armorName, damageReduction);

            HUDNotification notification = interactor.GetComponentInParent<HUDNotification>();
            notification?.Show($"{armorName} equipped ({damageReduction:P0} protection).");

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
            visual.name = "Prototype Armor Visual";
            visual.transform.SetParent(transform, false);
            visual.transform.localScale = new Vector3(0.65f, 0.75f, 0.22f);

            Collider visualCollider = visual.GetComponent<Collider>();
            if (visualCollider != null)
            {
                Destroy(visualCollider);
            }

            Renderer visualRenderer = visual.GetComponent<Renderer>();
            visualRenderer.material.color = new Color(0.16f, 0.24f, 0.28f, 1f);
        }

        private void OnValidate()
        {
            damageReduction = Mathf.Clamp(damageReduction, 0f, 0.9f);
        }
    }
}

using SubjectA04.Interactions;
using SubjectA04.UI;
using UnityEngine;

namespace SubjectA04.World
{
    public class WorldControlButton : MonoBehaviour, IInteractable
    {
        private enum ControlAction
        {
            ActivateEnemy,
            ResetEnemy,
            ResetGroundItems
        }

        [SerializeField] private PrototypeWorldController worldController;
        [SerializeField] private ControlAction action;
        [SerializeField] private string prompt = "USE CONTROL";

        public string InteractionPrompt => prompt;

        private void Awake()
        {
            CreatePrototypeVisualIfNeeded();
        }

        public bool Interact(GameObject interactor)
        {
            if (worldController == null)
            {
                return false;
            }

            string message;

            switch (action)
            {
                case ControlAction.ActivateEnemy:
                    worldController.ActivateEnemy();
                    message = "Enemy activated.";
                    break;

                case ControlAction.ResetEnemy:
                    worldController.ResetEnemy();
                    message = "Enemy reset.";
                    break;

                case ControlAction.ResetGroundItems:
                    worldController.ResetGroundItems();
                    message = "Ground items reset.";
                    break;

                default:
                    return false;
            }

            HUDNotification notification = interactor.GetComponentInParent<HUDNotification>();
            notification?.Show(message);
            return true;
        }

        private void CreatePrototypeVisualIfNeeded()
        {
            if (GetComponentInChildren<Renderer>() != null)
            {
                return;
            }

            GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Cube);
            visual.name = "Prototype Control Visual";
            visual.transform.SetParent(transform, false);
            visual.transform.localScale = new Vector3(0.5f, 0.25f, 0.5f);

            Collider visualCollider = visual.GetComponent<Collider>();
            if (visualCollider != null)
            {
                Destroy(visualCollider);
            }

            Renderer renderer = visual.GetComponent<Renderer>();
            renderer.material.color = action switch
            {
                ControlAction.ActivateEnemy => new Color(0.75f, 0.2f, 0.12f, 1f),
                ControlAction.ResetEnemy => new Color(0.2f, 0.35f, 0.75f, 1f),
                _ => new Color(0.7f, 0.55f, 0.12f, 1f)
            };
        }
    }
}

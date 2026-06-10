using UnityEngine;

namespace SubjectA04.Interactions
{
    public interface IInteractable
    {
        string InteractionPrompt { get; }

        bool Interact(GameObject interactor);
    }
}

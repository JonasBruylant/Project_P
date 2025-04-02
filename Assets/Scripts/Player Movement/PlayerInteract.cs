using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private LayerMask _interactableMask;
    public float InteractionDistance = 1.5f;

    //private StartDialogue _interactedDialogue;
    //private InteractableSequence _interactableSequence;

    private void Awake()
    {
        _interactableMask = LayerMask.GetMask("Interactable");
    }

    //public void CheckInteractable(InputAction.CallbackContext context)
    //{
    //    if (!context.started) return;
    //    RaycastHit2D rayHit = Physics2D.Raycast(transform.position, _interactionDirection, 3f, _interactableMask);
    //    if (!rayHit)
    //        return;
    //
    //    if (rayHit.distance > InteractionDistance)
    //        return;
    //
    //    var interactables = rayHit.transform.gameObject.GetComponents<IInteractable>();
    //
    //    Debug.Log($"<color=orange> Ray has hit {interactables.Length} interactables");
    //
    //    _interactedDialogue = rayHit.transform.gameObject.GetComponent<StartDialogue>();
    //    _interactableSequence = rayHit.transform.gameObject.GetComponent<InteractableSequence>();
    //    foreach (var interactable in interactables)
    //    {
    //        interactable?.Interact();
    //    }
    //}
    //
    //public void OnProgressDialogue(InputAction.CallbackContext context)
    //{
    //    if (_interactedDialogue == null || !context.started) return;
    //
    //    if (!_interactedDialogue.Progress()) return;
    //
    //    _interactedDialogue = null;
    //}
    //
    //public void OnProgressSequence(InputAction.CallbackContext context)
    //{
    //    if (_interactableSequence == null || !context.started) return;
    //
    //    if (!_interactableSequence.Progress()) return;
    //
    //    _interactableSequence = null;
    //}

}

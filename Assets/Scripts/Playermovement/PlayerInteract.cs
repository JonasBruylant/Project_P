using System.Threading;
using UnityEditor;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private LayerMask _interactableMask;
    public float InteractionDistance = 3f;
    public GameObject Camera;

    private StartDialogue _interactedDialogue;
    private InteractableSequence _interactableSequence;

    DataManager dataManager;
    private void Awake()
    {
        _interactableMask = LayerMask.GetMask("Interactable");
        //_interactableMask = (1<<6);
    }

    private void Start()
    {
        dataManager = DataManager.Instance;

    }

    private void Update()
    {
        if (dataManager.IsPlayerMovementDisabled())
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                OnProgressDialogue();
                OnProgressSequence();
            }
            return;
        }


        if (Input.GetKeyUp(KeyCode.E))
            CheckInteractable();
    }

    public void CheckInteractable()
    {
        Physics.Raycast(Camera.transform.position, Camera.transform.forward, out RaycastHit rayHit, InteractionDistance, _interactableMask);

        if (rayHit.transform == null)
            return;
    
        if (rayHit.distance > InteractionDistance)
            return;
    
        var interactables = rayHit.transform.gameObject.GetComponents<IInteractable>();
    
        Debug.Log($"<color=orange> Ray has hit {interactables.Length} interactables");
    
        _interactedDialogue = rayHit.transform.gameObject.GetComponent<StartDialogue>();
        _interactableSequence = rayHit.transform.gameObject.GetComponent<InteractableSequence>();
        foreach (var interactable in interactables)
        {
            interactable?.Interact();
        }
    }
    
    public void OnProgressDialogue()
    {
        if (_interactedDialogue == null) return;
    
        if (!_interactedDialogue.Progress()) return;
    
        _interactedDialogue = null;
    }
    
    public void OnProgressSequence()
    {
        if (_interactableSequence == null) return;
    
        if (!_interactableSequence.Progress()) return;
    
        _interactableSequence = null;
    }
}

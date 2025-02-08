using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartDialogue : MonoBehaviour, IInteractable
{
    public bool ShouldDisablePlayerMovement = false;
    public TextMeshProUGUI TextComponent;
    public float TextSpeed = 0.2f;
    public string[] DialogueKeys;

    private InputAction _dialogueProgressAction;
    private InputActionMap _playerActionMap;
    private List<string> _dialogueValues = new();

    private int _dialogueIndex = 0;
    //TO DO: Add action map for UI to get the action to progress dialogue states
    private Coroutine _dialogueEnumerator;

    void Start()
    {
        var dmInstance = DialogueManager.Instance;
        for (int i = 0; i < DialogueKeys.Length; i++)
        {
            if (_dialogueValues.Contains(dmInstance.GetDialogueValue(DialogueKeys[i]))) continue;

            _dialogueValues.Add(dmInstance.GetDialogueValue(DialogueKeys[i]));
        }

        if (TextComponent != null) TextComponent.text = "";

        _dialogueProgressAction = InputSystem.actions.FindAction("UI/ProgressDialogue");
        _playerActionMap = InputSystem.actions.FindActionMap("Player");

        _dialogueProgressAction.performed += ctx => ProgressDialogue();
    }

    public void Interact()
    {
        if (ShouldDisablePlayerMovement)
            _playerActionMap.Disable();


        _dialogueProgressAction.Enable();
        _dialogueEnumerator = StartCoroutine(DisplayText(_dialogueIndex));
    }

    private IEnumerator DisplayText(int dialogueIndex)
    {
        TextComponent.text = "";
        //TO DO: Finish method to display the wanted amount
        //of text / dialogue.

        for (int j = 0; j < _dialogueValues[dialogueIndex].Length; j++)
        {
            TextComponent.text += _dialogueValues[dialogueIndex][j];

            yield return new WaitForSeconds(TextSpeed);
        }
        yield return null;
    }

    private void ProgressDialogue()
    {
        ++_dialogueIndex;
        if (_dialogueIndex >= _dialogueValues.Count)
        {
            StopCoroutine(_dialogueEnumerator);
            _dialogueIndex = 0;

            TextComponent.text = "";
            _dialogueProgressAction.Disable();
            _playerActionMap.Enable();
            return;
        }

        TextComponent.text = "";
        StartCoroutine(DisplayText(_dialogueIndex));
    }
}

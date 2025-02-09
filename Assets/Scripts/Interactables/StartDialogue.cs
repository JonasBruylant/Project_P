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
    public string[] DialogueItemsGotKeys;

    public int ItemIDToCheck = -1;

    private InputAction _dialogueProgressAction;
    private InputActionMap _playerActionMap;
    private List<string> _dialogueValues = new();
    private List<string> _dialogueItemGotValues = new();

    private int _dialogueIndex = 0;
    private Coroutine _dialogueEnumerator;

    void Start()
    {
        AddDialogueValuesToList(_dialogueValues, DialogueKeys);
        AddDialogueValuesToList(_dialogueItemGotValues, DialogueItemsGotKeys);

        if (TextComponent != null) TextComponent.text = "";

        _dialogueProgressAction = InputSystem.actions.FindAction("UI/ProgressDialogue");
        _playerActionMap = InputSystem.actions.FindActionMap("Player");

        _dialogueProgressAction.performed += ctx => ProgressDialogue();
    }

    private void AddDialogueValuesToList(List<string> list, string[] keyValues)
    {
        var dmInstance = DialogueManager.Instance;
        for (int i = 0; i < keyValues.Length; i++)
        {
            if (list.Contains(dmInstance.GetDialogueValue(keyValues[i]))) continue;

            list.Add(dmInstance.GetDialogueValue(keyValues[i]));
        }
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

        if (DataManager.Instance.HasItemFromID(ItemIDToCheck))
        {
            for (int i = 0; i < _dialogueItemGotValues[dialogueIndex].Length; i++)
            {
                TextComponent.text += _dialogueItemGotValues[dialogueIndex][i];

                yield return new WaitForSeconds(TextSpeed);
            }
            yield return null;
        }
        else
        {
            for (int i = 0; i < _dialogueValues[dialogueIndex].Length; i++)
            {
                TextComponent.text += _dialogueValues[dialogueIndex][i];

                yield return new WaitForSeconds(TextSpeed);
            }
            yield return null;
        }
    }

    private void ProgressDialogue()
    {
        ++_dialogueIndex;
        int totalDialogueCount = DataManager.Instance.HasItemFromID(ItemIDToCheck) == true ? _dialogueItemGotValues.Count : _dialogueValues.Count;

        if (_dialogueIndex >= totalDialogueCount)
        {
            StopCoroutine(_dialogueEnumerator);
            _dialogueIndex = 0;

            TextComponent.text = "";
            _dialogueProgressAction.Disable();
            _playerActionMap.Enable();
            return;
        }

        StopCoroutine(_dialogueEnumerator);
        TextComponent.text = "";
        _dialogueEnumerator = StartCoroutine(DisplayText(_dialogueIndex));
    }
}

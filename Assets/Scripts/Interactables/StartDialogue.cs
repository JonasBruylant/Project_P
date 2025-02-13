using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartDialogue : MonoBehaviour, IInteractable
{
    public bool ShouldDisablePlayerMovement = false;
    public TextMeshProUGUI TextComponent;
    public float TextSpeed = 0.02f;
    public string[] DialogueKeys;
    public string[] DialogueItemsGotKeys;

    public bool HasDelay = false;
    public float DialogueStartDelay = 0f;
    public int ItemIDToCheck = -1;

    private List<string> _dialogueValues = new();
    private List<string> _dialogueItemGotValues = new();

    private int _dialogueIndex = 0;
    private Coroutine _dialogueEnumerator;
    private bool _isFirstDialogueLine = true;

    private PlayerInput _playerInput;

    void Start()
    {
        AddDialogueValuesToList(_dialogueValues, DialogueKeys);
        AddDialogueValuesToList(_dialogueItemGotValues, DialogueItemsGotKeys);

        if (TextComponent != null) TextComponent.text = "";

        _playerInput = FindFirstObjectByType<PlayerInput>();
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
        _playerInput.SwitchCurrentActionMap("UI");
        _dialogueEnumerator = StartCoroutine(DisplayText(_dialogueIndex));
    }

    private IEnumerator DisplayText(int dialogueIndex)
    {
        //If the dialogue has a delay, make sure that only the first line of the dialogue has a delay on it.
        if (HasDelay && DialogueStartDelay > 0 && _isFirstDialogueLine)
        {
            _isFirstDialogueLine = false;
            yield return new WaitForSeconds(DialogueStartDelay);
        }

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

    public bool Progress()
    {
        ++_dialogueIndex;
        int totalDialogueCount = DataManager.Instance.HasItemFromID(ItemIDToCheck) ? _dialogueItemGotValues.Count : _dialogueValues.Count;

        StopCoroutine(_dialogueEnumerator);
        TextComponent.text = "";
        
        if (_dialogueIndex >= totalDialogueCount)
        {
            EndDialogue();
            return true;
        }

        _dialogueEnumerator = StartCoroutine(DisplayText(_dialogueIndex));
        return false;
    }

    private void EndDialogue()
    {
        _dialogueIndex = 0;

        _playerInput.SwitchCurrentActionMap("Player");
    }
}

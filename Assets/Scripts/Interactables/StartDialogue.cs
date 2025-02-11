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

    private bool _isInteracted = false;
    private PlayerInput _playerInput;

    void Start()
    {
        AddDialogueValuesToList(_dialogueValues, DialogueKeys);
        AddDialogueValuesToList(_dialogueItemGotValues, DialogueItemsGotKeys);

        if (TextComponent != null) TextComponent.text = "";

        _playerInput = FindFirstObjectByType<PlayerInput>();

        _playerInput.SwitchCurrentActionMap("UI");
        _playerInput.actions["ProgressDialogue"].performed += ctx => OnProgressDialoguePerformed();


        _playerInput.SwitchCurrentActionMap("Player");
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
        _isInteracted = true;

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

    private void OnProgressDialoguePerformed()
    {
        if (!_isInteracted) return;

        // Check if the current action map is UI before proceeding
        if (_playerInput.currentActionMap.name != "UI") return;

        ProgressDialogue();
    }


    private void ProgressDialogue()
    {
        ++_dialogueIndex;
        int totalDialogueCount = DataManager.Instance.HasItemFromID(ItemIDToCheck) == true ? _dialogueItemGotValues.Count : _dialogueValues.Count;

        if (_dialogueIndex >= totalDialogueCount)
        {
            EndDialogue();
            return;
        }

        StopCoroutine(_dialogueEnumerator);
        TextComponent.text = "";
        _dialogueEnumerator = StartCoroutine(DisplayText(_dialogueIndex));
    }

    private void EndDialogue()
    {
        StopCoroutine(_dialogueEnumerator);
        _dialogueIndex = 0;

        TextComponent.text = "";

        _playerInput.SwitchCurrentActionMap("Player");
        _isInteracted = false;
    }
}

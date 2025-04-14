using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableSequence : MonoBehaviour, IInteractable
{
    public List<SequenceAction> sequenceActions = new();
    private int _sequenceIndex = 0;
    private Coroutine _sequenceEnumerator;

    private const float TextSpeed = 0.02f;
    private bool _isSequencePlaying;

    DataManager _dataManager;

    void Start()
    {
        _dataManager = DataManager.Instance;
    }

    public void Interact()
    {
        _dataManager.DisablePlayerMovementAndRotation();
        _sequenceEnumerator = StartCoroutine(PerformSequenceAction(sequenceActions[0]));
    }

    private IEnumerator PerformSequenceAction(SequenceAction action)
    {
        _isSequencePlaying = true;
        switch (action.type)
        {
            case SequenceAction.SequenceActionType.DialogueAction:

                var sentence = DialogueManager.Instance.GetDialogueValue(action.dialogueKey);
                yield return new WaitForSeconds(action.dialogueDelay);
                action.panelComponent.SetActive(true);
                foreach (var c in sentence)
                {
                    action.textComponent.text += c;
                    yield return new WaitForSeconds(TextSpeed);
                }

                break;
            case SequenceAction.SequenceActionType.AnimationAction:
                action.sr.sprite = action.sprite;

                if (sequenceActions[_sequenceIndex + 1] != null
                    && sequenceActions[_sequenceIndex + 1].type == SequenceAction.SequenceActionType.DialogueAction)
                {
                    _isSequencePlaying = false;
                    Progress();
                }
                break;
            case SequenceAction.SequenceActionType.ItemPickup:
                _dataManager.CollectItem(action.itemID);

                action.itemCollider.enabled = false;
                action.itemRenderer.enabled = false;
                if (sequenceActions[_sequenceIndex + 1] != null
                    && sequenceActions[_sequenceIndex + 1].type == SequenceAction.SequenceActionType.DialogueAction)
                {
                    _isSequencePlaying = false;
                    Progress();
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        yield return null;
        _isSequencePlaying = false;
    }

    public bool Progress()
    {
        if(sequenceActions[_sequenceIndex].textComponent != null) sequenceActions[_sequenceIndex].textComponent.text = "";
        if (sequenceActions[_sequenceIndex].panelComponent != null) sequenceActions[_sequenceIndex].panelComponent.SetActive(false);

        ++_sequenceIndex;
        if(_sequenceEnumerator != null) StopCoroutine(_sequenceEnumerator);

        if (_sequenceIndex >= sequenceActions.Count)
        {
            _sequenceIndex = 0;

            _dataManager.EnablePlayerMovementAndRotation();

            return true;
        }

        sequenceActions[_sequenceIndex].textComponent.text = "";
        _sequenceEnumerator = StartCoroutine(PerformSequenceAction(sequenceActions[_sequenceIndex]));
        return false;
    }
}

[Serializable]
public class SequenceAction
{
    public string name;

    [Serializable]
    public enum SequenceActionType
    {
        DialogueAction,
        AnimationAction,
        ItemPickup
    }

    public SequenceActionType type;

    public TextMeshProUGUI textComponent;
    public GameObject panelComponent;
    public string dialogueKey;
    public float dialogueDelay;

    public SpriteRenderer sr;
    public Sprite sprite;

    public int itemID;
    public MeshRenderer itemRenderer;
    public Collider itemCollider;
}
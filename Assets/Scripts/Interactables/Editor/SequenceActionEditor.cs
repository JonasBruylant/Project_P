using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(SequenceAction))]
public class SequenceActionEditor : PropertyDrawer
{
    private SerializedProperty _name;
    private SerializedProperty _type;
    private SerializedProperty _sr;
    private SerializedProperty _sprite;
    private SerializedProperty _textComponent;
    private SerializedProperty _dialogueKey;
    private SerializedProperty _dialogueDelay;

    private int padding = 5;
    
    //how to draw it
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);

        _name = property.FindPropertyRelative("name");
        _type = property.FindPropertyRelative("type");
        _sr = property.FindPropertyRelative("sr");
        _sprite = property.FindPropertyRelative("sprite");
        _textComponent = property.FindPropertyRelative("textComponent");
        _dialogueKey = property.FindPropertyRelative("dialogueKey");
        _dialogueDelay = property.FindPropertyRelative("dialogueDelay");
        
        Rect foldOutBox = new Rect(position.min.x,position.min.y,position.size.x, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldOutBox, property.isExpanded, label);

        if (property.isExpanded)
        {
            Rect typePopup = new Rect(foldOutBox.min.x, foldOutBox.min.x,
                position.size.x, EditorGUIUtility.singleLineHeight);
            
            EditorGUI.PropertyField(typePopup, _name, new GUIContent("Name"));
            typePopup.y += EditorGUIUtility.singleLineHeight + padding;
            
            var action = (SequenceAction)property.boxedValue;
            EditorGUI.PropertyField(typePopup, _type, new GUIContent("Sequence Type"));
            
            if (action.type == SequenceAction.SequenceActionType.AnimationAction)
            {
                typePopup.y += EditorGUIUtility.singleLineHeight + padding;
                EditorGUI.PropertyField(typePopup, _sr, new GUIContent("Sprite Renderer"));
                typePopup.y += EditorGUIUtility.singleLineHeight + padding;
                EditorGUI.PropertyField(typePopup, _sprite, new GUIContent("Sprite"));
            }
            else
            {
                typePopup.y += EditorGUIUtility.singleLineHeight + padding;
                EditorGUI.PropertyField(typePopup, _textComponent, new GUIContent("Text Component"));
                typePopup.y += EditorGUIUtility.singleLineHeight + padding;
                EditorGUI.PropertyField(typePopup, _dialogueKey, new GUIContent("Dialogue Key"));
                typePopup.y += EditorGUIUtility.singleLineHeight + padding;
                EditorGUI.PropertyField(typePopup, _dialogueDelay, new GUIContent("Dialogue Delay"));
            }            
        }

        EditorGUI.EndProperty();
    }

    //Request more vertical spacing, return it
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int totalLines = 1;

        if (property.isExpanded)
        {
            totalLines += 5;
        }

        return (EditorGUIUtility.singleLineHeight + padding) * totalLines - padding;
    }
}

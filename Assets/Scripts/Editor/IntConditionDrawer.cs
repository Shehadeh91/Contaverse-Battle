using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(IntCondition))]
public class IntConditionDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        int size1 = (int)(position.width * 0.35f);
        int size2 = (int)(position.width * 0.3f);
        int size3 = (int)(position.width * 0.35f);

        var int1Rect = new Rect(position.x, position.y, size1, position.height);
        var comparisonRect = new Rect(position.x + size1, position.y, size2 , position.height);
        var int2Rect = new Rect(position.x + position.width - size3, position.y, size3, position.height);
        
        EditorGUI.PropertyField(int1Rect, property.FindPropertyRelative("int1"), GUIContent.none);
        EditorGUI.PropertyField(comparisonRect, property.FindPropertyRelative("comparisonMode"), GUIContent.none);
        EditorGUI.PropertyField(int2Rect, property.FindPropertyRelative("int2"), GUIContent.none);

        EditorGUI.EndProperty();
    }
}
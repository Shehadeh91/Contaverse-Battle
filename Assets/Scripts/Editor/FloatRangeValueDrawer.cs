using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FloatRangeValue))]
public class FloatRangeValueDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var minValue = property.FindPropertyRelative("minValue");
        var value = property.FindPropertyRelative("value");
        var maxValue = property.FindPropertyRelative("maxValue");

        var minRect = new Rect(position.x, position.y, 30, position.height);
        var valueRect = new Rect(position.x + 35, position.y, position.width - 70, position.height);
        var maxRect = new Rect(position.x + position.width - 30, position.y, 30, position.height);
        
        EditorGUI.DelayedFloatField(minRect, minValue, GUIContent.none);
        EditorGUI.Slider(valueRect, value, minValue.floatValue, maxValue.floatValue, GUIContent.none);
        EditorGUI.DelayedFloatField(maxRect, maxValue, GUIContent.none);

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomPropertyDrawer(typeof(IntReference))]
public class IntReferenceDrawer: PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        bool useConstant = property.FindPropertyRelative("useConstant").boolValue;

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var rect = new Rect(position.position, Vector2.one * 20);
        var content = EditorGUIUtility.IconContent("Icon Dropdown");
        var style = new GUIStyle(){fixedWidth = 50f, border = new RectOffset(1,1,1,1)};

        if(EditorGUI.DropdownButton(rect, content, FocusType.Keyboard, style))
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Use Constant"), useConstant, () => SetProperty(property, true));
            menu.AddItem(new GUIContent("Use Variable"), !useConstant, () => SetProperty(property, false));
            menu.ShowAsContext();
        }

        position.position += Vector2.right * 15;
        position.width -= 15;

        if(useConstant)
        {
            EditorGUI.PropertyField(position, property.FindPropertyRelative("constantValue"), GUIContent.none);
            // string newValue = EditorGUI.TextField(position, value.ToString());
            // int.TryParse(newValue, out value);
            // property.FindPropertyRelative("constantValue").intValue = value;
        }
        else
        {
            EditorGUI.ObjectField(position, property.FindPropertyRelative("variable"), GUIContent.none);
        }
        EditorGUI.EndProperty();
    }

    private void SetProperty(SerializedProperty property, bool value)
    {
        var propertyRelative = property.FindPropertyRelative("useConstant");
        propertyRelative.boolValue = value;
        propertyRelative.serializedObject.ApplyModifiedProperties();
    }
}
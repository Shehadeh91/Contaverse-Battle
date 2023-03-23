using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class ChangeTextStringVariable : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textField;
    [SerializeField]private StringVariable stringVariable;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if(textField == null)
            textField = GetComponent<TextMeshProUGUI>();
    }
#endif
    private void OnEnable()
    {
        stringVariable.OnChanged += VariableChanged;
        VariableChanged();
    }
    void OnDisable()
    {
        stringVariable.OnChanged -= VariableChanged;
    }
    public void VariableChanged()
    {
        if (textField == null)
        {
            Debug.LogError("Please assign a text", gameObject);
            return;
        }

        textField.text = stringVariable.Value;
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class ChangeTextIntVariable : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textField;
    [SerializeField] [HorizontalGroup("GroupPrefix")] private bool usePrefix;
    [SerializeField] [LabelWidth(50f)] [HorizontalGroup("GroupPrefix")] [ShowIf("usePrefix")] private string prefix;
    [SerializeField] [HorizontalGroup("GroupSuffix")] private bool useSuffix;
    [SerializeField] [LabelWidth(50f)] [HorizontalGroup("GroupSuffix")] [ShowIf("useSuffix")] private string suffix;
    [SerializeField] private bool useIntRange = false;
    [SerializeField] [HideIf("useIntRange")] private IntVariable intVariable = null;
    [SerializeField] [ShowIf("useIntRange")] private IntRangeVariable intRangeVariable = null;
    [SerializeField] [ShowIf("useIntRange")] private VariableValue intVariableValue = VariableValue.Value;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if(textField == null)
            textField = GetComponent<TextMeshProUGUI>();
    }
#endif
    private void OnEnable()
    {
        if(useIntRange)
        {
            intRangeVariable.OnChanged += VariableChanged;
        }
        else
        {
            intVariable.OnChanged += VariableChanged;
        }
        VariableChanged();
    }
    void OnDisable()
    {
        if (useIntRange)
            intRangeVariable.OnChanged -= VariableChanged;
        else
            intVariable.OnChanged -= VariableChanged;
    }
    public void VariableChanged()
    {
        if (textField == null)
        {
            Debug.LogError("Please assign a text", gameObject);
            return;
        }
        float variableValue = 0f;

        if (useIntRange)
        {
            switch(intVariableValue)
            {
                case VariableValue.Value:
                    variableValue = intRangeVariable.Value.Value;
                    break;
                case VariableValue.MinValue:
                    variableValue = intRangeVariable.Value.MinValue;
                    break;
                case VariableValue.MaxValue:
                    variableValue = intRangeVariable.Value.MaxValue;
                    break;
                case VariableValue.ValueMinDelta:
                    variableValue = intRangeVariable.Value.Value - intRangeVariable.Value.MinValue;
                    break;
                case VariableValue.ValueMaxDelta:
                    variableValue = intRangeVariable.Value.MaxValue - intRangeVariable.Value.Value;
                    break;
                case VariableValue.MinMaxDelta:
                    variableValue = intRangeVariable.Value.MaxValue - intRangeVariable.Value.MinValue;
                    break;
                default:
                    variableValue = intRangeVariable.Value.Value;
                    break;
            }
        }
        else
            variableValue = intVariable.Value;

        string variableValueString = variableValue.ToString();

        if (usePrefix)
            variableValueString = prefix + variableValueString;
        if (useSuffix)
            variableValueString = variableValueString + suffix;

        textField.text = variableValueString;
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class ChangeTextFloatVariable : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textField;
    [SerializeField] [HorizontalGroup("GroupPrefix")] private bool usePrefix;
    [SerializeField] [LabelWidth(50f)] [HorizontalGroup("GroupPrefix")] [ShowIf("usePrefix")] private string prefix;
    [SerializeField] [HorizontalGroup("GroupSuffix")] private bool useSuffix;
    [SerializeField] [LabelWidth(50f)] [HorizontalGroup("GroupSuffix")] [ShowIf("useSuffix")] private string suffix;
    [SerializeField] private int roundDecimals = 1;
    [SerializeField] private bool useFloatRange = false;
    [SerializeField] [HideIf("useFloatRange")] private FloatVariable floatVariable = null;
    [SerializeField] [ShowIf("useFloatRange")] private FloatRangeVariable floatRangeVariable = null;
    [SerializeField] [ShowIf("useFloatRange")] private VariableValue floatVariableValue = VariableValue.Value;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if(textField == null)
            textField = GetComponent<TextMeshProUGUI>();
    }
#endif
    private void OnEnable()
    {
        if(useFloatRange)
        {
            floatRangeVariable.OnChanged += VariableChanged;
            // floatRangeVariable.Value.OnChanged += VariableChanged;
        }
        else
        {
            floatVariable.OnChanged += VariableChanged;
        }
        VariableChanged();
    }
    void OnDisable()
    {
        if (useFloatRange)
        {
            floatRangeVariable.OnChanged -= VariableChanged;
            // floatRangeVariable.Value.OnChanged -= VariableChanged;
        }
        else
            floatVariable.OnChanged -= VariableChanged;
    }
    public void VariableChanged()
    {
        if (textField == null)
        {
            Debug.LogError("Please assign a text", gameObject);
            return;
        }
        float variableValue = 0f;

        if (useFloatRange)
        {
            switch(floatVariableValue)
            {
                case VariableValue.Value:
                    variableValue = floatRangeVariable.Value.Value;
                    break;
                case VariableValue.MinValue:
                    variableValue = floatRangeVariable.Value.MinValue;
                    break;
                case VariableValue.MaxValue:
                    variableValue = floatRangeVariable.Value.MaxValue;
                    break;
                case VariableValue.ValueMinDelta:
                    variableValue = floatRangeVariable.Value.Value - floatRangeVariable.Value.MinValue;
                    break;
                case VariableValue.ValueMaxDelta:
                    variableValue = floatRangeVariable.Value.MaxValue - floatRangeVariable.Value.Value;
                    break;
                case VariableValue.MinMaxDelta:
                    variableValue = floatRangeVariable.Value.MaxValue - floatRangeVariable.Value.MinValue;
                    break;
                default:
                    variableValue = floatRangeVariable.Value.Value;
                    break;
            }
        }
        else
            variableValue = floatVariable.Value;

        string variableValueString = Math.Round((decimal)variableValue, roundDecimals).ToString();

        if (usePrefix)
            variableValueString = prefix + variableValueString;
        if (useSuffix)
            variableValueString = variableValueString + suffix;

        textField.text = variableValueString;
    }
}
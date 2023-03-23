using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SetFloatRangeVariable : MonoBehaviour
{
    [SerializeField] private FloatRangeVariable variable;
    [SerializeField] private bool saveVariable = false;

    public void SetVariableValue(float newValue)
    {
        variable.Value.Value = newValue;
        if(saveVariable)
            variable.Save();
    }
    public void SetVariableMinValue(float newValue)
    {
        variable.Value.MinValue = newValue;
        if(saveVariable)
            variable.Save();
    }
    public void SetVariableMaxValue(float newValue)
    {
        variable.Value.MaxValue = newValue;
        if(saveVariable)
            variable.Save();
    }
    public void SetVariableProgress(float newValue)
    {
        // Debug.Log($"Setting Variable Progress to {newValue}");

        variable.Value.SetProgress(newValue);
        if(saveVariable)
            variable.Save();
    }
    [Button]
    public void SetRandomProgress()
    {
        SetVariableProgress(Random.Range(0f, 1f));
    }
}

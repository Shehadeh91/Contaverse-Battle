using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFloatVariable : MonoBehaviour
{
    [SerializeField]
    private FloatVariable variable;
    public void SetVariableValue(float newValue)
    {
        variable.Value = newValue;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeFillImageVariable : MonoBehaviour
{
    [SerializeField] private bool fillInverted;
    [SerializeField] private Image image;
    [SerializeField] private FloatRangeVariable variable = null;

    private void OnEnable()
    {
        variable.OnChanged += VariableChanged;
        image.fillAmount = variable.Value.GetProgress();
    }
    void OnDisable()
    {
        variable.OnChanged -= VariableChanged;
    }
    public void VariableChanged()
    {
        float newValue = variable.Value.GetProgress();
        newValue = fillInverted ? (1 - newValue) : newValue;
        image.fillAmount = newValue;
    }
}
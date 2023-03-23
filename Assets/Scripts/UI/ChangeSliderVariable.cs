using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChangeSliderVariable : ChangeSlider
{
    [SerializeField]
    private bool receiveChanges = true;
    [SerializeField]
    private bool sendChanges = false;

    [SerializeField]
    private FloatRangeVariable variable = null;
    [SerializeField] private UnityEvent onChanged;

    private void OnEnable()
    {
        if (variable is null)
        {
            Destroy(this);
            Debug.Log("There are missing references for " + gameObject.name, gameObject);
            return;
        }

        if (receiveChanges)
        {
            variable.OnChanged += VariableChanged;
            SetSliderProgress(variable.Value);
        }
    }
    void OnDisable()
    {
        if (receiveChanges)
        {
            variable.OnChanged -= VariableChanged;
        }
    }

    public void VariableChanged()
    {
        SetSliderProgress(variable.Value);
        onChanged?.Invoke();
    }

    public void SliderChanged()
    {
        if (sendChanges)
        {
            variable.Value.SetProgress(slider.value);
        }
    }
}
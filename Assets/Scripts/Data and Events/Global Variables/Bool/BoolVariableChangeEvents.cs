using System;
using UnityEngine;
using UnityEngine.Events;

public class BoolVariableChangeEvents : MonoBehaviour
{
    [SerializeField] private bool invertValue = false;
    [SerializeField] private bool initializeValueOnStart = true;
    [SerializeField] private BoolVariable boolVariable;

    [SerializeField] private BoolUnityEvent onChanged;
    [SerializeField] private UnityEvent onTrue;
    [SerializeField] private UnityEvent onFalse;

    private void OnEnable()
    {
        boolVariable.OnChanged += OnChanged;
        if(initializeValueOnStart)
            OnChanged();
    }
    private void OnDisable()
    {
        boolVariable.OnChanged -= OnChanged;
    }

    private void OnChanged()
    {
        bool outputValue = boolVariable.Value;
        if(invertValue)
            outputValue = !outputValue;

        onChanged?.Invoke(outputValue);
        if(outputValue)
        {
            onTrue?.Invoke();
        }
        else
        {
            onFalse?.Invoke();
        }
    }
}

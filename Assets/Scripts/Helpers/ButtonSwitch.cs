using System;
using UnityEngine;
using UnityEngine.Events;

public class ButtonSwitch : MonoBehaviour
{
    public bool isOn = false;

    public UnityEvent onEnabled;
    public UnityEvent onDisabled;
    public BoolUnityEvent onChanged;

    private void Start()
    {
        onChanged.Invoke(isOn);

        if (isOn)
            onEnabled.Invoke();
        else
            onDisabled.Invoke();
    }

    public void Switch()
    {
        isOn = !isOn;

        onChanged.Invoke(isOn);

        if (isOn)
            onEnabled.Invoke();
        else
            onDisabled.Invoke();
    }
    public void Switch(bool isOn)
    {
        this.isOn = isOn;

        onChanged.Invoke(isOn);

        if (isOn)
            onEnabled.Invoke();
        else
            onDisabled.Invoke();
    }
}

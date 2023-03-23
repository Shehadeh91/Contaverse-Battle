using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Variable/Float Variable")]
public class FloatVariable : GlobalVariable
{
    [SerializeField]
    private float value;
    [SerializeField]
    private float defaultValue;
    public Action OnChanged;

    public float Value
    {
        get
        {
            return this.value;
        }
        set
        {
            if (this.value != value)
            {
                this.value = value;
                OnChanged?.Invoke();
            }
        }
    }

    public override void Save()
    {
        PlayerPrefs.SetFloat(variableName, value);
        base.Save();
    }

    public override void InitializeVariable()
    {
        base.InitializeVariable();

        if (isSaveable && PlayerPrefs.HasKey(variableName))
        {
            value = PlayerPrefs.GetFloat(variableName);
        }
        else
        {
            value = defaultValue;
        }

        isInitialized = true;
    }

    public void OnChange()
    {
        OnChanged?.Invoke();
    }

    ///Sets the value without calling the onchanged events
    public void SetValueSilent(float newValue)
    {
        value = newValue;
    }
    public void SetValueLoud(float newValue)
    {
        value = newValue;
        OnChanged?.Invoke();
    }
    public bool ClampValue(float min, float max)
    {
        if (value > max)
        {
            value = max;
            OnChanged?.Invoke();
            return true;
        }
        else if (value < min)
        {
            value = min;
            OnChanged?.Invoke();
            return true;
        }
        return false;
    }
}
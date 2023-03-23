using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Variable/Int Range Variable")]
public class IntRangeVariable : GlobalVariable
{
    [SerializeField]
    private IntRangeValue value;
    [SerializeField]
    private IntRangeValue defaultValue; 
    public event Event OnChanged;

    public IntRangeValue Value
    {
        get
        {
            return this.value;
        }
        set
        {
            if(this.value != value)
                OnChanged?.Invoke();
            this.value = value;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        value.OnChanged += ()=> OnChanged?.Invoke();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        value.OnChanged -= ()=> OnChanged?.Invoke();
    }

    public override void Save()
    {
        PlayerPrefs.SetInt(variableName, value.Value);
        base.Save();
    }

    public override void InitializeVariable()
    {
        base.InitializeVariable();

        if(isSaveable && PlayerPrefs.HasKey(variableName))
        {
            value = defaultValue.Clone();
            value.Value = PlayerPrefs.GetInt(variableName);
        }
        else
        {
            value = defaultValue.Clone();
        }

        isInitialized = true;
    }
}
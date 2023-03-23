using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Variable/Float Range Variable")]
public class FloatRangeVariable : GlobalVariable
{
    [SerializeField]
    private FloatRangeValue value;
    public FloatRangeValue defaultValue;
    public Action OnChanged;

    public FloatRangeValue Value
    {
        get
        {
            return this.value;
        }
        set
        {
            if (this.value == value)
                return;

            this.value.OnChanged -= ()=> OnChanged?.Invoke();

            this.value = value;

            this.value.OnChanged += OnChanged.Invoke;

                OnChanged?.Invoke();
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
        PlayerPrefs.SetFloat(variableName, value.Value);
        base.Save();
    }

    public override void InitializeVariable()
    {
        base.InitializeVariable();

        if(isSaveable && PlayerPrefs.HasKey(variableName))
        {
            value = defaultValue.Clone();
            value.Value = PlayerPrefs.GetFloat(variableName);
        }
        else
        {
            value = defaultValue.Clone();
        }

        isInitialized = true;
    }
    public void SetValueSilent(float value)
    {
        this.value.Value = value;
    }

#if UNITY_EDITOR
    protected void OnValidate()
    {
        value.OnChanged?.Invoke();
        OnChanged?.Invoke();
    }
#endif
}
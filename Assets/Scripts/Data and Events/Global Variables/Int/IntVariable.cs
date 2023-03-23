using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Variable/Integer Variable")]
public class IntVariable : GlobalVariable
{
    [SerializeField]
    private int value;
    [SerializeField]
    private int defaultValue; 
    public Action OnChanged;

    public int Value
    {
        get
        {
            return this.value;
        }
        set
        {
            if(this.value != value)
            {
                this.value = value;
                OnChanged?.Invoke();
            }
        }
    }

    public override void Save()
    {
        PlayerPrefs.SetInt(variableName, value);
        base.Save();
    }

    public override void InitializeVariable()
    {
        base.InitializeVariable();

        if(isSaveable && PlayerPrefs.HasKey(variableName))
        {
            value = PlayerPrefs.GetInt(variableName);
        }
        else
        {
            value = defaultValue;
        }

        isInitialized = true;
    }
}
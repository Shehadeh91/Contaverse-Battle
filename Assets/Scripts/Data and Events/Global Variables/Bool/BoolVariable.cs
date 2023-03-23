using UnityEngine;

[CreateAssetMenu(menuName = "Variable/Bool Variable")]
public class BoolVariable : GlobalVariable
{
    [SerializeField]
    private bool value;
    [SerializeField]
    private bool defaultValue;
    public event Event OnChanged;

    public virtual bool Value
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
    
    protected virtual void RaiseOnChanged() => OnChanged?.Invoke();

    public override void Save()
    {
        PlayerPrefs.SetInt(variableName, value ? 1 : 0);
        base.Save();
    }

    public override void InitializeVariable()
    {
        base.InitializeVariable();

        if(isSaveable && PlayerPrefs.HasKey(variableName))
        {
            value = PlayerPrefs.GetInt(variableName) == 1;
        }
        else
        {
            value = defaultValue;
        }

        isInitialized = true;
    }
}
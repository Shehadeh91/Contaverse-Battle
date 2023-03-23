using UnityEngine;

[CreateAssetMenu(menuName = "Variable/String Variable")]
public class StringVariable : GlobalVariable
{
    [SerializeField]
    private string value;
    [SerializeField]
    private string defaultValue; 
    public event Event OnChanged;

    public string Value
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
        PlayerPrefs.SetString(variableName, value);
        base.Save();
    }

    public override void InitializeVariable()
    {
        base.InitializeVariable();

        if(isSaveable && PlayerPrefs.HasKey(variableName))
        {
            value = PlayerPrefs.GetString(variableName);
        }
        else
        {
            value = defaultValue;
        }

        isInitialized = true;
    }
}
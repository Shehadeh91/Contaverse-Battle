using UnityEngine;

[CreateAssetMenu(menuName = "Variable/Control Input Action")]
public class ControlInputAction : GlobalVariable
{
    public string displayedActionName;
    public string inputActionName;

    public string bindingOverride;

    public override void Save()
    {
        PlayerPrefs.SetString(variableName, bindingOverride);
    }

    public override void InitializeVariable()
    {
        base.InitializeVariable();

        if(isSaveable && PlayerPrefs.HasKey(variableName))
        {
            bindingOverride = PlayerPrefs.GetString(variableName);
        }
        else
        {
            bindingOverride = "";
        }

        isInitialized = true;
    }
}
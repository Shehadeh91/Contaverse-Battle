using UnityEngine;

[System.Serializable]
public class BoolReference
{
    public bool useConstant = true;
    public bool constantValue;
    public BoolVariable variable;

    public bool Value
    {
        get
        {
            if(useConstant)
                return constantValue;
            else
            {
                if(variable == null)
                {
                    Debug.LogError("The Variable was not set.");
                    useConstant = true;
                    return constantValue;
                }
                else
                {
                    return variable.Value;
                }
            }
        }
        set
        {
            if(useConstant)
                constantValue = value;
            else
            {
                if(variable == null)
                {
                    Debug.LogError("The Variable was not set.");
                    useConstant = true;
                }
                else
                {
                    variable.Value = value;
                }
            }
        }
    }

    public BoolReference(bool value)
    {
        constantValue = value;
    }
}
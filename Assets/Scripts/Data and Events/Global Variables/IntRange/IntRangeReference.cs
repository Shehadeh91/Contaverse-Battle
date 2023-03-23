using UnityEngine;

[System.Serializable]
public class IntRangeReference
{
    public bool useConstant = true;
    public IntRangeValue constantValue;
    public IntRangeVariable variable;

    public IntRangeValue Value
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
}